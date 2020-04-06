using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass;
using CarGlass.Dialogs;
using CarGlass.Domain;
using Gamma.Utilities;
using Gtk;
using MySql.Data.MySqlClient;
using QSOrmProject;
using QSProjectsLib;

public partial class MainWindow: FakeTDITabGtkWindowBase
{

	void PrerareCalendars()
	{
		var installServices = new List<OrderType>{
			OrderType.install,
			OrderType.repair,
			OrderType.polishing,
			OrderType.other,
		};

		var tintingServices = new List<OrderType>{
			OrderType.tinting,
			OrderType.repair,
			OrderType.polishing,
			OrderType.armoring,
			OrderType.other,
		};

		orderscalendar1.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar1.SetTimeRange(9, 22);
		orderscalendar1.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar1.PointNumber = 1;
		orderscalendar1.CalendarNumber = 1;
		orderscalendar1.OrdersTypes = installServices;
		orderscalendar1.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar1.NewOrder += OnNewOrder;
		orderscalendar1.NewSheduleWork += OnNewSheduleWork;

		orderscalendar2.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar2.SetTimeRange(9, 22);
		orderscalendar2.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar2.PointNumber = 1;
		orderscalendar2.CalendarNumber = 2;
		orderscalendar2.OrdersTypes = tintingServices;
		orderscalendar2.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar2.NewOrder += OnNewOrder;
		orderscalendar2.NewSheduleWork += OnNewSheduleWork;

		orderscalendar3.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar3.SetTimeRange(9, 22);
		orderscalendar3.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar3.PointNumber = 2;
		orderscalendar3.CalendarNumber = 1;
		orderscalendar3.OrdersTypes = installServices;
		orderscalendar3.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar3.NewOrder += OnNewOrder;
		orderscalendar3.NewSheduleWork += OnNewSheduleWork;

		orderscalendar4.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar4.SetTimeRange(9, 22);
		orderscalendar4.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar4.PointNumber = 2;
		orderscalendar4.CalendarNumber = 2;
		orderscalendar4.OrdersTypes = tintingServices;
		orderscalendar4.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar4.NewOrder += OnNewOrder;
		orderscalendar4.NewSheduleWork += OnNewSheduleWork;

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
			"WHERE date BETWEEN @start AND @end " +
			"AND point_number = @point " +
			"AND calendar_number = @calendar ";
		
		QSMain.CheckConnectionAlive();
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@start", arg.StartDate);
			cmd.Parameters.AddWithValue("@end", arg.StartDate.AddDays(6));
			cmd.Parameters.AddWithValue("@point", Calendar.PointNumber);
			cmd.Parameters.AddWithValue("@calendar", Calendar.CalendarNumber);

			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				Calendar.ClearTimeMap();
				while(rdr.Read())
				{
					CalendarItem order = new CalendarItem(rdr.GetDateTime("date"),
						rdr.GetInt32("hour")
					);
					OrderType type = (OrderType)Enum.Parse(typeof(OrderType), rdr["type"].ToString());
					order.id = rdr.GetInt32("id");
					order.Text = String.Format("{0} {1}\n{2}\n{3}",rdr["mark"], rdr["model"], rdr["phone"], rdr["comment"] );
					if(type == OrderType.install)
					{
						order.FullText = String.Format("{9}\nСостояние: {0}\nАвтомобиль: {1} {2}\nЕврокод: {3}\nПроизводитель: {4}\nСклад:{5}\nТелефон: {6}\nСтоимость: {7:C}\n{8}",
							rdr["status"],
							rdr["mark"],
							rdr["model"],
							rdr["eurocode"],
							rdr["manufacturer"],
							rdr["stock"],
							rdr["phone"],
							DBWorks.GetDecimal(rdr, "sum", 0),
							rdr["comment"],
						                               type.GetEnumTitle()
						);
					}
					else
					{
						order.FullText = String.Format("{0}\nСостояние: {1}\nАвтомобиль: {2} {3}\nТелефон: {4}\nСтоимость: {5:C}\n{6}",
						                               type.GetEnumTitle(),
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
					order.Type = type;
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

		logger.Info("Запрос расписания работы сотрудников на {0:d}...", arg.StartDate);
		sql = "select shw.id, shw.date_work " +
			"from shedule_works shw " +
			"WHERE shw.date_work BETWEEN @start AND @end " +
			"AND  shw.point_number = @point AND shw.calendar_number = @calendar";

		List<CalendarItem> calendarItemsShedule = new List<CalendarItem>();
		QSMain.CheckConnectionAlive();
		try
		{
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@start", arg.StartDate);
			cmd.Parameters.AddWithValue("@end", arg.StartDate.AddDays(6));
			cmd.Parameters.AddWithValue("@point", Calendar.PointNumber);
			cmd.Parameters.AddWithValue("@calendar", Calendar.CalendarNumber);

			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					CalendarItem shedule = new CalendarItem(rdr.GetDateTime("date_work"), 23);

					shedule.id = rdr.GetInt32("id");
					shedule.Tag = "";
					shedule.Color = shedule.TagColor = "#ffffff";
					shedule.Calendar = Calendar;
					shedule.DeleteOrder += OnDeleteShedule;
					shedule.OpenOrder += OnOpenSheduleWork;
					shedule.isSetSheduleWork = true;
					calendarItemsShedule.Add(shedule);
				}
			}

			foreach( var sh in calendarItemsShedule)
			{
				sh.Text = getEmployeeInShedule(sh.id);
				Calendar.AddItem((sh.Date - Calendar.StartDate).Days, 23, sh);
			}
		}

		catch(Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка получения списка сотрудников!", logger, ex);
		}

	}

	private string getEmployeeInShedule(int idShedule)
	{
		string sql = " select shew.id_shedule_works, shew.id_employee, emp.id, emp.first_name, emp.last_name, emp.patronymic" +
			"  FROM shedule_employee_works shew " +
			" LEFT JOIN employees emp on emp.id = shew.id_employee" +
			" WHERE shew.id_shedule_works = @id; ";
		string text = "";
		QSMain.CheckConnectionAlive();
		try
		{

			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

			cmd.Parameters.AddWithValue("@id", idShedule);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					text += String.Format("{0} {1} {2}\n", rdr["first_name"], rdr["last_name"], rdr["patronymic"]); 
				}
			return text;
			}
		}
		catch(Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка получения списка сотрудников!", logger, ex);
			return text;
		}

	}

	protected void OnOpenSheduleWork(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		SheduleDlg shedule = new SheduleDlg(item.id);
		shedule.Show();
		if((ResponseType)shedule.Run() == ResponseType.Ok)
			item.Calendar.RefreshOrders();
		shedule.Destroy();
	}

	protected void OnOpenOrder(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		OrderDlg OrderWin = new OrderDlg(item.id);
		OrderWin.Show();
		if ((ResponseType)OrderWin.Run() == ResponseType.Ok)
			item.Calendar.RefreshOrders();
		OrderWin.Destroy();
	}

	protected void OnDeleteShedule(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		Delete winDelete = new Delete();
		if(winDelete.RunDeletion("shedule_works", item.id))
			item.Calendar.RefreshOrders();
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
		OrderDlg OrderWin = new OrderDlg(arg.PointNumber, arg.CalendarNumber, arg.OrderType, arg.Date, arg.Hour);
		OrderWin.Show();
		int result = OrderWin.Run();
		if (result == (int)ResponseType.Ok)
			((OrdersCalendar)sender).RefreshOrders();
		OrderWin.Destroy();
	}

	protected void OnNewSheduleWork(object sender, NewSheduleWorkEventArgs arg)
	{
		SheduleDlg frmSheduleWork = new SheduleDlg(arg.PointNumber, arg.CalendarNumber, arg.Date);
		frmSheduleWork.Show();
		int result = frmSheduleWork.Run();
		if(result == (int)ResponseType.Ok)
			((OrdersCalendar)sender).RefreshOrders();
		frmSheduleWork.Destroy();
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

			if (arg.Date < order.Calendar.StartDate || arg.Date > order.Calendar.StartDate.AddDays(7))
				order.Calendar.RefreshOrders();
		}
		catch (Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка записи заказа!", logger, ex);
		}
	}
}