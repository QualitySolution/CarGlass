using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Project.Domain;

namespace CarGlass.Domain
{
	public class Note : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		DateTime date;
		[Display(Name = "Дата")]
		public virtual DateTime Date
		{
			get { return date; }
			set { SetField(ref date, value); }
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

		private string message;
		[Display(Name = "Сообщение")]
		public virtual string Message
		{
			get { return message; }
			set {if(value.Length > 2000)
					value = value.Substring(0, 2000);
				SetField(ref message, value); }
		}

		public Note()
		{
		}


	}
}
