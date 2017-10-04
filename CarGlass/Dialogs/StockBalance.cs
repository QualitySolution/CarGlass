using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using CarGlass.Representation;
using QSOrmProject;

namespace CarGlass.Dialogs
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class StockBalance : Gtk.Bin
    {
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		IList<StoreItem> StoreItems;
		GenericObservableList<StoreItem> ObservableStoreItems;

		enum StoreColumns
		{
			Glass
		}

		public StockBalance()
        {
            this.Build();

            entryCarBrand.SubjectType = typeof(CarBrand);
			entryCarModel.RepresentationModel = new CarModelsVM();

			comboGlass.SetRenderTextFunc<CarWindow>(x => x?.Name ?? "Не выбрано");
			var carWindows = UoW.GetAll<CarWindow>().ToList();
			comboGlass.ItemsList = carWindows;
			glassselector.GlassChanged += Glassselector_GlassChanged;

			var manufactures = UoW.GetAll<GlassManufacturer>().OrderBy(x => x.Name).ToList();

			ytreeviewStore.CreateFluentColumnsConfig<StoreItem>()
						  .AddColumn("Еврокод").AddTextRenderer(x => x.EuroCode).Editable()
						  .AddColumn("Стекло").Tag(StoreColumns.Glass).AddTextRenderer(x => x.CarWindow != null ? x.CarWindow.Name : String.Empty)
						  .AddColumn("Производитель").AddComboRenderer(x => x.Manufacturer).Editing()
						  .SetDisplayFunc(x => x.Name)
						  .FillItems(manufactures)
			              .AddColumn("Цена").AddNumericRenderer(x => x.Cost).Editing(new Gtk.Adjustment(0, 0, 1000000, 100, 1000, 0)).WidthChars(7)
			              .AddColumn("Наличие").AddNumericRenderer(x => x.Amount).Editing(new Gtk.Adjustment(0, 0, 1000, 1, 10, 0))
						  .AddColumn("Место").AddTextRenderer(x => x.Placement).Editable()
						  .AddColumn("Комментарий").AddTextRenderer(x => x.Comment).Editable()
			              .Finish();

			ytreeviewStore.Selection.Changed += TreeviewStoreSelection_Changed;
        }

		protected void OnEntryCarBrandChanged(object sender, EventArgs e)
		{
			(entryCarModel.RepresentationModel as CarModelsVM).OnlyBrand = (CarBrand)entryCarBrand.Subject;
			if (entryCarBrand.Subject != null && entryCarModel.Subject != null
			   && ((CarBrand)entryCarBrand.Subject).Id != entryCarModel.GetSubject<CarModel>().Brand.Id)
				entryCarModel.Subject = null;

			RefreshEditingItems();
		}

		void Glassselector_GlassChanged(object sender, EventArgs e)
		{
			int glassId = (int?)glassselector.SelectedGlass ?? 0;
			if (comboGlass.Active != glassId - 1)
				comboGlass.Active = glassId - 1;

			ytreeviewStore.ColumnsConfig.GetColumnsByTag(StoreColumns.Glass).First().Visible = glassselector.SelectedGlass == null;
		}

		protected void OnComboGlassChanged(object sender, EventArgs e)
		{
			glassselector.SelectedGlass = comboGlass.Active == -1 ? null : (Widgets.GlassSelector.GlassType?)comboGlass.Active + 1;
			RefreshEditingItems();
		}

		protected void OnButtonAddRowClicked(object sender, EventArgs e)
		{
			var row = new StoreItem();
			row.CarModel = entryCarModel.GetSubject<CarModel>();
			row.CarBrand = row.CarModel.Brand;
			row.CarWindow = comboGlass.SelectedItem as CarWindow;
			ObservableStoreItems.Add(row);
		}

		protected void OnButtonDeleteRowClicked(object sender, EventArgs e)
		{
			var selected = ytreeviewStore.GetSelectedObject<StoreItem>();
			ObservableStoreItems.Remove(selected);
			UoW.Delete(selected);
		}

		void RefreshEditingItems()
		{
			//Очищаем предыдущий список, не очищая остальные объекты сессии.
			if (StoreItems != null)
				StoreItems.ToList().ForEach(x => UoW.Session.Evict(x));

			if (comboGlass.Active == -1 && entryCarBrand.Subject == null && entryCarModel.Subject == null)
			{
				StoreItems.Clear();
				return;
			}

			var query = UoW.Session.QueryOver<StoreItem>();
			if (glassselector.SelectedGlass.HasValue)
				query.Where(x => x.CarWindow.Id == (int)glassselector.SelectedGlass.Value);
			if (entryCarBrand.Subject != null && entryCarModel.Subject == null)
				query.Where(x => x.CarBrand.Id == entryCarBrand.SubjectId);
			if (entryCarModel.Subject != null)
				query.Where(x => x.CarModel.Id == entryCarModel.SubjectId);

			StoreItems = query.List();
			ObservableStoreItems = new GenericObservableList<StoreItem>(StoreItems);
			ytreeviewStore.SetItemsSource(ObservableStoreItems);
			UpdateCanAddState();
		}

		protected void OnEntryCarModelChangedByUser(object sender, EventArgs e)
		{
			RefreshEditingItems();
		}

		void TreeviewStoreSelection_Changed(object sender, EventArgs e)
		{
			buttonDeleteRow.Sensitive = ytreeviewStore.Selection.CountSelectedRows() > 0;
		}

		void UpdateCanAddState()
		{
			buttonAddRow.Sensitive = (glassselector.SelectedGlass.HasValue && entryCarModel.Subject != null);
		}
	}
}
