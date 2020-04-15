using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
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

		IList<EmployeeServiceWork> employeeServiceWork = new List<EmployeeServiceWork>();
		[Display(Name = "исполняющие")]
		public virtual IList<EmployeeServiceWork> EmployeeServiceWork
		{
			get => employeeServiceWork;
			set => SetField(ref employeeServiceWork, value);
		}

		GenericObservableList<EmployeeServiceWork> observableEmployeeServiceWork;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<EmployeeServiceWork> ObservableEmployeeServiceWork
		{
			get
			{
				if(observableEmployeeServiceWork == null)
					observableEmployeeServiceWork = new GenericObservableList<EmployeeServiceWork>(EmployeeServiceWork);
				return observableEmployeeServiceWork;
			}
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
