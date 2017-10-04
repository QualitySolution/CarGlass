using System;
using QSOrmProject;

namespace CarGlass.Domain
{
	public class StoreItem : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private CarBrand carBrand;

		public virtual CarBrand CarBrand
		{
			get { return carBrand; }
			set { SetField(ref carBrand, value, () => CarBrand); }
		}

		private CarModel carModel;

		public virtual CarModel CarModel
		{
			get { return carModel; }
			set { SetField(ref carModel, value, () => CarModel); }
		}

		private CarWindow carWindow;

		public virtual CarWindow CarWindow
		{
			get { return carWindow; }
			set { SetField(ref carWindow, value, () => CarWindow); }
		}

		private string euroCode;

		public virtual string EuroCode
		{
			get { return euroCode; }
			set { SetField(ref euroCode, value, () => EuroCode); }
		}

		private GlassManufacturer manufacturer;

		public virtual GlassManufacturer Manufacturer
		{
			get { return manufacturer; }
			set { SetField(ref manufacturer, value, () => Manufacturer); }
		}

		private decimal cost;

		public virtual decimal Cost
		{
			get { return cost; }
			set { SetField(ref cost, value, () => Cost); }
		}

		private int amount;

		public virtual int Amount
		{
			get { return amount; }
			set { SetField(ref amount, value, () => Amount); }
		}

		private string placement;

		public virtual string Placement
		{
			get { return placement; }
			set { SetField(ref placement, value, () => Placement); }
		}

		private string comment;

		public virtual string Comment
		{
			get { return comment; }
			set { SetField(ref comment, value, () => Comment); }
		}

		public StoreItem()
		{
		}
	}
}
