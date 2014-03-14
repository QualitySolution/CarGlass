using System;
using Gtk;
using QSProjectsLib;
using System.Collections.Generic;
using NLog;

namespace CarGlass
{
	class MainClass
	{
		public static Label StatusBarLabel;
		public static MainWindow MainWin;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public static void Main(string[] args)
		{
			Application.Init();
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e) 
			{
				QSMain.ErrorMessage(MainWin, (Exception) e.ExceptionObject);
			};
			CreateProjectParam();
			//Настраиваем общую билиотеку
			QSMain.NewStatusText += delegate(object sender, QSProjectsLib.QSMain.NewStatusTextEventArgs e) 
			{
				StatusMessage (e.NewText);
			};
			// Создаем окно входа
			Login LoginDialog = new QSProjectsLib.Login ();
			LoginDialog.Logo = Gdk.Pixbuf.LoadFromResource ("CarGlass.icons.logo.png");
			LoginDialog.SetDefaultNames ("CarGlass");
			LoginDialog.DefaultLogin = "demo";
			LoginDialog.DefaultServer = "demo.qsolution.ru";
			LoginDialog.DemoServer = "demo.qsolution.ru";
			LoginDialog.DemoMessage = "Для подключения к демострационному серверу используйте следующие настройки:\n" +
				"\n" +
				"<b>Сервер:</b> demo.qsolution.ru\n" +
				"<b>Пользователь:</b> demo\n" +
				"<b>Пароль:</b> demo\n" +
				"\n" +
				"Для установки собственного сервера обратитесь к документации.";
			LoginDialog.UpdateFromGConf ();

			ResponseType LoginResult;
			LoginResult = (ResponseType) LoginDialog.Run();
			if (LoginResult == ResponseType.DeleteEvent || LoginResult == ResponseType.Cancel)
				return;

			LoginDialog.Destroy ();

			//Запускаем программу
			MainWin = new MainWindow ();
			if(QSMain.User.Login == "root")
				return;
			MainWin.Show ();
			Application.Run ();
		}

		static void CreateProjectParam()
		{
			QSMain.AdminFieldName = "admin";
			QSMain.ProjectPermission = new Dictionary<string, UserPermission>();
			//QSMain.ProjectPermission.Add("edit_slips", new UserPermission("edit_slips", "Изменение кассы задним числом",
			//"Пользователь может изменять или добавлять кассовые документы задним числом."));

			QSMain.User = new UserInfo();

			//Параметры удаления
			Dictionary<string, TableInfo> Tables = new Dictionary<string, TableInfo>();
			QSMain.ProjectTables = Tables;
			TableInfo PrepareTable;

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Пользователи";
			PrepareTable.ObjectName = "пользователя"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM users ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			Tables.Add ("users", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Услуги";
			PrepareTable.ObjectName = "услуга";
			PrepareTable.SqlSelect = "SELECT name , id FROM services ";
			PrepareTable.DisplayString = "Услуга {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("order_pays", 
				new TableInfo.DeleteDependenceItem ("WHERE service_id = @id ", "", "@id"));
			Tables.Add("services", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Оплаты заказа";
			PrepareTable.ObjectName = "оплата заказа";
			PrepareTable.SqlSelect = "SELECT order_id, id, cost FROM order_pays ";
			PrepareTable.DisplayString = "Оплата в заказе {0} на сумму{2:C}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			Tables.Add("order_pays", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Заказы";
			PrepareTable.ObjectName = "заказ";
			PrepareTable.SqlSelect = "SELECT id, date, hour FROM orders ";
			PrepareTable.DisplayString = "Заказ №{0} на {2} часа {1:d} числа";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			Tables.Add("orders", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Стекла в заказе";
			PrepareTable.ObjectName = "стекло в заказе";
			PrepareTable.SqlSelect = "SELECT order_id, id FROM order_glasses ";
			PrepareTable.DisplayString = "Стекло в заказе №{0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			Tables.Add("order_glasses", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Виды стекол";
			PrepareTable.ObjectName = "вид стекла";
			PrepareTable.SqlSelect = "SELECT name, id FROM glass ";
			PrepareTable.DisplayString = "Стекло {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("order_glasses", 
				new TableInfo.DeleteDependenceItem ("WHERE glass_id = @id ", "", "@id"));
			Tables.Add("glass", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Статусы";
			PrepareTable.ObjectName = "статус";
			PrepareTable.SqlSelect = "SELECT name, id FROM status ";
			PrepareTable.DisplayString = "Статус {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("orders", 
				new TableInfo.DeleteDependenceItem ("WHERE status_id = @id ", "", "@id"));
			Tables.Add("status", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Склады";
			PrepareTable.ObjectName = "склад";
			PrepareTable.SqlSelect = "SELECT name, id FROM stocks ";
			PrepareTable.DisplayString = "Склад {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.ClearItems.Add ("orders", 
				new TableInfo.ClearDependenceItem ("WHERE stock_id = @id", "", "@id", "stock_id"));
			Tables.Add("stocks", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Производители";
			PrepareTable.ObjectName = "производитель";
			PrepareTable.SqlSelect = "SELECT name, id FROM manufacturers ";
			PrepareTable.DisplayString = "Производитель {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.ClearItems.Add ("orders", 
				new TableInfo.ClearDependenceItem ("WHERE manufacturer_id = @id", "", "@id", "manufacturer_id"));
			Tables.Add("manufacturers", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Модели автомобилей";
			PrepareTable.ObjectName = "модель автомобиля";
			PrepareTable.SqlSelect = "SELECT marks.name, models.name, models.id FROM models " +
				"LEFT JOIN marks ON marks.id = models.mark_id ";
			PrepareTable.DisplayString = "Модель {0} {1}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("orders", 
				new TableInfo.DeleteDependenceItem ("WHERE car_model_id = @id ", "", "@id"));
			Tables.Add("models", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Марки автомобилей";
			PrepareTable.ObjectName = "марка автомобиля";
			PrepareTable.SqlSelect = "SELECT name, id FROM marks ";
			PrepareTable.DisplayString = "Марка {0}";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("models", 
				new TableInfo.DeleteDependenceItem ("WHERE mark_id = @id ", "", "@id"));
			Tables.Add("marks", PrepareTable);

		}

		public static void StatusMessage(string message)
		{
			StatusBarLabel.Text = message;
			logger.Info(message);
			while (GLib.MainContext.Pending())
			{
				Gtk.Main.Iteration();
			}
		}
	}
}
