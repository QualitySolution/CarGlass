using System;
using System.Collections.Generic;

namespace CarGlass.Calendar
{
	public class ItemsList
	{
		public readonly List<CalendarItem>[,] TimeMap = new List<CalendarItem>[7, 24];

		public ItemsList()
		{
		}

		public void AddItem(int day, int hour, CalendarItem item)
		{
			if(day > 6 || day < 0) return;
			if(TimeMap[day, hour] == null)
				TimeMap[day, hour] = new List<CalendarItem>();
			TimeMap[day, hour].Add(item);
		}
	}
}
