
// This file has been generated by the GUI designer. Do not modify.
namespace CarGlass.Dialogs
{
	public partial class AddEditFormulas
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.Label label3;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label1;

		private global::Gamma.GtkWidgets.yEntry yentry;

		private global::Gtk.Label label4;

		private global::Gamma.GtkWidgets.yEntry yentryComment;

		private global::Gtk.Label label2;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTreeView ytreeCoeff;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Button btnSelectCoeff;

		private global::Gtk.Button btnAddCoeff;

		private global::Gtk.Button btnDeleteCoeff;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CarGlass.Dialogs.AddEditFormulas
			this.Name = "CarGlass.Dialogs.AddEditFormulas";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CarGlass.Dialogs.AddEditFormulas.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("label4");
			this.vbox2.Add(this.label3);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.label3]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Формула:");
			this.hbox1.Add(this.label1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label1]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.yentry = new global::Gamma.GtkWidgets.yEntry();
			this.yentry.CanFocus = true;
			this.yentry.Name = "yentry";
			this.yentry.IsEditable = true;
			this.yentry.MaxLength = 45;
			this.yentry.InvisibleChar = '●';
			this.hbox1.Add(this.yentry);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.yentry]));
			w4.Position = 1;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Комментарий:");
			this.hbox1.Add(this.label4);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label4]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.yentryComment = new global::Gamma.GtkWidgets.yEntry();
			this.yentryComment.CanFocus = true;
			this.yentryComment.Name = "yentryComment";
			this.yentryComment.IsEditable = true;
			this.yentryComment.MaxLength = 45;
			this.yentryComment.InvisibleChar = '●';
			this.hbox1.Add(this.yentryComment);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.yentryComment]));
			w6.Position = 3;
			w6.Expand = false;
			w6.Fill = false;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("label2");
			this.vbox2.Add(this.label2);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.label2]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.ytreeCoeff = new global::Gamma.GtkWidgets.yTreeView();
			this.ytreeCoeff.CanFocus = true;
			this.ytreeCoeff.Name = "ytreeCoeff";
			this.GtkScrolledWindow.Add(this.ytreeCoeff);
			this.vbox2.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
			w10.Position = 3;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnSelectCoeff = new global::Gtk.Button();
			this.btnSelectCoeff.CanFocus = true;
			this.btnSelectCoeff.Name = "btnSelectCoeff";
			this.btnSelectCoeff.UseUnderline = true;
			this.btnSelectCoeff.Label = global::Mono.Unix.Catalog.GetString("Выбрать");
			this.hbox2.Add(this.btnSelectCoeff);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.btnSelectCoeff]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnAddCoeff = new global::Gtk.Button();
			this.btnAddCoeff.CanFocus = true;
			this.btnAddCoeff.Name = "btnAddCoeff";
			this.btnAddCoeff.UseUnderline = true;
			this.btnAddCoeff.Label = global::Mono.Unix.Catalog.GetString("Добавить");
			this.hbox2.Add(this.btnAddCoeff);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.btnAddCoeff]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnDeleteCoeff = new global::Gtk.Button();
			this.btnDeleteCoeff.CanFocus = true;
			this.btnDeleteCoeff.Name = "btnDeleteCoeff";
			this.btnDeleteCoeff.UseUnderline = true;
			this.btnDeleteCoeff.Label = global::Mono.Unix.Catalog.GetString("Удалить");
			this.hbox2.Add(this.btnDeleteCoeff);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.btnDeleteCoeff]));
			w13.Position = 2;
			w13.Expand = false;
			w13.Fill = false;
			this.vbox2.Add(this.hbox2);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
			w14.Position = 4;
			w14.Expand = false;
			w14.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w15.Position = 0;
			// Internal child CarGlass.Dialogs.AddEditFormulas.ActionArea
			global::Gtk.HButtonBox w16 = this.ActionArea;
			w16.Name = "dialog1_ActionArea";
			w16.Spacing = 10;
			w16.BorderWidth = ((uint)(5));
			w16.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16[this.buttonCancel]));
			w17.Expand = false;
			w17.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w18 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16[this.buttonOk]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 526;
			this.DefaultHeight = 300;
			this.Show();
			this.btnSelectCoeff.Clicked += new global::System.EventHandler(this.OnBtnSelectCoeffClicked);
			this.btnAddCoeff.Clicked += new global::System.EventHandler(this.OnBtnAddCoeffClicked);
			this.btnDeleteCoeff.Clicked += new global::System.EventHandler(this.OnBtnDeleteCoeffClicked);
			this.buttonCancel.Clicked += new global::System.EventHandler(this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}