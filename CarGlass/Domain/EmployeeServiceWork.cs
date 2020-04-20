using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class EmployeeServiceWork : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		Employee employee;

		public virtual Employee Employee
		{
			get { return employee; }
			set { SetField(ref employee, value, () => Employee); }
		}

		WorkOrderPay workOrderPay;
		public virtual WorkOrderPay WorkOrderPay
		{
			get { return workOrderPay; }
			set { SetField(ref workOrderPay, value, () => WorkOrderPay); }
		}

		DateTime dateWork;
		public virtual DateTime DateWork
		{
			get { return dateWork; }
			set { SetField(ref dateWork, value, () => DateWork); }
		}

		public EmployeeServiceWork()
		{
		}

		public EmployeeServiceWork(Employee emp, WorkOrderPay workOrPay, DateTime date)
		{
			this.Employee = emp;
			this.WorkOrderPay = workOrPay;
			this.DateWork = date;
		}
	}
}
