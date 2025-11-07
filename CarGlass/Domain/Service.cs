using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Extensions.Observable.Collections.List;

namespace CarGlass.Domain
{
	public class Service : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название")]
		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value); }
		}

		private decimal price;

		[Display(Name = "Цена")]
		public virtual decimal Price
		{
			get { return price; }
			set { SetField(ref price, value); }
		}

		private int ordinal;

		[Display(Name = "Последовательность")]
		public virtual int Ordinal
		{
			get { return ordinal; }
			set { SetField(ref ordinal, value); }
		}

		private IObservableList<ServiceOrderType> serviceOrderTypes = new ObservableList<ServiceOrderType>();
		[Display(Name = "список типов заказов, к которым относится услуга")]
		public virtual IObservableList<ServiceOrderType> ServiceOrderTypes {
			get => serviceOrderTypes;
			set => SetField(ref serviceOrderTypes, value);
		}
	}
}
