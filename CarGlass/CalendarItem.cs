using System;

namespace CarGlass
{
	public class CalendarItem
	{
		public DateTime Date;
		public int Hour;
		public int id;
		public int Type;
		public string Text;
		public string Color;

		public event EventHandler OpenOrder;

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
				
	}
}

