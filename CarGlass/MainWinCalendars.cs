using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass;
using CarGlass.Dialogs;
using CarGlass.Domain;
using Gamma.Utilities;
using Gtk;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using QSOrmProject;
using QSProjectsLib;

public partial class MainWindow: FakeTDITabGtkWindowBase
{
	
	void PrerareCalendars()
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		var listOrderTypesOrderCalendar1 = UoW.Session.QueryOver<OrderTypeClass>().List().Where(x => x.PositionInTabs.ToUpper().Contains(label1.LabelProp.ToUpper())).ToList();
		var listOrderTypesOrderCalendar2 = UoW.Session.QueryOver<OrderTypeClass>().List().Where(x => x.PositionInTabs.ToUpper().Contains(label2.LabelProp.ToUpper())).ToList();
		var listOrderTypesOrderCalendar3 = UoW.Session.QueryOver<OrderTypeClass>().List().Where(x => x.PositionInTabs.ToUpper().Contains(label3.LabelProp.ToUpper())).ToList();
		var listOrderTypesOrderCalendar4 = UoW.Session.QueryOver<OrderTypeClass>().List().Where(x => x.PositionInTabs.ToUpper().Contains(label4.LabelProp.ToUpper())).ToList();

		orderscalendar1.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar1.SetTimeRange(9, 21);
		orderscalendar1.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar1.PointNumber = 1;
		orderscalendar1.CalendarNumber = 1;
		orderscalendar1.OrdersTypes = listOrderTypesOrderCalendar1;
		orderscalendar1.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar1.NewOrder += OnNewOrder;
		orderscalendar1.NewSheduleWork += OnNewSheduleWork;
		orderscalendar1.NewNote += OnNewNote;

		orderscalendar2.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar2.SetTimeRange(9, 21);
		orderscalendar2.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar2.PointNumber = 1;
		orderscalendar2.CalendarNumber = 2;
		orderscalendar2.OrdersTypes = listOrderTypesOrderCalendar2;
		orderscalendar2.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar2.NewOrder += OnNewOrder;
		orderscalendar2.NewSheduleWork += OnNewSheduleWork;
		orderscalendar2.NewNote += OnNewNote;

		orderscalendar3.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.DayOfWeek + 6) % 7));
		orderscalendar3.SetTimeRange(9, 21);
		orderscalendar3.BackgroundColor = new Gdk.Color(234, 230, 255);
		orderscalendar3.PointNumber = 2;
		orderscalendar3.CalendarNumber = 1;
		orderscalendar3.OrdersTypes = listOrderTypesOrderCalendar3;
		orderscalendar3.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar3.NewOrder += OnNewOrder;
		orderscalendar3.NewSheduleWork += OnNewSheduleWork;
		orderscalendar3.NewNote += OnNewNote;

		orderscalendar4.StartDate = DateTime.Today.AddDays(-(((int)DateTime.Today.Date.DayOfWeek + 6) % 7));
		orderscalendar4.SetTimeRange(9, 21);
		orderscalendar4.BackgroundColor = new Gdk.Color(255, 230, 230);
		orderscalendar4.PointNumber = 2;
		orderscalendar4.CalendarNumber = 2;
		orderscalendar4.OrdersTypes = listOrderTypesOrderCalendar4;
		orderscalendar4.NeedRefreshOrders += OnRefreshCalendarEvent;
		orderscalendar4.NewOrder += OnNewOrder;
		orderscalendar4.NewSheduleWork += OnNewSheduleWork;
		orderscalendar4.NewNote += OnNewNote;

		orderscalendar1.RefreshOrders();
	}

	protected void OnRefreshCalendarEvent(object sender, RefreshOrdersEventArgs arg)
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
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
					order.id = rdr.GetInt32("id");
					if(rdr["id_order_type"] == DBNull.Value)
						throw new InvalidCastException($"В заказе {order.id} id_order_type = null");
					OrderTypeClass orderTypeClass = UoW.Session.QueryOver<OrderTypeClass>().List().FirstOrDefault(x => x.Id == rdr.GetInt32("id_order_type"));
					order.Text = String.Format("{0} {1}\n{2}\n{3}",rdr["mark"], rdr["model"], rdr["phone"], rdr["comment"] );
					if(orderTypeClass.IsShowAdditionalWidgets)
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
							orderTypeClass.Name
						                               
						);
					}
					else
					{
						order.FullText = String.Format("{0}\nСостояние: {1}\nАвтомобиль: {2} {3}\nТелефон: {4}\nСтоимость: {5:C}\n{6}",
							 orderTypeClass.Name,
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
					order.OrderType = orderTypeClass;
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
					shedule.Color = shedule.TagColor = GetBgColor(Calendar);
					shedule.Calendar = Calendar;
					shedule.DeleteOrder += OnDeleteShedule;
					shedule.OpenOrder += OnOpenSheduleWork;
					shedule.isNoOrder = true;
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

		IList<Note> listNote = UoW.Session.QueryOver<Note>().List().Where(x => x.Date >= arg.StartDate && x.Date <= arg.StartDate.AddDays(6) &&
								x.PointNumber == Calendar.PointNumber && x.CalendarNumber == Calendar.CalendarNumber).ToList();

		if(listNote != null && listNote.Count > 0)
			foreach(var note in listNote)
			{
				CalendarItem calendarItemNote = new CalendarItem(note.Date, 22);

				calendarItemNote.id = note.Id;
				calendarItemNote.Tag = "";
				calendarItemNote.Color = calendarItemNote.TagColor = GetBgColor(Calendar);
				calendarItemNote.Calendar = Calendar;
				calendarItemNote.DeleteOrder += OnDeleteNote;
				calendarItemNote.OpenOrder += OnOpenNote;
				calendarItemNote.isNoOrder = true;
				if(note.Message.Length > 400)
					calendarItemNote.Text = note.Message.Substring(0, 400);
				else calendarItemNote.Text = note.Message;
				int day = (note.Date - Calendar.StartDate).Days;
				Calendar.AddItem(day, 22, calendarItemNote);
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
					Employee emp = new Employee(rdr["last_name"].ToString(), rdr["first_name"].ToString(), rdr["patronymic"].ToString());
					if(text.Length > 0)
						text += ", ";
					text += emp.PersonNameWithInitials();
	
				}
				for(int i = 1; i < text.Length; i++)
					if((i % 30) == 0)
						text = text.Insert(++i, "\n");
				return text;
			}
		}
		catch(Exception ex)
		{
			QSMain.ErrorMessageWithLog("Ошибка получения списка сотрудников!", logger, ex);
			return text;
		}

	}
	private string GetBgColor(OrdersCalendar Calendar)
	{
		if(Calendar.CalendarNumber == 1)
			return "#f0f8ff";
		else 
			return "#fffaf0";
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

	protected void OnOpenNote(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		NoteDlg frmNote = new NoteDlg(item.id);
		frmNote.Show();
		if((ResponseType)frmNote.Run() == ResponseType.Ok)
			item.Calendar.RefreshOrders();
		frmNote.Destroy();
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

	protected void OnDeleteNote(object sender, EventArgs arg)
	{
		CalendarItem item = (CalendarItem)sender;
		Delete winDelete = new Delete();
		if(winDelete.RunDeletion("note", item.id))
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

	protected void OnNewNote(object sender, NewNoteEventArgs arg)
	{
		NoteDlg frmNote = new NoteDlg(arg.PointNumber, arg.CalendarNumber, arg.Date);
		frmNote.Show();
		int result = frmNote.Run();
		if(result == (int)ResponseType.Ok)
			((OrdersCalendar)sender).RefreshOrders();
		frmNote.Destroy();
	}
	protected void OnChangeTimeOrderEvent(object sender, CalendarItem.TimeChangedEventArgs arg)
	{
		CalendarItem order = (CalendarItem)sender;

		logger.Info("Изменение времени заказа на {0:d} {1}ч...", arg.Date, arg.Hour);
		string sql = "UPDATE orders SET date = @date, hour = @hour WHERE id = @id;" +
			" DELETE FROM employee_service_work WHERE id_order_pay in (SELECT id FROM order_pays WHERE order_id = @id)";
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