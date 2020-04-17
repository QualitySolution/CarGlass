using System;

namespace CarGlass.Dialogs
{
	public partial class SalarySettingsDlg : Gtk.Dialog
	{
		public SalarySettingsDlg()
		{
			this.Build();
			this.Title = "Настройки";
		}

		protected void OnButton3053Clicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}
