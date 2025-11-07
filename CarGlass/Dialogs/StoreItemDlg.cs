using System;
using System.Linq;
using CarGlass.Domain;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using QS.Validation;
using QSOrmProject;

namespace CarGlass.Dialogs
{
	[WidgetWindow(HideButtons = true)]
	public partial class StoreItemDlg : EntityDialogBase<StoreItem>
	{
		static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public StoreItemDlg(CarWindow glass) : this ()
		{
			if(glass != null)
				Entity.CarWindow = UoW.GetById<CarWindow>(glass.Id);
		}

		public StoreItemDlg()
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<StoreItem>();
			ConfigureDlg();
		}

		public StoreItemDlg(int id)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<StoreItem>(id);
			ConfigureDlg();
		}

		public StoreItemDlg(StoreItem sub) : this (sub.Id)
		{
		}

		void ConfigureDlg()
		{
			comboGlass.SetRenderTextFunc<CarWindow>(x => x.Name);
			comboGlass.ItemsList = UoW.GetAll<CarWindow>();
			comboGlass.Binding.AddBinding(Entity, e => e.CarWindow, w => w.SelectedItem).InitializeFromSource();

			comboModel.SetRenderTextFunc<CarModel>(x => x.Name);
			entryBrand.SubjectType = typeof(CarBrand);
			entryBrand.Binding.AddBinding(Entity, e => e.CarBrand, w => w.Subject).InitializeFromSource();

			comboModel.Binding.AddBinding(Entity, e => e.CarModel, w => w.SelectedItem).InitializeFromSource();

			comboManufacturer.SetRenderTextFunc<Manufacturer>(x => x.Name);
			comboManufacturer.ItemsList = UoW.GetAll<Manufacturer>();
			comboManufacturer.Binding.AddBinding(Entity, e => e.Manufacturer, w => w.SelectedItem).InitializeFromSource();

			yentryEurocode.Binding.AddBinding(Entity, e => e.EuroCode, w => w.Text).InitializeFromSource();
			yspinCost.Binding.AddBinding(Entity, e => e.Cost, w => w.ValueAsDecimal).InitializeFromSource();
			yspinAmount.Binding.AddBinding(Entity, e => e.Amount, w => w.ValueAsInt).InitializeFromSource();
			yentryPlacment.Binding.AddBinding(Entity, e => e.Placement, w => w.Text).InitializeFromSource();
			ytextComment.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();
		}

		public override bool Save()
		{
			var valid = new ObjectValidator(new GtkValidationViewFactory());
			if (valid.Validate(Entity))
				return false;

			logger.Info("Сохраняем складскую позицию...");
			UoWGeneric.Save();
			logger.Info("Ok.");
			return true;
		}

		protected void OnEntryBrandChanged(object sender, EventArgs e)
		{
			if (Entity.CarBrand != null)
				comboModel.ItemsList = UoW.GetAll<CarModel>().Where(x => x.Brand.Id == Entity.CarBrand.Id).ToList();
			else
				comboModel.ItemsList = null;
		}
	}
}
