using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Dialogs;
using CarGlass.Domain;
using Gtk;
using NLog;

namespace CarGlass.Dialogs
{
	public partial class ClientCalendar : Gtk.Window
	{
		public List<CalendarItem>[,] TimeMap = new List<CalendarItem>[7, 24];
		public List<CalendarItem>[,] TimeMapClient = new List<CalendarItem>[7, 24];
		private CalendarHBox[,] CalendarBoxes = new CalendarHBox[7, 24];
		private DateTime _StartDate;
		private int StartTime, EndTime;
		public TypeTab TypeTab;
		public OrdersCalendar OrdersCalendar;

		public bool isOpen = false;

		public Label[] HeadLabels = new Label[7];
		public Label[] HoursLabels = new Label[24];

		public ClientCalendar() : base(Gtk.WindowType.Toplevel) { }

		public void CreaeteClientCalendar()
		{
			this.Build();
			Maximize();

			for(uint i = 1; i <= 7; i++)
			{
				Label templabel = new Label();
				templabel.UseMarkup = true;
				tableOrders.Attach(templabel, i, i + 1, 0, 1, AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
				HeadLabels[i - 1] = templabel;
			};

			tableOrders.SetColSpacing(0, 3);
		}

		public DateTime StartDate
		{
			get { return _StartDate; }
			set
			{
				_StartDate = value;
				//создаем колонки
				for(int i = 1; i <= 7; i++)
				{
					string loc = _StartDate.AddDays(i - 1).ToString("d, ddd")+".";
					string DayName = loc.Length == 6 ? "0" + loc: loc;
					HeadLabels[i - 1].LabelProp =  String.Format("<span size='15000'><b>{0}</b></span>", DayName);
					DayName = _StartDate.AddDays(i - 1).ToLongDateString();
					HeadLabels[i - 1].TooltipText = DayName;
				};
				if(_StartDate.Day > _StartDate.AddDays(7).Day)
					this.Title = String.Format("{0:dd MMMMM}-{1:D}", _StartDate, _StartDate.AddDays(7));
				else
					this.Title = String.Format("{0:dd}-{1:D}", _StartDate, _StartDate.AddDays(7));
			}
		}

		public void SetTimeRange(int StartHour, int EndHour)
		{
			StartTime = StartHour;
			EndHour = 18;
			EndTime = 18;

			tableOrders.NRows = (uint)(EndHour - StartHour);

			uint Position = 1;
			for(int i = StartHour; i <= EndHour; i++)
			{
				Label templabel;
				templabel = new Label(String.Format(" {0:D2}:00 ", i));
				templabel.UseMarkup = true;
				tableOrders.Attach(templabel, 0, 1, Position, Position + 1, AttachOptions.Shrink, AttachOptions.Expand, 0, 0);
				HoursLabels[i] = templabel;
				templabel.Show();

				for(uint x = 1; x <= 7; x++)
				{
					CalendarHBox tempBox = new CalendarHBox(this);
					CalendarBoxes[x - 1, i] = tempBox;
					tableOrders.Attach(tempBox, x, x + 1, Position, Position + 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				}

				Position++;
			}
			tableOrders.ShowAll();
		}

		protected void OnTableOrdersExposeEvent(object o, ExposeEventArgs args)
		{
			if(TimeMap == null) return;
			int x, y, w, h;
			h = tableOrders.Allocation.Height;
			w = tableOrders.Allocation.Width;

			for(int day = 0; day < 7; day++)
			{
				CalendarBoxes[day, 10].TranslateCoordinates(tableOrders, -1, 0, out x, out y);
				tableOrders.GdkWindow.DrawLine(this.Style.ForegroundGC(this.State), x, 0, x, h);
			}
			for(int hour = StartTime; hour <= EndTime; hour++)
			{
				CalendarBoxes[0, hour].TranslateCoordinates(tableOrders, 0, -1, out x, out y);
				tableOrders.GdkWindow.DrawLine(this.Style.ForegroundGC(this.State), 0, y, w, y);
			}
		}


		public void RefreshButtons()
		{
			if(TimeMap == null) return;
			DateTime currentDay = StartDate;
			for(int x = 0; x < 7; x++)
			{
				for(int y = StartTime; y <= EndTime; y++)
				{
					if(TimeMap[x, y] != null)
					{
						TimeMapClient[x, y] = new List<CalendarItem> { new CalendarItem(currentDay, y) {Text = String.Format("     {0:D2}:00 ", y) } };
						TimeMapClient[x, y][0].Color = TimeMapClient[x, y][0].TagColor = "#ff2400";
					}
					else
					{
						var calItem = new CalendarItem(currentDay, y);
							calItem.Text = String.Format("     {0:D2}:00 ", y);
						TimeMapClient[x, y] = new List<CalendarItem> { calItem };
						TimeMapClient[x, y][0].Color = TimeMapClient[x, y][0].TagColor = getColor(calItem, y);
					}
					CalendarBoxes[x, y].ListItems = TimeMapClient[x, y];
				}
				currentDay = currentDay.AddDays(1);
			}
		}

		private string getColor(CalendarItem calendarItem, int hour)
		{
			var dateNow = DateTime.Now;
			if((calendarItem.Date < dateNow.Date) || (calendarItem.Date == dateNow.Date && dateNow.Hour >= hour)) return "#ff2400";
			if(TypeTab != TypeTab.Setting) return "#77dd77";

			if((calendarItem.Date < dateNow.Date) || (calendarItem.Date == dateNow.Date && dateNow.Hour < 11 && hour < 14) ||
				(calendarItem.Date == dateNow.Date && dateNow.Hour >= 11)
				|| (calendarItem.Date == dateNow.AddDays(1).Date && dateNow.Hour >= 11 && hour < 14))
				return "#ff2400";

			return "#77dd77"; 
		}
	}

	public enum TypeTab
	{
		Setting,
		Toning
	}
}
