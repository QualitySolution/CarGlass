using System;
namespace QSChat
{
	public partial class NewMessage : Gtk.Window
	{
		public static uint CloseAfterSeconds = 30;
		private static uint? timerId;

		public static Action OpenChat;

		public NewMessage() :
			base(Gtk.WindowType.Popup)
		{
			this.Build();
		}

		static NewMessage OnScreen;

		static public void ShowMessage(string sender, string senderColor, string text)
		{
			if (OnScreen == null)
			{
				OnScreen = new NewMessage();
				OnScreen.eventbox1.ButtonPressEvent += Eventbox1_ButtonPressEvent;
			}

			OnScreen.labelMessageFrom.Markup = String.Format("<span foreground=\"{1}\" weight=\"bold\">Сообщение от {0}</span>", sender, senderColor);
			OnScreen.labelMessageText.LabelProp = text;

			OnScreen.ShowNow();

			CleanTimer();
			timerId = GLib.Timeout.Add(CloseAfterSeconds * 1000, new GLib.TimeoutHandler(CloseByTimeout));
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);

			var y = Screen.Height - allocation.Height - 60;
			var x = Screen.Width - allocation.Width - 20;

			Move(x, y);
		}

		private static void CleanTimer()
		{
			if (timerId.HasValue)
			{
				GLib.Source.Remove(timerId.Value);
				timerId = null;
			}
		}

		static void CloseMessage()
		{
			timerId = null;
			OnScreen.Hide();
		}

		static bool CloseByTimeout()
		{
			CloseMessage();
			return false;
		}

		protected void OnButtonCloseClicked(object sender, EventArgs e)
		{
			CleanTimer();
			CloseMessage();
		}

		static void Eventbox1_ButtonPressEvent(object o, Gtk.ButtonPressEventArgs args)
		{
			CleanTimer();
			CloseMessage();
			OpenChat?.Invoke();
		}

		public static void HideIfActive()
		{
			if(OnScreen != null)
			{
				CleanTimer();
				CloseMessage();
			}
		}
	}
}
