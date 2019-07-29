using System;
using CarGlass.Domain;
using CarGlass.Representation;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;

namespace CarGlass.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class StockBalance : WidgetOnDialogBase
    {
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		StoreItemsVM representation;

		public StockBalance()
        {
            this.Build();

			representation = new StoreItemsVM(UoW);

			representationViewStoreItems.RepresentationModel = representation;
			representationViewStoreItems.ModifyFont(Pango.FontDescription.FromString("Purisa 12"));
			representationViewStoreItems.RulesHint = true;

			representationViewStoreItems.Selection.Changed += TreeviewStoreSelection_Changed;
        }


		protected void OnButtonAddRowClicked(object sender, EventArgs e)
		{
			var rowDlg = new StoreItemDlg(GetSelectedWindow());
			OpenNewTab(rowDlg);
		}

		protected void OnButtonDeleteRowClicked(object sender, EventArgs e)
		{
			var selected = representationViewStoreItems.GetSelectedObject<StoreItemsVMNode>();
			var item = UoW.GetById<StoreItem>(selected.Id);
			UoW.Delete(item);
			UoW.Commit();
			representation.UpdateNodes();
		}

		void TreeviewStoreSelection_Changed(object sender, EventArgs e)
		{
			buttonDeleteRow.Sensitive = buttonEdit.Sensitive = representationViewStoreItems.Selection.CountSelectedRows() > 0;
		}

		protected void OnButtonSearchCleanClicked(object sender, EventArgs e)
		{
			yentrySearch.Text = String.Empty;
		}

		protected void OnGlassselectorGlassChanged(object sender, EventArgs e)
		{
			representation.OnlyItemType = GetSelectedWindow();
		}

		private CarWindow GetSelectedWindow()
		{
			var glass = glassselector.SelectedGlass;
			return glass.HasValue ? UoW.GetById<CarWindow>((int)glass.Value) : null;
		}

		protected void OnYentrySearchChanged(object sender, EventArgs e)
		{
			representation.SearchString = yentrySearch.Text;
		}

		protected void OnButtonEditClicked(object sender, EventArgs e)
		{
			var selected = representationViewStoreItems.GetSelectedObject<StoreItemsVMNode>();
			var rowDlg = new StoreItemDlg(selected.Id);
			OpenNewTab(rowDlg);
		}

		protected void OnRepresentationViewStoreItemsRowActivated(object o, Gtk.RowActivatedArgs args)
		{
			buttonEdit.Click();
		}
	}
}
