
// This file has been generated by the GUI designer. Do not modify.
namespace CarGlass.Dialogs
{
	public partial class OrderTypeDlg
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label1;

		private global::Gtk.Entry entryNumberOrder;

		private global::Gtk.Table table1;

		private global::Gamma.GtkWidgets.yEntry entryNameAccusative;

		private global::Gtk.Entry entryNameOrder;

		private global::Gtk.Label label3;

		private global::Gamma.GtkWidgets.yLabel ylabel1;

		private global::Gtk.CheckButton checkbuttonCalculationSalary;

		private global::Gtk.Frame frame1;

		private global::Gtk.Alignment GtkAlignment2;

		private global::Gtk.VBox vbox4;

		private global::Gtk.HBox hbox7;

		private global::Gtk.CheckButton checkbuttonInstallationSuburban;

		private global::Gtk.CheckButton checkbuttonOrder;

		private global::Gtk.HBox hbox9;

		private global::Gtk.CheckButton checkbuttonSuburbanTinting;

		private global::Gtk.CheckButton checkbuttonTintedEntry;

		private global::Gtk.Label GtkLabel5;

		private global::Gtk.Frame frame3;

		private global::Gtk.Alignment GtkAlignment3;

		private global::Gtk.HBox hbox11;

		private global::Gtk.VBox vbox5;

		private global::Gtk.CheckButton checkbuttonMainParameters;

		private global::Gtk.CheckButton checkbuttonAdditionalParameters;

		private global::Gtk.Label GtkLabel8;

		private global::Gtk.Frame frame4;

		private global::Gtk.Alignment GtkAlignment4;

		private global::Gtk.HBox hbox12;

		private global::Gtk.VBox vbox6;

		private global::Gtk.CheckButton checkbuttonInstall;

		private global::Gtk.CheckButton checkbuttonOther;

		private global::Gtk.Label GtkLabel15;

		private global::Gtk.Label label5;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTreeView ytreeviewService;

		private global::Gtk.HBox hbox5;

		private global::Gtk.Button button247;

		private global::Gtk.Button button249;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CarGlass.Dialogs.OrderTypeDlg
			this.Name = "CarGlass.Dialogs.OrderTypeDlg";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CarGlass.Dialogs.OrderTypeDlg.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = "Номер типа заказа:";
			this.hbox1.Add(this.label1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.entryNumberOrder = new global::Gtk.Entry();
			this.entryNumberOrder.Sensitive = false;
			this.entryNumberOrder.CanFocus = true;
			this.entryNumberOrder.Name = "entryNumberOrder";
			this.entryNumberOrder.IsEditable = true;
			this.entryNumberOrder.InvisibleChar = '●';
			this.hbox1.Add(this.entryNumberOrder);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.entryNumberOrder]));
			w3.Position = 1;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(2)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.entryNameAccusative = new global::Gamma.GtkWidgets.yEntry();
			this.entryNameAccusative.CanFocus = true;
			this.entryNameAccusative.Name = "entryNameAccusative";
			this.entryNameAccusative.IsEditable = true;
			this.entryNameAccusative.InvisibleChar = '●';
			this.table1.Add(this.entryNameAccusative);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.entryNameAccusative]));
			w5.TopAttach = ((uint)(1));
			w5.BottomAttach = ((uint)(2));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryNameOrder = new global::Gtk.Entry();
			this.entryNameOrder.CanFocus = true;
			this.entryNameOrder.Name = "entryNameOrder";
			this.entryNameOrder.IsEditable = true;
			this.entryNameOrder.InvisibleChar = '●';
			this.table1.Add(this.entryNameOrder);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.entryNameOrder]));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Наименование типа заказа:");
			this.table1.Add(this.label3);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.label3]));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabel1 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel1.Name = "ylabel1";
			this.ylabel1.Xalign = 1F;
			this.ylabel1.LabelProp = global::Mono.Unix.Catalog.GetString("Винительный падеж (Что?):");
			this.table1.Add(this.ylabel1);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabel1]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.table1]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.checkbuttonCalculationSalary = new global::Gtk.CheckButton();
			this.checkbuttonCalculationSalary.CanFocus = true;
			this.checkbuttonCalculationSalary.Name = "checkbuttonCalculationSalary";
			this.checkbuttonCalculationSalary.Label = global::Mono.Unix.Catalog.GetString("Рассчитывать заработную плату");
			this.checkbuttonCalculationSalary.DrawIndicator = true;
			this.checkbuttonCalculationSalary.UseUnderline = true;
			this.vbox2.Add(this.checkbuttonCalculationSalary);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.checkbuttonCalculationSalary]));
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame();
			this.frame1.Name = "frame1";
			this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.vbox4 = new global::Gtk.VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hbox7 = new global::Gtk.HBox();
			this.hbox7.Name = "hbox7";
			this.hbox7.Spacing = 6;
			// Container child hbox7.Gtk.Box+BoxChild
			this.checkbuttonInstallationSuburban = new global::Gtk.CheckButton();
			this.checkbuttonInstallationSuburban.CanFocus = true;
			this.checkbuttonInstallationSuburban.Name = "checkbuttonInstallationSuburban";
			this.checkbuttonInstallationSuburban.Label = global::Mono.Unix.Catalog.GetString("Установка пригородный");
			this.checkbuttonInstallationSuburban.Active = true;
			this.checkbuttonInstallationSuburban.DrawIndicator = true;
			this.checkbuttonInstallationSuburban.UseUnderline = true;
			this.hbox7.Add(this.checkbuttonInstallationSuburban);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.checkbuttonInstallationSuburban]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child hbox7.Gtk.Box+BoxChild
			this.checkbuttonOrder = new global::Gtk.CheckButton();
			this.checkbuttonOrder.CanFocus = true;
			this.checkbuttonOrder.Name = "checkbuttonOrder";
			this.checkbuttonOrder.Label = global::Mono.Unix.Catalog.GetString("Заказные");
			this.checkbuttonOrder.Active = true;
			this.checkbuttonOrder.DrawIndicator = true;
			this.checkbuttonOrder.UseUnderline = true;
			this.hbox7.Add(this.checkbuttonOrder);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.checkbuttonOrder]));
			w12.Position = 1;
			this.vbox4.Add(this.hbox7);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.hbox7]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hbox9 = new global::Gtk.HBox();
			this.hbox9.Name = "hbox9";
			this.hbox9.Spacing = 6;
			// Container child hbox9.Gtk.Box+BoxChild
			this.checkbuttonSuburbanTinting = new global::Gtk.CheckButton();
			this.checkbuttonSuburbanTinting.CanFocus = true;
			this.checkbuttonSuburbanTinting.Name = "checkbuttonSuburbanTinting";
			this.checkbuttonSuburbanTinting.Label = global::Mono.Unix.Catalog.GetString("Тонировка пригородный");
			this.checkbuttonSuburbanTinting.Active = true;
			this.checkbuttonSuburbanTinting.DrawIndicator = true;
			this.checkbuttonSuburbanTinting.UseUnderline = true;
			this.hbox9.Add(this.checkbuttonSuburbanTinting);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.checkbuttonSuburbanTinting]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox9.Gtk.Box+BoxChild
			this.checkbuttonTintedEntry = new global::Gtk.CheckButton();
			this.checkbuttonTintedEntry.CanFocus = true;
			this.checkbuttonTintedEntry.Name = "checkbuttonTintedEntry";
			this.checkbuttonTintedEntry.Label = global::Mono.Unix.Catalog.GetString("Тонировка въезд");
			this.checkbuttonTintedEntry.Active = true;
			this.checkbuttonTintedEntry.DrawIndicator = true;
			this.checkbuttonTintedEntry.UseUnderline = true;
			this.hbox9.Add(this.checkbuttonTintedEntry);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.checkbuttonTintedEntry]));
			w15.Position = 1;
			this.vbox4.Add(this.hbox9);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.hbox9]));
			w16.Position = 1;
			w16.Expand = false;
			w16.Fill = false;
			this.GtkAlignment2.Add(this.vbox4);
			this.frame1.Add(this.GtkAlignment2);
			this.GtkLabel5 = new global::Gtk.Label();
			this.GtkLabel5.Name = "GtkLabel5";
			this.GtkLabel5.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Отображать на вкладках:</b>");
			this.GtkLabel5.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel5;
			this.vbox2.Add(this.frame1);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.frame1]));
			w19.Position = 3;
			w19.Expand = false;
			w19.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.frame3 = new global::Gtk.Frame();
			this.frame3.Name = "frame3";
			this.frame3.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame3.Gtk.Container+ContainerChild
			this.GtkAlignment3 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment3.Name = "GtkAlignment3";
			this.GtkAlignment3.LeftPadding = ((uint)(12));
			// Container child GtkAlignment3.Gtk.Container+ContainerChild
			this.hbox11 = new global::Gtk.HBox();
			this.hbox11.Name = "hbox11";
			this.hbox11.Spacing = 6;
			// Container child hbox11.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.checkbuttonMainParameters = new global::Gtk.CheckButton();
			this.checkbuttonMainParameters.CanFocus = true;
			this.checkbuttonMainParameters.Name = "checkbuttonMainParameters";
			this.checkbuttonMainParameters.Label = global::Mono.Unix.Catalog.GetString("Общие параметры(марка, модель, год, телефон)");
			this.checkbuttonMainParameters.Active = true;
			this.checkbuttonMainParameters.DrawIndicator = true;
			this.checkbuttonMainParameters.UseUnderline = true;
			this.vbox5.Add(this.checkbuttonMainParameters);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.checkbuttonMainParameters]));
			w20.Position = 0;
			w20.Expand = false;
			w20.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.checkbuttonAdditionalParameters = new global::Gtk.CheckButton();
			this.checkbuttonAdditionalParameters.CanFocus = true;
			this.checkbuttonAdditionalParameters.Name = "checkbuttonAdditionalParameters";
			this.checkbuttonAdditionalParameters.Label = global::Mono.Unix.Catalog.GetString("Дополнительные(производитель, склад, еврокод)");
			this.checkbuttonAdditionalParameters.DrawIndicator = true;
			this.checkbuttonAdditionalParameters.UseUnderline = true;
			this.vbox5.Add(this.checkbuttonAdditionalParameters);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.checkbuttonAdditionalParameters]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			this.hbox11.Add(this.vbox5);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox11[this.vbox5]));
			w22.Position = 0;
			this.GtkAlignment3.Add(this.hbox11);
			this.frame3.Add(this.GtkAlignment3);
			this.GtkLabel8 = new global::Gtk.Label();
			this.GtkLabel8.Name = "GtkLabel8";
			this.GtkLabel8.LabelProp = global::Mono.Unix.Catalog.GetString("<b>При открытии показывать:</b>");
			this.GtkLabel8.UseMarkup = true;
			this.frame3.LabelWidget = this.GtkLabel8;
			this.vbox2.Add(this.frame3);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.frame3]));
			w25.Position = 4;
			w25.Expand = false;
			w25.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.frame4 = new global::Gtk.Frame();
			this.frame4.Name = "frame4";
			this.frame4.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame4.Gtk.Container+ContainerChild
			this.GtkAlignment4 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment4.Name = "GtkAlignment4";
			this.GtkAlignment4.LeftPadding = ((uint)(12));
			// Container child GtkAlignment4.Gtk.Container+ContainerChild
			this.hbox12 = new global::Gtk.HBox();
			this.hbox12.Name = "hbox12";
			this.hbox12.Spacing = 6;
			// Container child hbox12.Gtk.Box+BoxChild
			this.vbox6 = new global::Gtk.VBox();
			this.vbox6.Name = "vbox6";
			this.vbox6.Spacing = 6;
			// Container child vbox6.Gtk.Box+BoxChild
			this.checkbuttonInstall = new global::Gtk.CheckButton();
			this.checkbuttonInstall.CanFocus = true;
			this.checkbuttonInstall.Name = "checkbuttonInstall";
			this.checkbuttonInstall.Label = global::Mono.Unix.Catalog.GetString("Относится к установке");
			this.checkbuttonInstall.DrawIndicator = true;
			this.checkbuttonInstall.UseUnderline = true;
			this.vbox6.Add(this.checkbuttonInstall);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox6[this.checkbuttonInstall]));
			w26.Position = 0;
			w26.Expand = false;
			w26.Fill = false;
			// Container child vbox6.Gtk.Box+BoxChild
			this.checkbuttonOther = new global::Gtk.CheckButton();
			this.checkbuttonOther.CanFocus = true;
			this.checkbuttonOther.Name = "checkbuttonOther";
			this.checkbuttonOther.Label = global::Mono.Unix.Catalog.GetString("Относится к общему (прочему) типу заказа");
			this.checkbuttonOther.DrawIndicator = true;
			this.checkbuttonOther.UseUnderline = true;
			this.vbox6.Add(this.checkbuttonOther);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox6[this.checkbuttonOther]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			this.hbox12.Add(this.vbox6);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.vbox6]));
			w28.Position = 0;
			this.GtkAlignment4.Add(this.hbox12);
			this.frame4.Add(this.GtkAlignment4);
			this.GtkLabel15 = new global::Gtk.Label();
			this.GtkLabel15.Name = "GtkLabel15";
			this.GtkLabel15.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Дополнительные параметры типа заказа:</b>");
			this.GtkLabel15.UseMarkup = true;
			this.frame4.LabelWidget = this.GtkLabel15;
			this.vbox2.Add(this.frame4);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.frame4]));
			w31.Position = 5;
			w31.Expand = false;
			w31.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 0F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Услуги:");
			this.vbox2.Add(this.label5);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.label5]));
			w32.Position = 6;
			w32.Expand = false;
			w32.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.ytreeviewService = new global::Gamma.GtkWidgets.yTreeView();
			this.ytreeviewService.CanFocus = true;
			this.ytreeviewService.Name = "ytreeviewService";
			this.GtkScrolledWindow.Add(this.ytreeviewService);
			this.vbox2.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
			w34.Position = 7;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.button247 = new global::Gtk.Button();
			this.button247.CanFocus = true;
			this.button247.Name = "button247";
			this.button247.UseUnderline = true;
			this.button247.Label = global::Mono.Unix.Catalog.GetString("Добавить услугу");
			this.hbox5.Add(this.button247);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.button247]));
			w35.Position = 0;
			w35.Expand = false;
			w35.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.button249 = new global::Gtk.Button();
			this.button249.CanFocus = true;
			this.button249.Name = "button249";
			this.button249.UseUnderline = true;
			this.button249.Label = global::Mono.Unix.Catalog.GetString("Убрать услугу");
			this.hbox5.Add(this.button249);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.button249]));
			w36.Position = 1;
			w36.Expand = false;
			w36.Fill = false;
			this.vbox2.Add(this.hbox5);
			global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox5]));
			w37.Position = 8;
			w37.Expand = false;
			w37.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w38.Position = 0;
			// Internal child CarGlass.Dialogs.OrderTypeDlg.ActionArea
			global::Gtk.HButtonBox w39 = this.ActionArea;
			w39.Name = "dialog1_ActionArea";
			w39.Spacing = 10;
			w39.BorderWidth = ((uint)(5));
			w39.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w40 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w39[this.buttonCancel]));
			w40.Expand = false;
			w40.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w41 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w39[this.buttonOk]));
			w41.Position = 1;
			w41.Expand = false;
			w41.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 506;
			this.DefaultHeight = 625;
			this.Show();
			this.entryNameOrder.Activated += new global::System.EventHandler(this.OnEntryNameOrderActivated);
			this.button247.Clicked += new global::System.EventHandler(this.OnButtonAddServiceClicked);
			this.button249.Clicked += new global::System.EventHandler(this.OnButtonDeleteServiceClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
