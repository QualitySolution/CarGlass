using System;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class SheduleEmployeeWork : PropertyChangedBase, IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }

		SheduleWorks sheduleWorks;
		public virtual SheduleWorks SheduleWorks
		{
			get { return sheduleWorks; }
			set { SetField(ref sheduleWorks, value, () => SheduleWorks); }
		}

		Employee employee;
		public virtual Employee Employee
		{
			get { return employee; }
			set { SetField(ref employee, value, () => Employee); }
		}
		#endregion


		public SheduleEmployeeWork()
		{ 
		}

		public SheduleEmployeeWork (SheduleWorks sheduleWorks, Employee employee)
		{
			this.sheduleWorks = sheduleWorks;
			this.employee = employee;
		}
	}
}
