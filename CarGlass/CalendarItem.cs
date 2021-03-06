﻿using System;
using CarGlass.Domain;

namespace CarGlass
{
	public class CalendarItem
	{
		public DateTime Date;
		public int Hour;
		public int id;
		public string Text;
		public string FullText;
		public string Color;
		public string Tag;
		public string TagColor;
		public uint MessageCount;
		public OrdersCalendar Calendar;
		public TypeItemOrButton TypeItemButton;

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
			OpenOrder?.Invoke(this, EventArgs.Empty);
		}

		public void Delete()
		{
			DeleteOrder?.Invoke(this, EventArgs.Empty);
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

