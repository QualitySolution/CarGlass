
// This file has been generated by the GUI designer. Do not modify.
namespace CarGlass.Views.SMS
{
	public partial class SendMessageView
	{
		private global::Gamma.GtkWidgets.yVBox yvbox1;

		private global::Gamma.GtkWidgets.yLabel ylabel1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTextView textMessage;

		private global::Gamma.GtkWidgets.yHBox yhbox1;

		private global::Gamma.GtkWidgets.yButton buttonSend;

		private global::Gamma.GtkWidgets.yButton buttonCancel;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CarGlass.Views.SMS.SendMessageView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "CarGlass.Views.SMS.SendMessageView";
			// Container child CarGlass.Views.SMS.SendMessageView.Gtk.Container+ContainerChild
			this.yvbox1 = new global::Gamma.GtkWidgets.yVBox();
			this.yvbox1.Name = "yvbox1";
			this.yvbox1.Spacing = 6;
			// Container child yvbox1.Gtk.Box+BoxChild
			this.ylabel1 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel1.Name = "ylabel1";
			this.ylabel1.Xalign = 0F;
			this.ylabel1.LabelProp = global::Mono.Unix.Catalog.GetString("Текст сообщения:");
			this.yvbox1.Add(this.ylabel1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.yvbox1[this.ylabel1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child yvbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.WidthRequest = 350;
			this.GtkScrolledWindow.HeightRequest = 80;
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.textMessage = new global::Gamma.GtkWidgets.yTextView();
			this.textMessage.CanFocus = true;
			this.textMessage.Name = "textMessage";
			this.textMessage.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow.Add(this.textMessage);
			this.yvbox1.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.yvbox1[this.GtkScrolledWindow]));
			w3.Position = 1;
			// Container child yvbox1.Gtk.Box+BoxChild
			this.yhbox1 = new global::Gamma.GtkWidgets.yHBox();
			this.yhbox1.Name = "yhbox1";
			this.yhbox1.Spacing = 6;
			// Container child yhbox1.Gtk.Box+BoxChild
			this.buttonSend = new global::Gamma.GtkWidgets.yButton();
			this.buttonSend.CanFocus = true;
			this.buttonSend.Name = "buttonSend";
			this.buttonSend.UseUnderline = true;
			this.buttonSend.Label = global::Mono.Unix.Catalog.GetString("Отправить");
			global::Gtk.Image w4 = new global::Gtk.Image();
			w4.Pixbuf = global::Gdk.Pixbuf.LoadFromResource("CarGlass.icons.buttons.send.png");
			this.buttonSend.Image = w4;
			this.yhbox1.Add(this.buttonSend);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.yhbox1[this.buttonSend]));
			w5.PackType = ((global::Gtk.PackType)(1));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child yhbox1.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gamma.GtkWidgets.yButton();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.yhbox1.Add(this.buttonCancel);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.yhbox1[this.buttonCancel]));
			w6.PackType = ((global::Gtk.PackType)(1));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			this.yvbox1.Add(this.yhbox1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.yvbox1[this.yhbox1]));
			w7.Position = 2;
			w7.Expand = false;
			w7.Fill = false;
			this.Add(this.yvbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.buttonCancel.Clicked += new global::System.EventHandler(this.OnButtonCancelClicked);
			this.buttonSend.Clicked += new global::System.EventHandler(this.OnButtonSendClicked);
		}
	}
}