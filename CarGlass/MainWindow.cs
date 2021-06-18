using System;
using Autofac;
using CarGlass;
using CarGlass.Dialogs;
using CarGlass.ReportDialog;
using Gtk;
using QS.Navigation;
using QS.Project.Versioning;
using QS.Project.Views;
using QS.Updater;
using QSOrmProject;
using QSProjectsLib;

public partial class MainWindow : FakeTDITabGtkWindowBase
{
	private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
	private ILifetimeScope AutofacScope = MainClass.AppDIContainer.BeginLifetimeScope();
	public GtkWindowsNavigationManager NavigationManager;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		Maximize();

		//Передаем лебл
		QSMain.StatusBarLabel = labelStatus;
		this.Title = AutofacScope.Resolve<IApplicationInfo>().ProductTitle;
		QSMain.MakeNewStatusTargetForNlog();
		Reference.RunReferenceItemDlg += OnRunReferenceItemDialog;

		QSMain.CheckServer(this); // Проверяем настройки сервера

		NavigationManager = AutofacScope.Resolve<GtkWindowsNavigationManager>();

		var checker = new VersionCheckerService(MainClass.AppDIContainer);
		checker.RunUpdate();

		if(QSMain.User.Login == "root")
		{
			string Message = "Вы зашли в программу под администратором базы данных. У вас есть только возможность создавать других пользователей.";
			MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent,
				MessageType.Info,
				ButtonsType.Ok,
				Message);
			md.Run();
			md.Destroy();
			Users WinUser = new Users();
			WinUser.Show();
			WinUser.Run();
			WinUser.Destroy();
			return;
		}

		UsersAction.Sensitive = QSMain.User.Admin;
		salarycalculation1.Visible = QSMain.User.Admin;
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
		QSMain.User.ChangeUserPassword(this);
	}

	protected void OnPropertiesActionActivated(object sender, EventArgs e)
	{
		Users winUsers = new Users();
		winUsers.Show();
		winUsers.Run();
		winUsers.Destroy();
	}

	protected void OnRefreshActionActivated(object sender, EventArgs e)
	{
		TimerRefreshDlg frmtimerRefreshDlg = new TimerRefreshDlg();
		frmtimerRefreshDlg.Show();
		frmtimerRefreshDlg.Run();
		frmtimerRefreshDlg.Destroy();
	}

	protected void OnRunReferenceItemDialog(object sender, Reference.RunReferenceItemDlgEventArgs e)
	{
		ResponseType Result;
		switch (e.TableName)
		{
			case "models":
				CarModelDlg ItemModel = new CarModelDlg();
				if (e.NewItem)
					ItemModel.NewItem = true;
				else
					ItemModel.Fill(e.ItemId);
				ItemModel.Show();
				Result = (ResponseType)ItemModel.Run();
				ItemModel.Destroy();
				break;
			case "status":
				Status StatusEdit = new Status();
				if (e.NewItem)
					StatusEdit.NewItem = true;
				else
					StatusEdit.Fill(e.ItemId);
				StatusEdit.Show();
				Result = (ResponseType)StatusEdit.Run();
				StatusEdit.Destroy();
				break;
			case "services":
				ServiceDlg ServiceEdit = new ServiceDlg();
				if (e.NewItem)
					ServiceEdit.NewItem = true;
				else
					ServiceEdit.Fill(e.ItemId);
				ServiceEdit.Show();
				Result = (ResponseType)ServiceEdit.Run();
				ServiceEdit.Destroy();
				break;
			case "stocks":
				OrderStock StocksEdit = new OrderStock();
				if (e.NewItem)
					StocksEdit.NewItem = true;
				else
					StocksEdit.Fill(e.ItemId);
				StocksEdit.Show();
				Result = (ResponseType)StocksEdit.Run();
				StocksEdit.Destroy();
				break;
			case "order_type":
				OrderTypeDlg OrderTypeEdit = e.NewItem ? new OrderTypeDlg() : new OrderTypeDlg(e.ItemId);
				OrderTypeEdit.Show();
				Result = (ResponseType)OrderTypeEdit.Run();
				OrderTypeEdit.Destroy();
				break;
			default:
				Result = ResponseType.None;
				break;
		}
		e.Result = Result;
	}

	protected void OnAboutActionActivated(object sender, EventArgs e)
	{
		using(var local = AutofacScope.BeginLifetimeScope())
		{
			var about = local.Resolve<AboutView>();
			about.Run();
			about.Destroy();
		}
	}

	protected void OnAction3Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true, false, true, true, true);
		winref.FillList("marks", "Марка автомобиля", "Марки автомобилей");
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
		winref.FillList("models", "Модель автомобиля", "Модели автомобилей");
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

	protected void OnNotebookMainSwitchPage(object o, SwitchPageArgs args)
	{
		switch (notebookMain.CurrentPage)
		{
			case 0:
				CurrentCalendar = frmClientCalendar.OrdersCalendar = orderscalendar1;
				orderscalendar1.RefreshOrders();
				break;
			case 1:
				CurrentCalendar = frmClientCalendar.OrdersCalendar = orderscalendar2;
				orderscalendar2.RefreshOrders();
				break;
			case 2:
				CurrentCalendar = frmClientCalendar.OrdersCalendar = orderscalendar3;
				orderscalendar3.RefreshOrders();
				break;
			case 3:
				CurrentCalendar = frmClientCalendar.OrdersCalendar = orderscalendar4;
				orderscalendar4.RefreshOrders();
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

	protected void OnActionHistoryLogActivated(object sender, EventArgs e)
	{
		QSMain.RunChangeLogDlg(this);
	}

	protected void OnActionCheckUpdatesActivated(object sender, EventArgs e)
	{
		using(var scope = MainClass.AppDIContainer.BeginLifetimeScope())
		{
			var updater = scope.Resolve<ApplicationUpdater>();
			updater.StartCheckUpdate(UpdaterFlags.ShowAnyway, scope);
		}
	}

	protected void OnActionStoreReportActivated(object sender, EventArgs e)
	{
		ViewReportExt.Run("Store", String.Empty);
	}

	protected void OnActionStorePlacmentActivated(object sender, EventArgs e)
	{
		var report = new StoreByPlacementsRDlg();
		report.Show();
		report.Run();
		report.Destroy();
	}

	protected void OnQuitActionActivated(object sender, EventArgs e)
	{
		Application.Quit();
	}

	protected void OnExcelActionActivated(object sender, EventArgs e)
	{
		var exportDlg = new ExportExcelDlg();
		exportDlg.Show();
		exportDlg.Run();
		exportDlg.Destroy();
	}

	protected void OnAction17Activated(object sender, EventArgs e)
	{
		EmployeesCatalogDlg employeesCatalogDlg = new EmployeesCatalogDlg();
		employeesCatalogDlg.Show();
		employeesCatalogDlg.Run();

	}

    protected void OnAction18Activated(object sender, EventArgs e)
    {
		Reference winref = new Reference();
		winref.SetMode(false, false, true, true, true);
		winref.FillList("order_type", "Заказы", "Виды заказов");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
}
