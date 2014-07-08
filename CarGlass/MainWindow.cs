using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using QSSupportLib;
using CarGlass;

public partial class MainWindow: Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		Maximize();

		//Передаем лебл
		MainClass.StatusBarLabel = labelStatus;
		Reference.RunReferenceItemDlg += OnRunReferenceItemDialog;
		QSMain.ReferenceUpdated += OnReferenceUpdate;

		//Test version of base
		try
		{
			MainSupport.BaseParameters = new BaseParam(QSMain.connectionDB);
		}
		catch(MySqlException e)
		{
			Console.WriteLine(e.Message);
			MessageDialog BaseError = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				MessageType.Warning, 
				ButtonsType.Close, 
				"Не удалось получить информацию о версии базы данных.");
			BaseError.Run();
			BaseError.Destroy();
			Environment.Exit(0);
		}

		MainSupport.ProjectVerion = new AppVersion(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString(),
			"gpl",
			System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
		MainSupport.TestVersion(this); //Проверяем версию базы
		QSMain.CheckServer (this); // Проверяем настройки сервера

		if(QSMain.User.Login == "root")
		{
			string Message = "Вы зашли в программу под администратором базы данных. У вас есть только возможность создавать других пользователей.";
			MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				MessageType.Info, 
				ButtonsType.Ok,
				Message);
			md.Run ();
			md.Destroy();
			Users WinUser = new Users();
			WinUser.Show();
			WinUser.Run ();
			WinUser.Destroy ();
			return;
		}

		if(QSMain.connectionDB.DataSource == "demo.qsolution.ru")
		{
			string Message = "Вы подключились к демонстрационному серверу. Сервер предназначен для оценки " +
				"возможностей программы, не используйте его для работы, так как ваши данные будут доступны " +
				"любому пользователю через интернет.\n\nДля полноценного использования программы вам необходимо " +
				"установить собственный сервер. Для его установки обратитесь к документации.\n\nЕсли у вас возникнут " +
				"вопросы вы можете обратится в нашу тех. поддержку.";
			MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				MessageType.Info, 
				ButtonsType.Ok,
				Message);
			md.Run ();
			md.Destroy();
			dialogAuthenticationAction.Sensitive = false;
		}

		//Загружаем информацию о пользователе
		if(QSMain.User.TestUserExistByLogin (true))
			QSMain.User.UpdateUserInfoByLogin ();
		UsersAction.Sensitive = QSMain.User.admin;
		labelUser.LabelProp = QSMain.User.Name;

		//Настраиваем календарь
		PrerareCalendars();

		notebookMain.CurrentPage = 0;
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}
	protected void OnDialogAuthenticationActionActivated(object sender, EventArgs e)
	{
		QSMain.User.ChangeUserPassword (this);
	}
		
	protected void OnPropertiesActionActivated(object sender, EventArgs e)
	{
		Users winUsers = new Users();
		winUsers.Show();
		winUsers.Run();
		winUsers.Destroy();
	}

	protected void OnReferenceUpdate(object sender, QSMain.ReferenceUpdatedEventArgs e)
	{
		/*	switch (e.ReferenceTable) {
		case "doc_types":
			ComboWorks.ComboFillReference (comboDocType, "doc_types", 0);
		break;
		} */
	}

	protected void OnRunReferenceItemDialog(object sender, Reference.RunReferenceItemDlgEventArgs e)
	{
		ResponseType Result;
		switch (e.TableName)
		{
			case "models":
				CarModel ItemModel = new CarModel();
				if(e.NewItem)
					ItemModel.NewItem = true;
				else 
					ItemModel.Fill(e.ItemId);
				ItemModel.Show();
				Result = (ResponseType)ItemModel.Run();
				ItemModel.Destroy();
				break;
			case "status":
				Status StatusEdit = new Status();
				if(e.NewItem)
					StatusEdit.NewItem = true;
				else 
					StatusEdit.Fill(e.ItemId);
				StatusEdit.Show();
				Result = (ResponseType)StatusEdit.Run();
				StatusEdit.Destroy();
				break;
			case "services":
				Service ServiceEdit = new Service();
				if(e.NewItem)
					ServiceEdit.NewItem = true;
				else 
					ServiceEdit.Fill(e.ItemId);
				ServiceEdit.Show();
				Result = (ResponseType)ServiceEdit.Run();
				ServiceEdit.Destroy();
				break;
			case "stocks":
				OrderStock StocksEdit = new OrderStock();
				if(e.NewItem)
					StocksEdit.NewItem = true;
				else 
					StocksEdit.Fill(e.ItemId);
				StocksEdit.Show();
				Result = (ResponseType)StocksEdit.Run();
				StocksEdit.Destroy();
				break;

			default:
				Result = ResponseType.None;
				break;
		}
		e.Result = Result;
	}

	protected void OnAboutActionActivated(object sender, EventArgs e)
	{
		AboutDialog dialog = new AboutDialog ();
		dialog.ProgramName = "QS: Мастерская автостекла";

		Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		dialog.Version = String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

		dialog.Logo = Gdk.Pixbuf.LoadFromResource ("CarGlass.icons.logo.png");

		dialog.Comments = "Регистрация заказов в мастерской автостекл." +
			"\nРазработана на MonoDevelop с использованием открытых технологий Mono, GTK#, MySQL." +
			"\nТелефон тех. поддержки +7(812)575-79-44";

		dialog.Copyright = "Quality Solution 2014";

		dialog.Authors = new string [] {"Ганьков Андрей <gav@qsolution.ru>"};

		dialog.Website = "http://www.qsolution.ru/";

		dialog.Run ();
		dialog.Destroy();
	}

	protected void OnAction3Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true, false, true, true, true);
		winref.FillList("marks","Марка автомобиля", "Марки автомобилей");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction4Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false, false, true, true, true);
		winref.SqlSelect = "SELECT models.id, models.name, marks.name as mark FROM @tablename " +
			"LEFT JOIN marks ON marks.id = models.mark_id ORDER BY mark, name";
		winref.Columns.Insert(1, new Reference.ColumnInfo("Марка", "{2}", true));
		winref.Columns[2].Name = "Модель";
		winref.FillList("models","Модель автомобиля", "Модели автомобилей");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction5Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true, false, true, true, true);
		winref.FillList("manufacturers", "Производитель", "Производители");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction6Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false, false, true, true, true);
		winref.FillList("stocks", "Склад", "Склады");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction7Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false, false, true, true, true);
		winref.FillList("status", "Статус заказа", "Статусы заказа");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction9Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true, false, true, true, true);
		winref.FillList("glass", "Стекло", "Виды стёкл");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
		
	protected void OnAction10Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false, false, true, true, true);
		winref.OrdinalField = "ordinal";
		winref.SqlSelect += " ORDER BY ordinal";
		winref.FillList("services", "Вид работы", "Виды работ");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnButton38Clicked(object sender, EventArgs e)
	{
		Order NewOrder = new Order(Order.OrderType.repair, DateTime.Now.Date, 9);
		NewOrder.NewItem = true;
		NewOrder.Show();
		NewOrder.Run();
		NewOrder.Destroy();
	}

	protected void OnNotebookMainSwitchPage(object o, SwitchPageArgs args)
	{
		switch (notebookMain.CurrentPage)
		{
			case 0:
				orderscalendar1.RefreshOrders();
				break;
			case 1:
				orderscalendar2.RefreshOrders();
				break;
		}
	}

	protected void OnAction15Activated(object sender, EventArgs e)
	{
		GlassOrders report = new GlassOrders();
		report.Show();
		report.Run();
		report.Destroy();
	}

	protected void OnAction16Activated(object sender, EventArgs e)
	{
		PrintDay report = new PrintDay();
		report.Show();
		report.Run();
		report.Destroy();
	}
		
	protected void OnAction14Activated(object sender, EventArgs e)
	{
		MonthReport report = new MonthReport();
		report.Show();
		report.Run();
		report.Destroy();
	}
}
