using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using QS.DomainModel.Entity;

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

		IList<ServiceOrderType> listServiceOrderType = new List<ServiceOrderType>();
		[Display(Name = "список типов заказов, к которым относится услуга")]
		public virtual IList<ServiceOrderType> ListServiceOrderType
		{
			get => listServiceOrderType;
			set => SetField(ref listServiceOrderType, value);
		}

		GenericObservableList<ServiceOrderType> observableServiceOrderType;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<ServiceOrderType> ObservableServiceOrderType
		{
			get
			{
				if(observableServiceOrderType == null)
					observableServiceOrderType = new GenericObservableList<ServiceOrderType>(ListServiceOrderType);
				return observableServiceOrderType;
			}
		}
	}
}
