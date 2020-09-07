using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;
using CarGlass.Domain;
using QS.DomainModel.UoW;
using System.Linq;
using Settings = CarGlass.Domain.Settings;

namespace CarGlass.Dialogs
{
	public partial class TimerRefreshDlg : Gtk.Dialog
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		Settings isUpdate;
		Settings timer;

		public TimerRefreshDlg()
		{
			this.Build();
			Fill();
		}

		private void Fill()
		{
			this.Title = "Обновление календаря";
			var settings = UoW.Session.QueryOver<Settings>().List();
			isUpdate = settings.FirstOrDefault(x => x.Parametr == "updateCalendar");
			if(isUpdate != null)
				ycheckbutton.Active = bool.Parse(isUpdate.ValueSettting);
			timer = settings.FirstOrDefault(x => x.Parametr == "timerCalendar");
			if(timer != null)
				yspinbutton.Value = double.Parse(timer.ValueSettting);
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			checkSettingsExist();

			isUpdate.ValueSettting = ycheckbutton.Active.ToString();
			timer.ValueSettting = ((int)yspinbutton.Value).ToString();
			isUpdate.DateEdit = timer.DateEdit = DateTime.Now;

			UoW.Save(isUpdate);
			UoW.Save(timer);
			UoW.Commit();
			Respond(ResponseType.Ok);
		}

		private void checkSettingsExist()
		{
			if(isUpdate == null)
			{
				isUpdate = new Settings();
				isUpdate.Parametr = "updateCalendar";
			}
			if(timer == null)
			{
				timer = new Settings();
				timer.Parametr = "timerCalendar";
			}
		}

		protected void OnYspinbuttonChanged(object sender, EventArgs e)
		{
			if(yspinbutton.Value > 240)
				yspinbutton.Value = 240;
			if(yspinbutton.Value < 1)
				yspinbutton.Value = 1;
		}
	}
}
