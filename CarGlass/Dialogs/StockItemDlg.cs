using System;
using CarGlass.Domain;
using QSOrmProject;

namespace CarGlass.Dialogs
{
	public partial class StoreItemDlg : OrmGtkDialogBase<StoreItem>
	{
		public StoreItemDlg()
		{
			this.Build();
		}
	}
}
