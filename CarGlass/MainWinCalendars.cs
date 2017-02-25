using System;
using System.Collections.Generic;
using CarGlass;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

public partial class MainWindow: Gtk.Window
{

	void PrerareCalendars()
	{
		var installServices = new Dictionary<Order.OrderType, string>{
			{Order.OrderType.install, "Установка стекл"}
		};

		var tintingServices = new Dictionary<Order.OrderType, string>{
			{ Order.OrderType.tinting, "Тонировка" },
			{ Order.OrderType.repair, "Ремонт сколов" }
		};

		orderscalendar1.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar1.SetTimeRange(9, 22);
		orderscalendar1.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar1.OrdersTypes = installServices;
		orderscalendar1.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar1.NewOrder += OnNewOrder;

		orderscalendar2.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar2.SetTimeRange(9, 22);
		orderscalendar2.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar2.OrdersTypes = tintingServices;
		orderscalendar2.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar2.NewOrder += OnNewOrder;

		orderscalendar1.RefreshOrders();
	}

	protected void OnRefreshCalendarEvent(object sender, RefreshOrdersEventArgs arg)
	{
		OrdersCalendar Calendar = (OrdersCalendar)sender;

		logger.Info("Запрос заказов на {0:d}...", arg.StartDate);
		string sql = "SELECT orders.*, models.name as model, marks.name as mark, status.color, stocks.name as stock, stocks.color as stockcolor, " +
			"status.name as status, manufacturers.name as manufacturer, tablesum.sum FROM orders " +
			"LEFT JOIN models ON models.id = orders.car_model_id " +
			"LEFT JOIN marks ON marks.id = models.mark_id " +
			"LEFT JOIN status ON status.id = orders.status_id " +
			"LEFT JOIN stocks ON stocks.id = orders.stock_id " +
			"LEFT JOIN manufacturers ON manufacturers.id = orders.manufacturer_id " +
			"LEFT JOIN (" +
			"SELECT order_id, SUM(cost) as sum FROM order_pays GROUP BY order_id) as tablesum " +
			"ON tablesum.order_id = orders.id " +
			"WHERE date BETWEEN @start AND @end ";
		if (Calendar == orderscalendar1)
			sql += "AND orders.type = 'install' ";
		else 
			sql += "AND orders.type <> 'install' ";
		QSMain.CheckConnectionAlive();
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@start", arg.StartDate);
			cmd.Parameters.AddWithValue("@end", arg.StartDate.AddDays(6));

			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				Calendar.ClearTimeMap();
				while(rdr.Read())
				{
					CalendarItem order = new CalendarItem(rdr.GetDateTime("date"),
						rdr.GetInt32("hour")
					);
					Order.OrderType type = (Order.OrderType)Enum.Parse(typeof(Order.OrderType), rdr["type"].ToString());
					order.id = rdr.GetInt32("id");
					order.Text = String.Format("{0} {1}\n{2}\n{3}",rdr["mark"], rdr["model"], rdr["phone"], rdr["comment"] );
					if(type == Order.OrderType.install)
					{
						order.FullText = String.Format("Состояние: {0}\nАвтомобиль: {1} {2}\nЕврокод: {3}\nПроизводитель: {4}\nСклад:{5}\nТелефон: {6}\nСтоимость: {7:C}\n{8}",
							rdr["status"],
							rdr["mark"],
							rdr["model"],
							rdr["eurocode"],
							rdr["manufacturer"],
							rdr["stock"],
							rdr["phone"],
							DBWorks.GetDecimal(rdr, "sum", 0),
							rdr["comment"]
						);
					}
					else
					{
						order.FullText = String.Format("Заказ на {0}\nСостояние: {1}\nАвтомобиль: {2} {3}\nТелефон: {4}\nСтоимость: {5:C}\n{6}",
							type == Order.OrderType.tinting ? "тонировку" : "ремонт",
							rdr["status"],
							rdr["mark"],
							rdr["model"],
							rdr["phone"],
							DBWorks.GetDecimal(rdr, "sum", 0),
							rdr["comment"]
						);
					}
					order.Color = DBWorks.GetString(rdr, "color", "");
					order.TagColor = DBWorks.GetString(rdr, "stockcolor", "");
					if(rdr["stock"].ToString().Length > 0 && rdr["stockcolor"] != DBNull.Value)
						order.Tag = rdr["stock"].ToString().Substring(0, 1);
					order.Type = (int) type;
					order.Calendar = Calendar;
					order.DeleteOrder += OnDeleteOrder;
					order.OpenOrder += OnOpenOrder;
					order.TimeChanged += OnChangeTimeOrderEvent;
					int day = (order.Date - Calendar.StartDate).Days;
					Calendar.AddItem(day, order.Hour, order);
				}
			}
			logger.Info("Ok");
		}
		catch (Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка получения списка заказов!", logger, ex);
		}

	}

	protected void OnOpenOrder(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		Order OrderWin = new Order((Order.OrderType)item.Type);
		OrderWin.Fill(item.id);
		OrderWin.Show();
		if ((ResponseType)OrderWin.Run() == ResponseType.Ok)
			item.Calendar.RefreshOrders();
		OrderWin.Destroy();
	}

	protected void OnDeleteOrder(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		Delete winDelete = new Delete();
		if (winDelete.RunDeletion("orders", item.id))
			item.Calendar.RefreshOrders();
	}

	protected void OnNewOrder(object sender, NewOrderEventArgs arg)
	{
		Order OrderWin = new Order(arg.OrderType, arg.Date, arg.Hour);
		OrderWin.NewItem = true;
		OrderWin.Show();
		int result = OrderWin.Run();
		if (result == (int)ResponseType.Ok)
			((OrdersCalendar)sender).RefreshOrders();
		OrderWin.Destroy();
	}

	protected void OnChangeTimeOrderEvent(object sender, CalendarItem.TimeChangedEventArgs arg)
	{
		CalendarItem order = (CalendarItem)sender;

		logger.Info("Изменение времени заказа на {0:d} {1}ч...", arg.Date, arg.Hour);
		string sql = "UPDATE orders SET date = @date, hour = @hour WHERE id = @id ";
		QSMain.CheckConnectionAlive();
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@date", arg.Date);
			cmd.Parameters.AddWithValue("@hour", arg.Hour);
			cmd.Parameters.AddWithValue("@id", order.id);

			cmd.ExecuteNonQuery();
			logger.Info("Ok");
		}
		catch (Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка записи заказа!", logger, ex);
		}

	}
}