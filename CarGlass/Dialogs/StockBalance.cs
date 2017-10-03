using System;
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

		public StockBalance()
        {
            this.Build();

            entryCarBrand.SubjectType = typeof(CarBrand);
			entryCarModel.RepresentationModel = new CarModelsVM();

			comboGlass.SetRenderTextFunc<CarWindow>(x => x?.Name ?? "Не выбрано");
			var carWindows = UoW.GetAll<CarWindow>().ToList();
			comboGlass.ItemsList = carWindows;
			glassselector.GlassChanged += Glassselector_GlassChanged;
        }

		protected void OnEntryCarBrandChanged(object sender, EventArgs e)
		{
			(entryCarModel.RepresentationModel as CarModelsVM).OnlyBrand = (CarBrand)entryCarBrand.Subject;
			if (entryCarBrand.Subject != null && entryCarModel.Subject != null
			   && ((CarBrand)entryCarBrand.Subject).Id != entryCarModel.GetSubject<CarModel>().Brand.Id)
				entryCarModel.Subject = null;
		}

		void Glassselector_GlassChanged(object sender, EventArgs e)
		{
			int glassId = (int?)glassselector.SelectedGlass ?? 0;
			if (comboGlass.Active != glassId - 1)
				comboGlass.Active = glassId - 1;
		}

		protected void OnComboGlassChanged(object sender, EventArgs e)
		{
			glassselector.SelectedGlass = comboGlass.Active == -1 ? null : (Widgets.GlassSelector.GlassType?)comboGlass.Active + 1;
		}
	}
}
