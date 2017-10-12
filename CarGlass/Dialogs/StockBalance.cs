using System;
using CarGlass.Domain;
using CarGlass.Representation;
using QSOrmProject;

namespace CarGlass.Dialogs
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class StockBalance : Gtk.Bin
    {
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		StoreItemsVM representation;

		public StockBalance()
        {
            this.Build();

			representation = new StoreItemsVM(UoW);

			representationViewStoreItems.RepresentationModel = representation;

			representationViewStoreItems.Selection.Changed += TreeviewStoreSelection_Changed;
        }


		protected void OnButtonAddRowClicked(object sender, EventArgs e)
		{
			//var row = new StoreItem();
			//row.CarModel = entryCarModel.GetSubject<CarModel>();
			//row.CarBrand = row.CarModel.Brand;
			//row.CarWindow = comboGlass.SelectedItem as CarWindow;
			//ObservableStoreItems.Add(row);
		}

		protected void OnButtonDeleteRowClicked(object sender, EventArgs e)
		{
			var selected = representationViewStoreItems.GetSelectedObject<StoreItemsVMNode>();
			var item = UoW.GetById<StoreItem>(selected.Id);
			UoW.Delete(item);
		}


		void TreeviewStoreSelection_Changed(object sender, EventArgs e)
		{
			buttonDeleteRow.Sensitive = representationViewStoreItems.Selection.CountSelectedRows() > 0;
		}

		protected void OnButtonSearchCleanClicked(object sender, EventArgs e)
		{
			yentrySearch.Text = String.Empty;
		}

		protected void OnGlassselectorGlassChanged(object sender, EventArgs e)
		{
			var glass = glassselector.SelectedGlass;
			representation.OnlyItemType = glass.HasValue ? UoW.GetById<CarWindow>((int)glass.Value) : null;
		}

		protected void OnYentrySearchChanged(object sender, EventArgs e)
		{
			representation.SearchString = yentrySearch.Text;
		}
	}
}
