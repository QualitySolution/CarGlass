using System;

namespace CarGlass.Dialogs
{
	public partial class SalarySettingsDlg : Gtk.Dialog
	{
		public SalarySettingsDlg()
		{
			this.Build();
		}

		protected void OnButton3053Clicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}
