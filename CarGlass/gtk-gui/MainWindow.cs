
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.Action Action;
	private global::Gtk.Action Action8;
	private global::Gtk.Action Action2;
	private global::Gtk.Action dialogAuthenticationAction;
	private global::Gtk.Action UsersAction;
	private global::Gtk.Action quitAction;
	private global::Gtk.Action aboutAction;
	private global::Gtk.Action Action3;
	private global::Gtk.Action Action4;
	private global::Gtk.Action Action5;
	private global::Gtk.Action Action6;
	private global::Gtk.Action Action7;
	private global::Gtk.Action Action11;
	private global::Gtk.Action Action10;
	private global::Gtk.Action Action12;
	private global::Gtk.Action Action15;
	private global::Gtk.Action Action14;
	private global::Gtk.VBox vbox1;
	private global::Gtk.MenuBar menuMain;
	private global::Gtk.Notebook notebookMain;
	private global::CarGlass.OrdersCalendar orderscalendar1;
	private global::Gtk.Label label1;
	private global::CarGlass.OrdersCalendar orderscalendar2;
	private global::Gtk.Label label2;
	private global::Gtk.VBox vbox2;
	private global::Gtk.HBox hbox5;
	private global::Gtk.Table table1;
	private global::Gtk.Label label4;
	private global::Gtk.Label label5;
	private global::Gtk.Label label6;
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	private global::Gtk.TreeView treeviewOrders;
	private global::Gtk.Label labelTotal;
	private global::Gtk.Label label3;
	private global::Gtk.Statusbar statusbar1;
	private global::Gtk.Label labelUser;
	private global::Gtk.Label labelStatus;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.Action = new global::Gtk.Action ("Action", global::Mono.Unix.Catalog.GetString ("База"), null, null);
		this.Action.ShortLabel = global::Mono.Unix.Catalog.GetString ("База");
		w1.Add (this.Action, null);
		this.Action8 = new global::Gtk.Action ("Action8", global::Mono.Unix.Catalog.GetString ("Справочники"), null, null);
		this.Action8.ShortLabel = global::Mono.Unix.Catalog.GetString ("Справочники");
		w1.Add (this.Action8, null);
		this.Action2 = new global::Gtk.Action ("Action2", global::Mono.Unix.Catalog.GetString ("Справка"), null, null);
		this.Action2.ShortLabel = global::Mono.Unix.Catalog.GetString ("Справка");
		w1.Add (this.Action2, null);
		this.dialogAuthenticationAction = new global::Gtk.Action ("dialogAuthenticationAction", global::Mono.Unix.Catalog.GetString ("Изменить пароль"), null, "gtk-dialog-authentication");
		this.dialogAuthenticationAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Изменить пароль");
		w1.Add (this.dialogAuthenticationAction, null);
		this.UsersAction = new global::Gtk.Action ("UsersAction", global::Mono.Unix.Catalog.GetString ("Пользователи"), null, "gtk-properties");
		this.UsersAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Пользователи");
		w1.Add (this.UsersAction, null);
		this.quitAction = new global::Gtk.Action ("quitAction", global::Mono.Unix.Catalog.GetString ("В_ыход"), null, "gtk-quit");
		this.quitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("В_ыход");
		w1.Add (this.quitAction, null);
		this.aboutAction = new global::Gtk.Action ("aboutAction", global::Mono.Unix.Catalog.GetString ("_О программе"), null, "gtk-about");
		this.aboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_О программе");
		w1.Add (this.aboutAction, null);
		this.Action3 = new global::Gtk.Action ("Action3", global::Mono.Unix.Catalog.GetString ("Марки авто"), null, null);
		this.Action3.ShortLabel = global::Mono.Unix.Catalog.GetString ("Марки авто");
		w1.Add (this.Action3, null);
		this.Action4 = new global::Gtk.Action ("Action4", global::Mono.Unix.Catalog.GetString ("Модели авто"), null, null);
		this.Action4.ShortLabel = global::Mono.Unix.Catalog.GetString ("Модели авто");
		w1.Add (this.Action4, null);
		this.Action5 = new global::Gtk.Action ("Action5", global::Mono.Unix.Catalog.GetString ("Производители"), null, null);
		this.Action5.ShortLabel = global::Mono.Unix.Catalog.GetString ("Производители");
		w1.Add (this.Action5, null);
		this.Action6 = new global::Gtk.Action ("Action6", global::Mono.Unix.Catalog.GetString ("Склады"), null, null);
		this.Action6.ShortLabel = global::Mono.Unix.Catalog.GetString ("Склады");
		w1.Add (this.Action6, null);
		this.Action7 = new global::Gtk.Action ("Action7", global::Mono.Unix.Catalog.GetString ("Статусы заказа"), null, null);
		this.Action7.ShortLabel = global::Mono.Unix.Catalog.GetString ("Статусы заказа");
		w1.Add (this.Action7, null);
		this.Action11 = new global::Gtk.Action ("Action11", global::Mono.Unix.Catalog.GetString ("Виды стёкл"), null, null);
		this.Action11.ShortLabel = global::Mono.Unix.Catalog.GetString ("Виды стёкл");
		w1.Add (this.Action11, null);
		this.Action10 = new global::Gtk.Action ("Action10", global::Mono.Unix.Catalog.GetString ("Виды работ"), null, null);
		this.Action10.ShortLabel = global::Mono.Unix.Catalog.GetString ("Виды работ");
		w1.Add (this.Action10, null);
		this.Action12 = new global::Gtk.Action ("Action12", global::Mono.Unix.Catalog.GetString ("Отчеты"), null, null);
		this.Action12.ShortLabel = global::Mono.Unix.Catalog.GetString ("Отчеты");
		w1.Add (this.Action12, null);
		this.Action15 = new global::Gtk.Action ("Action15", global::Mono.Unix.Catalog.GetString ("Отчет по заказам"), null, null);
		this.Action15.ShortLabel = global::Mono.Unix.Catalog.GetString ("Отчет по заказам");
		w1.Add (this.Action15, null);
		this.Action14 = new global::Gtk.Action ("Action14", global::Mono.Unix.Catalog.GetString ("Отчет за месяц"), null, null);
		this.Action14.ShortLabel = global::Mono.Unix.Catalog.GetString ("Отчет за месяц");
		w1.Add (this.Action14, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("QS: Мастерская автостекла");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menuMain'><menu name='Action' action='Action'><menuitem name='dialogAuthenticationAction' action='dialogAuthenticationAction'/><menuitem name='UsersAction' action='UsersAction'/><separator/><menuitem name='quitAction' action='quitAction'/></menu><menu name='Action8' action='Action8'><menuitem name='Action3' action='Action3'/><menuitem name='Action4' action='Action4'/><separator/><menuitem name='Action5' action='Action5'/><menuitem name='Action6' action='Action6'/><separator/><menuitem name='Action11' action='Action11'/><menuitem name='Action10' action='Action10'/><menuitem name='Action7' action='Action7'/></menu><menu name='Action12' action='Action12'><menuitem name='Action15' action='Action15'/><menuitem name='Action14' action='Action14'/></menu><menu name='Action2' action='Action2'><menuitem name='aboutAction' action='aboutAction'/></menu></menubar></ui>");
		this.menuMain = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menuMain")));
		this.menuMain.Name = "menuMain";
		this.vbox1.Add (this.menuMain);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menuMain]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.notebookMain = new global::Gtk.Notebook ();
		this.notebookMain.CanFocus = true;
		this.notebookMain.Name = "notebookMain";
		this.notebookMain.CurrentPage = 0;
		// Container child notebookMain.Gtk.Notebook+NotebookChild
		this.orderscalendar1 = new global::CarGlass.OrdersCalendar ();
		this.orderscalendar1.Events = ((global::Gdk.EventMask)(256));
		this.orderscalendar1.Name = "orderscalendar1";
		this.orderscalendar1.StartDate = new global::System.DateTime (0);
		this.notebookMain.Add (this.orderscalendar1);
		// Notebook tab
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Календарь Установка");
		this.notebookMain.SetTabLabel (this.orderscalendar1, this.label1);
		this.label1.ShowAll ();
		// Container child notebookMain.Gtk.Notebook+NotebookChild
		this.orderscalendar2 = new global::CarGlass.OrdersCalendar ();
		this.orderscalendar2.Events = ((global::Gdk.EventMask)(256));
		this.orderscalendar2.Name = "orderscalendar2";
		this.orderscalendar2.StartDate = new global::System.DateTime (0);
		this.notebookMain.Add (this.orderscalendar2);
		global::Gtk.Notebook.NotebookChild w4 = ((global::Gtk.Notebook.NotebookChild)(this.notebookMain [this.orderscalendar2]));
		w4.Position = 1;
		// Notebook tab
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Календарь Тонировка");
		this.notebookMain.SetTabLabel (this.orderscalendar2, this.label2);
		this.label2.ShowAll ();
		// Container child notebookMain.Gtk.Notebook+NotebookChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbox5 = new global::Gtk.HBox ();
		this.hbox5.Name = "hbox5";
		this.hbox5.Spacing = 6;
		// Container child hbox5.Gtk.Box+BoxChild
		this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(3)), false);
		this.table1.RowSpacing = ((uint)(6));
		this.table1.ColumnSpacing = ((uint)(6));
		// Container child table1.Gtk.Table+TableChild
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.Xalign = 1F;
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Статус:");
		this.table1.Add (this.label4);
		global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
		w5.XOptions = ((global::Gtk.AttachOptions)(4));
		w5.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label5 = new global::Gtk.Label ();
		this.label5.Name = "label5";
		this.label5.Xalign = 1F;
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Склад:");
		this.table1.Add (this.label5);
		global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
		w6.TopAttach = ((uint)(1));
		w6.BottomAttach = ((uint)(2));
		w6.XOptions = ((global::Gtk.AttachOptions)(4));
		w6.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("label4");
		this.table1.Add (this.label6);
		global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label6]));
		w7.TopAttach = ((uint)(2));
		w7.BottomAttach = ((uint)(3));
		w7.XOptions = ((global::Gtk.AttachOptions)(4));
		w7.YOptions = ((global::Gtk.AttachOptions)(4));
		this.hbox5.Add (this.table1);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.table1]));
		w8.Position = 0;
		this.vbox2.Add (this.hbox5);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox5]));
		w9.Position = 0;
		w9.Expand = false;
		w9.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.treeviewOrders = new global::Gtk.TreeView ();
		this.treeviewOrders.CanFocus = true;
		this.treeviewOrders.Name = "treeviewOrders";
		this.GtkScrolledWindow.Add (this.treeviewOrders);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w11.Position = 1;
		// Container child vbox2.Gtk.Box+BoxChild
		this.labelTotal = new global::Gtk.Label ();
		this.labelTotal.Name = "labelTotal";
		this.labelTotal.Xalign = 1F;
		this.labelTotal.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
		this.vbox2.Add (this.labelTotal);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.labelTotal]));
		w12.Position = 2;
		w12.Expand = false;
		w12.Fill = false;
		this.notebookMain.Add (this.vbox2);
		global::Gtk.Notebook.NotebookChild w13 = ((global::Gtk.Notebook.NotebookChild)(this.notebookMain [this.vbox2]));
		w13.Position = 2;
		// Notebook tab
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Журнал заказов");
		this.notebookMain.SetTabLabel (this.vbox2, this.label3);
		this.label3.ShowAll ();
		this.vbox1.Add (this.notebookMain);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.notebookMain]));
		w14.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		// Container child statusbar1.Gtk.Box+BoxChild
		this.labelUser = new global::Gtk.Label ();
		this.labelUser.Name = "labelUser";
		this.labelUser.LabelProp = global::Mono.Unix.Catalog.GetString ("label5");
		this.statusbar1.Add (this.labelUser);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.statusbar1 [this.labelUser]));
		w15.Position = 0;
		w15.Expand = false;
		w15.Fill = false;
		// Container child statusbar1.Gtk.Box+BoxChild
		this.labelStatus = new global::Gtk.Label ();
		this.labelStatus.Name = "labelStatus";
		this.labelStatus.LabelProp = global::Mono.Unix.Catalog.GetString ("label4");
		this.statusbar1.Add (this.labelStatus);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.statusbar1 [this.labelStatus]));
		w16.Position = 2;
		w16.Expand = false;
		w16.Fill = false;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w17.Position = 2;
		w17.Expand = false;
		w17.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 670;
		this.DefaultHeight = 300;
		this.vbox2.Hide ();
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.dialogAuthenticationAction.Activated += new global::System.EventHandler (this.OnDialogAuthenticationActionActivated);
		this.UsersAction.Activated += new global::System.EventHandler (this.OnPropertiesActionActivated);
		this.aboutAction.Activated += new global::System.EventHandler (this.OnAboutActionActivated);
		this.Action3.Activated += new global::System.EventHandler (this.OnAction3Activated);
		this.Action4.Activated += new global::System.EventHandler (this.OnAction4Activated);
		this.Action5.Activated += new global::System.EventHandler (this.OnAction5Activated);
		this.Action6.Activated += new global::System.EventHandler (this.OnAction6Activated);
		this.Action7.Activated += new global::System.EventHandler (this.OnAction7Activated);
		this.Action11.Activated += new global::System.EventHandler (this.OnAction9Activated);
		this.Action10.Activated += new global::System.EventHandler (this.OnAction10Activated);
		this.Action15.Activated += new global::System.EventHandler (this.OnAction15Activated);
		this.notebookMain.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnNotebookMainSwitchPage);
	}
}
