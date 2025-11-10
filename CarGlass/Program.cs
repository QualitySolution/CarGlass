using System;
using System.Collections.Generic;
using Autofac;
using Gtk;
using NLog;
using QS.Configuration;
using QS.Dialog;
using QS.ErrorReporting;
using QSProjectsLib;

namespace CarGlass
{
	partial class MainClass
	{
		public static MainWindow MainWin;
		private static Logger logger = LogManager.GetCurrentClassLogger();

		[STAThread]
		public static void Main(string[] args)
		{
			UnhandledExceptionHandler unhandledExceptionHandler = new UnhandledExceptionHandler();
			try
			{
				WindowStartupFix.WindowsCheck();
				Application.Init();
				QSMain.GuiThread = System.Threading.Thread.CurrentThread;
				GtkGuiDispatcher.GuiThread = System.Threading.Thread.CurrentThread;
				
				var builder = new ContainerBuilder();
				AutofacStartupConfig(builder);
				StartupContainer = builder.Build();
				unhandledExceptionHandler.UpdateDependencies(StartupContainer);
				unhandledExceptionHandler.SubscribeToUnhandledExceptions();

			} catch(MissingMethodException ex) when (ex.Message.Contains("System.String System.String.Format")) {
				WindowStartupFix.DisplayWindowsOkMessage("Версия .Net Framework должна быть не ниже 4.6.1. Установите более новую платформу.", "Старая версия .Net");
				return;
			}
			catch (Exception fallEx) {
				if (WindowStartupFix.IsWindows)
					WindowStartupFix.DisplayWindowsOkMessage(fallEx.ToString(), "Критическая ошибка");
				else
					Console.WriteLine(fallEx);

				logger.Fatal(fallEx);
				return;
			}

			CreateProjectParam();

			ILifetimeScope scopeLoginTime = StartupContainer.BeginLifetimeScope();
			var configuration = scopeLoginTime.Resolve<IChangeableConfiguration>();
			// Создаем окно входа
			Login LoginDialog = new Login (configuration);
			LoginDialog.Logo = Gdk.Pixbuf.LoadFromResource ("CarGlass.icons.logo.png");
			Login.MakeDefaultConnections = () => new Connection[] {
				new Connection(
					ConnectionType.MySQL,
					"Рабочая",
					"CarGlass",
					"stekloff.qsolution.ru"
				),
			};
			LoginDialog.UpdateFromGConf ();

			ResponseType LoginResult;
			LoginResult = (ResponseType) LoginDialog.Run();
			if (LoginResult == ResponseType.DeleteEvent || LoginResult == ResponseType.Cancel)
				return;

			LoginDialog.Destroy ();

			//Настройка базы
			CreateBaseConfig();

			// Для корректного удаления в справочниках
			QSOrmProject.OrmMain.DisableLegacyDeletion();

			//Запускаем программу
			MainWin = new MainWindow ();
			QSMain.ErrorDlgParrent = MainWin;
			if(QSMain.User.Login == "root")
				return;
			MainWin.Show ();
			Application.Run ();
		}

		static void CreateProjectParam()
		{
			QSMain.ProjectPermission = new Dictionary<string, UserPermission>();
			QSMain.ProjectPermission.Add("manager", new UserPermission("manager", "Режим менеджера",
				"Пользователь может добавлять и редактировать заказы."
			));
			QSMain.ProjectPermission.Add("worker", new UserPermission("worker", "Режим рабочего",
			"Пользователь может просматривать заказы."
			));

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
			PrepareTable.DeleteItems.Add("employee_service_work",
				new TableInfo.DeleteDependenceItem("WHERE id_order_pay = @id ", "", "@id"));
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

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "График работ";
			PrepareTable.ObjectName = "график работы";
			PrepareTable.SqlSelect = "SELECT id, date_work, point_number, calendar_number FROM shedule_works ";
			PrepareTable.DisplayString = "График работ {1:d} числа";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			PrepareTable.DeleteItems.Add("shedule_employee_works",
			new TableInfo.DeleteDependenceItem("WHERE id_shedule_works = @id ", "", "@id"));
			Tables.Add("shedule_works", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "График работ сотрудников";
			PrepareTable.ObjectName = "график работ сотрудников";
			PrepareTable.SqlSelect = "SELECT id, id_shedule_works, id_employee FROM shedule_employee_works ";
			PrepareTable.DisplayString = "График работ сотрудников {1}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			Tables.Add("shedule_employee_works", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Список выполняемых услуг";
			PrepareTable.ObjectName = "список выполняемых услуг";
			PrepareTable.SqlSelect = "SELECT id, id_employee, id_employee, id_order_pay, date_work FROM employee_service_work";
			PrepareTable.DisplayString = "Список выполненных услуг";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			Tables.Add("employee_service_work", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Список типов заказов";
			PrepareTable.ObjectName = "список типов заказов";
			PrepareTable.SqlSelect = "SELECT id, name, is_calculate_salary, position_in_tabs," +
				"is_show_main_widgets, is_show_additional_widgets, is_install_type FROM order_type";
			PrepareTable.DisplayString = "Список типов заказов";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			Tables.Add("order_type", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Заметки";
			PrepareTable.ObjectName = "список заметок";
			PrepareTable.SqlSelect = "SELECT id, date, point_number, calendar_number, message";
			PrepareTable.DisplayString = "Список заметок";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys("id");
			Tables.Add("note", PrepareTable);
		}

    }
}
