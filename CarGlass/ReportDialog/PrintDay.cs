using System;
using QSProjectsLib;

namespace CarGlass
{
	public partial class PrintDay : Gtk.Dialog
	{
		public PrintDay()
		{
			this.Build();

			dateCalendar.Date = DateTime.Today;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string type;
			if (radioinstall.Active)
				type = "install";
			else
				type = "tinting,repair";
			string date = String.Format("{0:u}", dateCalendar.Date).Substring(0, 10);

			string param = String.Format("type={0}&date={1}", type, date);
			ViewReportExt.Run("PrintDay", param);
		}
	}
}

