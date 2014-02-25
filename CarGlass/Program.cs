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
			PrepareTable.ObjectsName = "Места";
			PrepareTable.ObjectName = "место";
			PrepareTable.SqlSelect = "SELECT place_types.name as type, place_no, area , type_id FROM places " +
			"LEFT JOIN place_types ON places.type_id = place_types.id ";
			PrepareTable.DisplayString = "Место {0}-{1} с площадью {2} кв.м.";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys("type_id", "place_no");
			PrepareTable.DeleteItems.Add("contracts", 
				new TableInfo.DeleteDependenceItem("WHERE contracts.place_type_id = @type_id AND contracts.place_no = @place_no", "@place_no", "@type_id"));
			PrepareTable.DeleteItems.Add("meters", 
				new TableInfo.DeleteDependenceItem("WHERE meters.place_type_id = @type_id AND meters.place_no = @place_no ", "@place_no", "@type_id"));
			PrepareTable.DeleteItems.Add("events", 
				new TableInfo.DeleteDependenceItem("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NULL", "@place_no", "@type_id"));
			PrepareTable.ClearItems.Add("events", 
				new TableInfo.ClearDependenceItem("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NOT NULL", "@place_no", "@type_id", "place_type_id", "place_no"));
			Tables.Add("places", PrepareTable);
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
