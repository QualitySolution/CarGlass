using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;
using QSProjectsLib;

namespace CarGlass
{
	public partial class GlassOrders : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public GlassOrders()
		{
			this.Build();

			QSMain.CheckConnectionAlive();
			ComboWorks.ComboFillReference(comboStatus, "status", ComboWorks.ListMode.WithNo);

			string sql = "SELECT id, name FROM stocks";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			using (MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					checklistStock.AddCheckButton(rdr["id"].ToString(), rdr["name"].ToString());
				}
			}

		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string param = String.Format("status={0}&start={1}&end={2}&cost1={3}",
				               ComboWorks.GetActiveId(comboStatus),
							String.Format ("{0:u}", selectperiod1.DateBegin).Substring (0, 10),
							String.Format ("{0:u}", selectperiod1.DateEnd).Substring (0, 10),
				               1);
			param += "&stock=";
			foreach(KeyValuePair<string, Gtk.CheckButton> pair in checklistStock.CheckButtons)
			{
				if (pair.Value.Active)
					param += String.Format ("{0},", pair.Key);
			}
			logger.Debug(String.Format("Report parameters=|{0}|", param));

			ViewReportExt.Run("glassorders", param.TrimEnd (','));
		}

		private void TestCanSave()
		{
			bool statusOk = comboStatus.Active > 0;
			bool stockOk = checklistStock.SelectedCount > 0;
			bool DatesOk = !selectperiod1.IsAllTime;
			buttonOk.Sensitive = statusOk && stockOk && DatesOk;
		}

		protected void OnComboStatusChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnSelectperiod1DatesChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnChecklistStockCheckListClicked(object sender, QSWidgetLib.CheckListClickedEventArgs e)
		{
			TestCanSave();
		}
	}
}

