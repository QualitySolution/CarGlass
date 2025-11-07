using System;
using Gtk;
using QSProjectsLib;
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
			isUpdate = settings.FirstOrDefault(x => x.Parameter == "updateCalendar");
			if(isUpdate != null)
				ycheckbutton.Active = bool.Parse(isUpdate.ValueSetting);
			timer = settings.FirstOrDefault(x => x.Parameter == "timerCalendar");
			if(timer != null)
				yspinbutton.Value = double.Parse(timer.ValueSetting);
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			checkSettingsExist();

			isUpdate.ValueSetting = ycheckbutton.Active.ToString();
			timer.ValueSetting = ((int)yspinbutton.Value).ToString();
			isUpdate.DateEdit = timer.DateEdit = DateTime.Now;

			UoW.Save(isUpdate);
			UoW.Save(timer);
			UoW.Commit();
			MessageDialogWorks.RunInfoDialog("Изменения вступят в силу после перезапуска программы.");
			Respond(ResponseType.Ok);
		}

		private void checkSettingsExist()
		{
			if(isUpdate == null)
			{
				isUpdate = new Settings();
				isUpdate.Parameter = "updateCalendar";
			}
			if(timer == null)
			{
				timer = new Settings();
				timer.Parameter = "timerCalendar";
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
