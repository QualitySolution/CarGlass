using System;
using CarGlass.Domain;
using NHibernate.Criterion;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace CarGlass.ReportDialog
{
	public partial class StoreByPlacementsRDlg : Gtk.Dialog
	{
		public StoreByPlacementsRDlg()
		{
			this.Build();
			var uow = UnitOfWorkFactory.CreateWithoutRoot();
			var places = uow.Session.QueryOver<StoreItem>()
			                .Select(Projections.Distinct(Projections.Property<StoreItem>(x => x.Placement)))
			                .OrderBy(x => x.Placement).Asc
			                .List<string>();
			comboPlacement.ItemsList = places;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string parameters = String.Empty;
			if (!comboPlacement.IsSelectedAll)
				parameters = $"place={comboPlacement.SelectedItem}";
			ViewReportExt.Run("StoreByPlacements", parameters);
		}
	}
}
