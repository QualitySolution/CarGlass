using System;
using System.ComponentModel.DataAnnotations;
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

		private OrderType orderType;

		[Display(Name = "Тип заказа")]
		public virtual OrderType OrderType
		{
			get { return orderType; }
			set { SetField(ref orderType, value); }
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
	}
}
