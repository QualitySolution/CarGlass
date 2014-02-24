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
		orderscalendar1.OrdersTypes = new Dictionary<int, string>();
		orderscalendar1.OrdersTypes.Add((int)Order.OrderType.install, "Установка стекл");
		orderscalendar1.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar1.NewOrder += OnNewOrder;

		orderscalendar2.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar2.SetTimeRange(9, 20);
		orderscalendar2.OrdersTypes = new Dictionary<int, string>();
		orderscalendar2.OrdersTypes.Add((int)Order.OrderType.tinting, "Тонировка");
		orderscalendar2.OrdersTypes.Add((int)Order.OrderType.repair, "Ремонт сколов");
		orderscalendar2.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar2.NewOrder += OnNewOrder;
	}

	protected void OnRefreshCalendarEvent(object sender, OrdersCalendar.RefreshOrdersEventArgs arg)
	{
		OrdersCalendar Calendar = (OrdersCalendar)sender;

		MainClass.StatusMessage(String.Format ("Запрос заказов на {0:d}...", arg.StartDate));
		string sql = "SELECT orders.*, models.name as model, marks.name as mark, status.color FROM orders " +
			"LEFT JOIN models ON models.id = orders.car_model_id " +
			"LEFT JOIN marks ON marks.id = models.mark_id " +
			"LEFT JOIN status ON status.id = orders.status_id " +
			"WHERE date BETWEEN @start AND @end ";
		if (Calendar == orderscalendar1)
			sql += "AND orders.type = 'install' ";
		else 
			sql += "AND orders.type <> 'install' ";
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
					order.id = rdr.GetInt32("id");
					order.Text = String.Format("{0} {1}",rdr["mark"].ToString(), rdr["model"].ToString() );
					order.Color = DBWorks.GetString(rdr, "color", "");
					order.Type = (int) Enum.Parse(typeof(Order.OrderType), rdr["type"].ToString());
					order.OpenOrder += OnOpenOrder;
					int day = (order.Date - Calendar.StartDate).Days;
					Calendar.TimeMap[day, order.Hour] = order;
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
		OrderWin.Run();
		OrderWin.Destroy();
	}

	protected void OnNewOrder(object sender, OrdersCalendar.NewOrderEventArgs arg)
	{
		Order OrderWin = new Order((Order.OrderType)arg.OrderType, arg.Date, arg.Hour);
		OrderWin.NewItem = true;
		OrderWin.Show();
		int result = OrderWin.Run();
		if (result == (int)ResponseType.Ok)
			((OrdersCalendar)sender).RefreshOrders();
		OrderWin.Destroy();
	}

}