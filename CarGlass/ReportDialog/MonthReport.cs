using System;
using QSProjectsLib;

namespace CarGlass
{
	public partial class MonthReport : Gtk.Dialog
	{
		public MonthReport()
		{
			this.Build();
		}

		private void TestCanSave()
		{
			bool DatesOk = !selectperiod1.IsAllTime;
			buttonOk.Sensitive = DatesOk;
		}

		protected void OnSelectperiod1DatesChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string param = String.Format("start={1}&end={2}",
				String.Format ("{0:u}", selectperiod1.DateBegin).Substring (0, 10),
				String.Format ("{0:u}", selectperiod1.DateEnd).Substring (0, 10)
			);

			ViewReportExt.Run("MonthReport", param);
		}
	}
}

