
// This file has been generated by the GUI designer. Do not modify.
namespace CarGlass.Dialogs
{
	public partial class EmployeesCatalogDlg
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label10;

		private global::Gamma.GtkWidgets.yCheckButton ycheckbutton1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gamma.GtkWidgets.yTreeView ytree;

		private global::Gtk.HBox hbox2;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Button btnAdd;

		private global::Gtk.Button btnAccept;

		private global::Gtk.Button btnDismiss;

		private global::Gtk.Button btnDelete;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CarGlass.Dialogs.EmployeesCatalogDlg
			this.Name = "CarGlass.Dialogs.EmployeesCatalogDlg";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CarGlass.Dialogs.EmployeesCatalogDlg.VBox
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
			this.label10 = new global::Gtk.Label();
			this.label10.Name = "label10";
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString("Сотрудники:");
			this.hbox1.Add(this.label10);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label10]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.ycheckbutton1 = new global::Gamma.GtkWidgets.yCheckButton();
			this.ycheckbutton1.CanFocus = true;
			this.ycheckbutton1.Name = "ycheckbutton1";
			this.ycheckbutton1.Label = global::Mono.Unix.Catalog.GetString("работающие");
			this.ycheckbutton1.Active = true;
			this.ycheckbutton1.DrawIndicator = true;
			this.ycheckbutton1.UseUnderline = true;
			this.hbox1.Add(this.ycheckbutton1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.ycheckbutton1]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.ytree = new global::Gamma.GtkWidgets.yTreeView();
			this.ytree.CanFocus = true;
			this.ytree.Name = "ytree";
			this.GtkScrolledWindow1.Add(this.ytree);
			this.vbox2.Add(this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow1]));
			w6.Position = 1;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.btnAdd = new global::Gtk.Button();
			this.btnAdd.CanFocus = true;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseUnderline = true;
			this.btnAdd.Label = global::Mono.Unix.Catalog.GetString("Добавить");
			this.hbox3.Add(this.btnAdd);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.btnAdd]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.btnAccept = new global::Gtk.Button();
			this.btnAccept.Sensitive = false;
			this.btnAccept.CanFocus = true;
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.UseUnderline = true;
			this.btnAccept.Label = global::Mono.Unix.Catalog.GetString("Принять");
			this.hbox3.Add(this.btnAccept);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.btnAccept]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.btnDismiss = new global::Gtk.Button();
			this.btnDismiss.CanFocus = true;
			this.btnDismiss.Name = "btnDismiss";
			this.btnDismiss.UseUnderline = true;
			this.btnDismiss.Label = global::Mono.Unix.Catalog.GetString("Уволить");
			this.hbox3.Add(this.btnDismiss);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.btnDismiss]));
			w9.Position = 2;
			w9.Expand = false;
			w9.Fill = false;
			this.hbox2.Add(this.hbox3);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.hbox3]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnDelete = new global::Gtk.Button();
			this.btnDelete.CanFocus = true;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.UseUnderline = true;
			this.btnDelete.Label = global::Mono.Unix.Catalog.GetString("Удалить пустые строки");
			this.hbox2.Add(this.btnDelete);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.btnDelete]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.vbox2.Add(this.hbox2);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
			w12.Position = 2;
			w12.Expand = false;
			w12.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w13.Position = 0;
			// Internal child CarGlass.Dialogs.EmployeesCatalogDlg.ActionArea
			global::Gtk.HButtonBox w14 = this.ActionArea;
			w14.Name = "dialog1_ActionArea";
			w14.Spacing = 10;
			w14.BorderWidth = ((uint)(5));
			w14.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w15 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w14[this.buttonCancel]));
			w15.Expand = false;
			w15.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w16 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w14[this.buttonOk]));
			w16.Position = 1;
			w16.Expand = false;
			w16.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 427;
			this.DefaultHeight = 300;
			this.Show();
			this.ycheckbutton1.Clicked += new global::System.EventHandler(this.OnYcheckbutton1Clicked);
			this.btnAdd.Clicked += new global::System.EventHandler(this.OnBtnAddClicked);
			this.btnAccept.Clicked += new global::System.EventHandler(this.OnBtnAcceptClicked);
			this.btnDismiss.Clicked += new global::System.EventHandler(this.OnBtnDismissClicked);
			this.btnDelete.Clicked += new global::System.EventHandler(this.OnBtnDeleteClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler(this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
