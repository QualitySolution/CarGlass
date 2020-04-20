using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class EmployeeStatusHistory : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		Employee employee;

		public virtual Employee Employee
		{
			get { return employee; }
			set { SetField(ref employee, value, () => Employee); }
		}

		StatusEmployee status;
		public virtual StatusEmployee Status
		{
			get { return status; }
			set { SetField(ref status, value, () => Status); }
		}

		DateTime dateCreate;
		public virtual DateTime DateCreate
		{
			get { return dateCreate; }
			set { SetField(ref dateCreate, value, () => DateCreate); }
		}
		public EmployeeStatusHistory()
		{
		}

		public EmployeeStatusHistory(Employee emp, StatusEmployee stEmp)
		{
			this.Employee = emp;
			this.Status = stEmp;
			this.DateCreate = DateTime.Now;
		}
	}
}
