using System;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class ServiceOrderType : PropertyChangedBase, IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }

		Service service;
		public virtual Service Service
		{
			get { return service; }
			set { SetField(ref service, value, () => Service); }
		}

		OrderTypeClass orderTypeClass;
		public virtual OrderTypeClass OrderTypeClass
		{
			get { return orderTypeClass; }
			set { SetField(ref orderTypeClass, value, () => OrderTypeClass); }
		}
		#endregion

		public ServiceOrderType()
		{
		}
		public ServiceOrderType(Service service, OrderTypeClass orderTypeClass)
		{
			this.orderTypeClass = orderTypeClass;
			this.Service = service;
		}
	}
}
