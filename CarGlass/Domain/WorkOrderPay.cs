using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class WorkOrderPay : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private WorkOrder workOrder;

		[Display(Name = "Заказ")]
		public virtual WorkOrder WorkOrder
		{
			get { return workOrder; }
			set { SetField(ref workOrder, value); }
		}

		private Service service;

		[Display(Name = "Услуга")]
		public virtual Service Service
		{
			get { return service; }
			set { SetField(ref service, value); }
		}

		private decimal cost;

		[Display(Name = "Стоимость")]
		public virtual decimal Cost
		{
			get { return cost; }
			set { SetField(ref cost, value); }
		}

		public WorkOrderPay()
		{
		}

		public WorkOrderPay(WorkOrder order, Service service)
		{
			workOrder = order;
			this.service = service;
		}
	}
}
