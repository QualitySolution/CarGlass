using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	[Appellative(Gender = GrammaticalGender.Feminine,
	NominativePlural = "складские позиции",
	Nominative = "складская позиция")]
	public class StoreItem : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private CarBrand carBrand;

		[Required(ErrorMessage = "Марка должна быть заполнена.")]
		public virtual CarBrand CarBrand
		{
			get { return carBrand; }
			set { SetField(ref carBrand, value, () => CarBrand); }
		}

		private CarModel carModel;

		[Required(ErrorMessage = "Модель должна быть заполнена.")]
		public virtual CarModel CarModel
		{
			get { return carModel; }
			set { SetField(ref carModel, value, () => CarModel); }
		}

		private CarWindow carWindow;

		[Required(ErrorMessage = "Размещение стекла должно быть заполнено.")]
		public virtual CarWindow CarWindow
		{
			get { return carWindow; }
			set { SetField(ref carWindow, value, () => CarWindow); }
		}

		private string euroCode;
		[StringLength(45)]
		[Required(ErrorMessage = "Еврокод должен быть заполнен.")]
		public virtual string EuroCode
		{
			get { return euroCode; }
			set { SetField(ref euroCode, value, () => EuroCode); }
		}

		private GlassManufacturer manufacturer;

		[Required(ErrorMessage = "Производитель должен быть заполнен.")]
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

		[StringLength(45)]
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
