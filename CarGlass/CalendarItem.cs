using System;
using CarGlass.Domain;

namespace CarGlass
{
	public class CalendarItem
	{
		public DateTime Date;
		public int Hour;
		public int id;
		public OrderTypeClass OrderType;
		public string Text;
		public string FullText;
		public string Color;
		public string Tag;
		public string TagColor;
		public TypeItemOrButton TypeItemOrButton;
		public bool isClientCalendarItem;
		public OrdersCalendar Calendar;

		public event EventHandler OpenOrder;
		public event EventHandler DeleteOrder;
		public event EventHandler<TimeChangedEventArgs> TimeChanged;

		public class TimeChangedEventArgs : EventArgs
		{
			public DateTime Date;
			public int Hour;
		}

		public CalendarItem(DateTime date, int hour)
		{
			Date = date;
			Hour = hour;
		}

		public void Open()
		{
			EventHandler handler = OpenOrder;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public void Delete()
		{
			EventHandler handler = DeleteOrder;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public void ChangeTime(DateTime date, int hour)
		{
			EventHandler<TimeChangedEventArgs> handler = TimeChanged;
			if (handler != null)
			{
				TimeChangedEventArgs arg = new TimeChangedEventArgs();
				arg.Date = date;
				arg.Hour = hour;
				handler(this, arg);
			}

			Date = date;
			Hour = hour;
		}
	}
}

