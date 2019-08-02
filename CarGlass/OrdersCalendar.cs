using System;
using System.Collections.Generic;
using CarGlass.Domain;
using Gtk;
using NLog;

namespace CarGlass
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrdersCalendar : Gtk.Bin
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public List<CalendarItem>[,] TimeMap;
		public List<OrderType> OrdersTypes;
		private CalendarHBox[,] CalendarBoxes;
		private DateTime _StartDate;
		private int StartTime, EndTime;
		private int dayHilight = -1, hourHilight = -1;

		public Label[] HeadLabels;
		public Label[] HoursLabels;

		private bool DragIn;

		public int PointNumber { get; set;}
		public int CalendarNumber { get; set; }

		public event EventHandler<RefreshOrdersEventArgs> NeedRefreshOrders;
		public event EventHandler<NewOrderEventArgs> NewOrder;

		internal void OnNeedRefreshOrders()
		{
			EventHandler<RefreshOrdersEventArgs> handler = NeedRefreshOrders;
			if (handler != null)
			{
				RefreshOrdersEventArgs e = new RefreshOrdersEventArgs();
				e.StartDate = _StartDate;
				e.StartTime = StartTime;
				e.EndTime = EndTime;
				handler(this, e);
			}
			RefreshButtons();
		}

		public Gdk.Color BackgroundColor
		{
			set{
				eventbox1.ModifyBg(StateType.Normal, value);
			}
		}

		public OrdersCalendar()
		{
			this.Build();

			TimeMap = new List<CalendarItem>[7,24];
			CalendarBoxes = new CalendarHBox[7,24];

			HeadLabels = new Label[7];
			HoursLabels = new Label[24];
			for(uint i = 1; i <= 7; i++)
			{
				Label templabel = new Label();
				templabel.UseMarkup = true;
				tableOrders.Attach(templabel , i, i + 1, 0, 1, AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
				HeadLabels[i-1] = templabel;
			};

			tableOrders.SetColSpacing(0, 3);
		}

		public DateTime StartDate {
			get{ return _StartDate;}
			set{
				_StartDate = value;
				//создаем колонки
				for(int i = 1; i <= 7; i++)
				{
					string DayName = _StartDate.AddDays(i - 1).ToString("d, dddd");
					HeadLabels[i-1].LabelProp = DayName;
					DayName = _StartDate.AddDays(i - 1).ToLongDateString();
					HeadLabels[i-1].TooltipText = DayName;
				};
				if (_StartDate.Day > _StartDate.AddDays(7).Day)
					labelWeek.LabelProp = String.Format("{0:dd MMMMM}-{1:D}", _StartDate, _StartDate.AddDays(7));
				else
					labelWeek.LabelProp = String.Format("{0:dd}-{1:D}", _StartDate, _StartDate.AddDays(7));
				RefreshOrders();
			}
		}

		public void SetTimeRange(int StartHour, int EndHour)
		{
			StartTime = StartHour;
			EndTime = EndHour;

			tableOrders.NRows =  (uint)(EndHour - StartHour + 2);

			uint Position = 1;
			for(int i = StartHour; i <= EndHour; i++)
			{
				Label templabel = new Label(String.Format(" {0:D2}:00 ", i));
				templabel.UseMarkup = true;
				tableOrders.Attach(templabel, 0, 1, Position, Position + 1, AttachOptions.Shrink, AttachOptions.Expand, 0, 0);
				HoursLabels[i] = templabel;
				templabel.Show();
				//Добавляем воксы календаря
				for(uint x = 1; x <= 7; x++)
				{
					CalendarHBox tempBox = new CalendarHBox(this);
					tempBox.NewOrderClicked += OnButtonNewOrderClick;
					tempBox.DragMotion += HandleTargetDragMotion;
					tempBox.DragLeave += HandleTargetDragLeave;
					tempBox.DragDrop += HandleTargetDragDrop;
					CalendarBoxes[x - 1, i] = tempBox;
					tableOrders.Attach(tempBox, x, x + 1, Position, Position + 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				}
				Position++;
			}
			tableOrders.ShowAll();
		}

		public void ClearTimeMap()
		{
			TimeMap = new List<CalendarItem>[7,24];
		}

		public void AddItem(int day, int hour, CalendarItem item)
		{
			if (TimeMap[day, hour] == null)
				TimeMap[day, hour] = new List<CalendarItem>();
			TimeMap[day, hour].Add(item);
		}

		protected void OnButtonRefreshClicked(object sender, EventArgs e)
		{
			OnNeedRefreshOrders();
		}

		protected void OnButtonNewOrderClick(object sender, NewOrderEventArgs e)
		{
			CalendarHBox box = (CalendarHBox)sender;
			EventHandler<NewOrderEventArgs> handler = NewOrder;
			if (handler != null)
			{
				int x, y;
				GetCalendarPosition(box, out x, out y);
				e.Date = _StartDate.AddDays(x);
				e.Hour = (ushort)y;
				e.PointNumber = (ushort)PointNumber;
				e.CalendarNumber = (ushort)CalendarNumber;
				e.result = false;
				handler(this, e);
				if (e.result)
					RefreshOrders();
			}
		}

		public void RefreshOrders()
		{
			if(_StartDate != default(DateTime) && EndTime > 0)
				OnNeedRefreshOrders();
		}

		private void RefreshButtons()
		{
			for(int x = 0; x < 7; x++)
			{
				for(int y = StartTime; y <= EndTime; y++)
				{
					CalendarBoxes[x, y].ListItems = TimeMap[x, y];
				}
			}
		}

		protected void OnButtonLeftClicked(object sender, EventArgs e)
		{
			this.StartDate = this.StartDate.AddDays(-7);
		}

		protected void OnButtonRightClicked(object sender, EventArgs e)
		{
			this.StartDate = this.StartDate.AddDays(7);
		}

		//Таскание
		void HandleTargetDragMotion(object sender, Gtk.DragMotionArgs args)
		{
			CalendarHBox target = (CalendarHBox)sender;
			ItemButton source = (ItemButton)Drag.GetSourceWidget(args.Context);
			if(!DragIn)
			{
				DragIn = true;
				logger.Debug ("Set DragIn=true;");
				target.SetPreviewItem(source.Item);
			}
			Gdk.Drag.Status (args.Context,
				args.Context.SuggestedAction,
				args.Time);
			args.RetVal = true;
		}

		void HandleTargetDragDrop(object sender, Gtk.DragDropArgs args)
		{
			logger.Debug ("drop");
			DragIn = false;
			CalendarHBox target = (CalendarHBox)sender;
			ItemButton source = (ItemButton)Drag.GetSourceWidget(args.Context);

			int day, hour;
			GetCalendarPosition(target, out day, out hour);
			source.Item.ChangeTime(_StartDate.AddDays(day), hour);
			Gtk.Drag.Finish (args.Context, true, false, args.Time);
			RefreshOrders();
			args.RetVal = false;
		}

		private void HandleTargetDragLeave (object sender, DragLeaveArgs args)
		{
			logger.Debug ("leave");
			DragIn = false;

			CalendarHBox target = (CalendarHBox)sender;
			target.UnsetPreview();
		}

		protected void OnTableOrdersExposeEvent(object o, ExposeEventArgs args)
		{
			logger.Debug("Table Explose");
			int x, y, w, h;
			h = tableOrders.Allocation.Height;
			w = tableOrders.Allocation.Width;

			logger.Debug("size: {0}, {1}", w, h);
			for (int day = 0; day < 7; day++)
			{
				if (CalendarBoxes[day, 10] == null)
					continue;
				CalendarBoxes[day, 10].TranslateCoordinates(tableOrders, -1, 0, out x, out y);
				tableOrders.GdkWindow.DrawLine(this.Style.ForegroundGC (this.State), x, 0, x, h);
			}
			for (int hour = StartTime; hour <= EndTime; hour++)
			{
				if (CalendarBoxes[0, hour] == null)
					continue;
				CalendarBoxes[0, hour].TranslateCoordinates(tableOrders, 0, -1, out x, out y);
				//logger.Debug("cor: {0}, {1}", x, y);
				tableOrders.GdkWindow.DrawLine(this.Style.ForegroundGC (this.State), 0, y, w, y);
			}
		}

		private bool GetCalendarPosition(CalendarHBox cell, out int day, out int hour)
		{
			day = -1;
			hour = -1;
			for (int day1 = 0; day1 < 7; day1++)
			{
				for (int hour1 = StartTime; hour1 <= EndTime; hour1++)
				{
					if (cell == CalendarBoxes[day1, hour1])
					{
						day = day1;
						hour = hour1;
						return true;
					}
				}
			}
			return false;
		}

		protected void OnEventbox1MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			int x, y;
			int newDayHilight = -1;
			int newHourHilight = -1;

			//Ищем точку
			for (int day = 0; day < 7; day++)
			{
				if (CalendarBoxes[day, 10] == null)
					continue;
				CalendarBoxes[day, 10].TranslateCoordinates(tableOrders, 0, 0, out x, out y);
				int columnWidth = CalendarBoxes[day, 10].Allocation.Width;
				if(args.Event.X > x && args.Event.X < columnWidth + x)
				{
					newDayHilight = day;
				}
			}
			for (int hour = StartTime; hour <= EndTime; hour++)
			{
				if (CalendarBoxes[0, hour] == null)
					continue;
				CalendarBoxes[0, hour].TranslateCoordinates(tableOrders, 0, 0, out x, out y);
				int columnHeight = CalendarBoxes[0, hour].Allocation.Height;
				if(args.Event.Y > y && args.Event.Y < columnHeight + y)
				{
					newHourHilight = hour;
				}
			}
			//Проверяем надо ли что нибудь менять
			if(dayHilight != newDayHilight)
			{
				if(dayHilight != -1)
					HeadLabels[dayHilight].LabelProp = _StartDate.AddDays(dayHilight).ToString("d, dddd");
				dayHilight = newDayHilight;
				if(dayHilight != -1)
					HeadLabels[dayHilight].LabelProp = String.Format("<span foreground=\"red\">{0}</span>", _StartDate.AddDays(dayHilight).ToString("d, dddd"));
			}
			if(hourHilight != newHourHilight)
			{
				if(hourHilight != -1)
					HoursLabels[hourHilight].LabelProp = String.Format(" {0:D2}:00 ", hourHilight);
				hourHilight = newHourHilight;
				if(hourHilight != -1)
					HoursLabels[hourHilight].LabelProp = String.Format("<span foreground=\"red\"><b>{0:D2}:00</b></span>", hourHilight);
			}
		}

		protected void OnEventbox1LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if(dayHilight != -1)
				HeadLabels[dayHilight].LabelProp = _StartDate.AddDays(dayHilight).ToString("d, dddd");
			if(hourHilight != -1)
				HoursLabels[hourHilight].LabelProp = String.Format(" {0:D2}:00 ", hourHilight);
			dayHilight = -1;
			hourHilight = -1;
		}

	}

	public class NewOrderEventArgs : EventArgs
	{
		public DateTime Date;
		public ushort Hour;
		public OrderType OrderType;
		public ushort PointNumber;
		public ushort CalendarNumber;
		public bool result { get; set; }
	}

	public class RefreshOrdersEventArgs : EventArgs
	{
		public DateTime StartDate { get; set; }
		public int StartTime { get; set; }
		public int EndTime { get; set; }
	}

}

