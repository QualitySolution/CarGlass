using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Extensions.Observable.Collections.List;
using QS.Project.Domain;

namespace CarGlass.Domain
{
	[Appellative(Nominative = "график работ", Prepositional = "графике работ")]
	public class ScheduleWorks : PropertyChangedBase, IDomainObject
	{
		#region Свойства
		public virtual int Id { get; set; }

		DateTime dateWork;
		public virtual DateTime DateWork
		{
			get { return dateWork; }
			set { SetField(ref dateWork, value, () => DateWork); }
		}

		DateTime dateCreate;

		public virtual DateTime DateCreate
		{
			get { return dateCreate; }
			set { SetField(ref dateCreate, value, () => DateCreate); }
		}

		private UserBase createdBy;

		[Display(Name = "Автор")]
		public virtual UserBase CreatedBy
		{
			get { return createdBy; }
			set { SetField(ref createdBy, value); }
		}

		private ushort pointNumber;

		[Display(Name = "Место:пригород/город")]
		public virtual ushort PointNumber
		{
			get { return pointNumber; }
			set { SetField(ref pointNumber, value); }
		}

		private ushort calendarNumber;

		[Display(Name = "Вид календаря")]
		public virtual ushort CalendarNumber
		{
			get { return calendarNumber; }
			set { SetField(ref calendarNumber, value); }
		}
		#endregion

		#region Коллекции
		private IObservableList<SheduleEmployeeWork> scheduleEmployeeWorks = new ObservableList<SheduleEmployeeWork>();
		[Display(Name = "список сотрудников работающих в этот день")]
		public virtual IObservableList<SheduleEmployeeWork> ScheduleEmployeeWorks {
			get => scheduleEmployeeWorks;
			set => SetField(ref scheduleEmployeeWorks, value);
		}
		#endregion

		public ScheduleWorks()
		{
		}
	}
}
