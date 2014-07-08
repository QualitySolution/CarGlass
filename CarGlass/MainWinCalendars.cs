using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data.MySqlClient;
using CarGlass;
using QSProjectsLib;

public partial class MainWindow: Gtk.Window
{

	void PrerareCalendars()
	{
		orderscalendar1.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar1.SetTimeRange(9, 20);
		orderscalendar1.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar1.OrdersTypes = new Dictionary<int, string>();
		orderscalendar1.OrdersTypes.Add((int)Order.OrderType.install, "Установка стекл");
		orderscalendar1.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar1.NewOrder += OnNewOrder;

		orderscalendar2.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar2.SetTimeRange(9, 20);
		orderscalendar2.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar2.OrdersTypes = new Dictionary<int, string>();
		orderscalendar2.OrdersTypes.Add((int)Order.OrderType.tinting, "Тонировка");
		orderscalendar2.OrdersTypes.Add((int)Order.OrderType.repair, "Ремонт сколов");
		orderscalendar2.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar2.NewOrder += OnNewOrder;

		orderscalendar1.RefreshOrders();
	}

	protected void OnRefreshCalendarEvent(object sender, RefreshOrdersEventArgs arg)
	{
		OrdersCalendar Calendar = (OrdersCalendar)sender;

		MainClass.StatusMessage(String.Format ("Запрос заказов на {0:d}...", arg.StartDate));
		string sql = "SELECT orders.*, models.name as model, marks.name as mark, status.color, stocks.name as stock, " +
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
					order.Text = String.Format("{0} {1}\n{2}",rdr["mark"].ToString(), rdr["model"].ToString(), rdr["phone"].ToString() );
					if(type == Order.OrderType.install)
					{
						order.FullText = String.Format("Состояние: {0}\nАвтомобиль: {1} {2}\nЕврокод: {3}\nПроизводитель: {4}\nСклад:{5}\nТелефон: {6}\nСтоимость: {7:C}\n{8}",
							rdr["status"].ToString(),
							rdr["mark"].ToString(),
							rdr["model"].ToString(),
							rdr["eurocode"].ToString(),
							rdr["manufacturer"].ToString(),
							rdr["stock"].ToString(),
							rdr["phone"].ToString(),
							DBWorks.GetDecimal(rdr, "sum", 0),
							rdr["comment"].ToString()
						);
					}
					else
					{
						order.FullText = String.Format("Заказ на {0}\nСостояние: {1}\nАвтомобиль: {2} {3}\nТелефон: {4}\nСтоимость: {5:C}\n{6}",
							type == Order.OrderType.tinting ? "тонировку" : "ремонт",
							rdr["status"].ToString(),
							rdr["mark"].ToString(),
							rdr["model"].ToString(),
							rdr["phone"].ToString(),
							DBWorks.GetDecimal(rdr, "sum", 0),
							rdr["comment"].ToString()
						);
					}
					order.Color = DBWorks.GetString(rdr, "color", "");
					order.Type = (int) type;
					order.Calendar = Calendar;
					order.DeleteOrder += OnDeleteOrder;
					order.OpenOrder += OnOpenOrder;
					order.TimeChanged += OnChangeTimeOrderEvent;
					int day = (order.Date - Calendar.StartDate).Days;
					Calendar.AddItem(day, order.Hour, order);
				}
			}
			MainClass.StatusMessage("Ok");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
			MainClass.StatusMessage("Ошибка получения списка заказов!");
			QSMain.ErrorMessage(this,ex);
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
		Order OrderWin = new Order((Order.OrderType)arg.OrderType, arg.Date, arg.Hour);
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

		MainClass.StatusMessage(String.Format ("Изменение времени заказа на {0:d} {1}ч...", arg.Date, arg.Hour));
		string sql = "UPDATE orders SET date = @date, hour = @hour WHERE id = @id ";
		QSMain.CheckConnectionAlive();
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@date", arg.Date);
			cmd.Parameters.AddWithValue("@hour", arg.Hour);
			cmd.Parameters.AddWithValue("@id", order.id);

			cmd.ExecuteNonQuery();
			MainClass.StatusMessage("Ok");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
			MainClass.StatusMessage("Ошибка записи заказа!");
			QSMain.ErrorMessage(this,ex);
		}

	}
}