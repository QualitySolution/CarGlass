using System;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class SheduleEmployeeWork : PropertyChangedBase, IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }

		ScheduleWorks scheduleWorks;
		public virtual ScheduleWorks ScheduleWorks
		{
			get { return scheduleWorks; }
			set { SetField(ref scheduleWorks, value, () => ScheduleWorks); }
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

		public SheduleEmployeeWork (ScheduleWorks scheduleWorks, Employee employee)
		{
			this.scheduleWorks = scheduleWorks;
			this.employee = employee;
		}
	}
}
