using System;
using CarGlass.Domain;
using CarGlass.Representation;

namespace CarGlass.Dialogs
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class StockBalance : Gtk.Bin
    {
        public StockBalance()
        {
            this.Build();

            entryCarBrand.SubjectType = typeof(CarBrand);
			entryCarModel.RepresentationModel = new CarModelsVM();
        }

		protected void OnEntryCarBrandChanged(object sender, EventArgs e)
		{
			(entryCarModel.RepresentationModel as CarModelsVM).OnlyBrand = (CarBrand)entryCarBrand.Subject;
			if (entryCarBrand.Subject != null && entryCarModel.Subject != null
			   && ((CarBrand)entryCarBrand.Subject).Id != entryCarModel.GetSubject<CarModel>().Brand.Id)
				entryCarModel.Subject = null;
		}
	}
}
