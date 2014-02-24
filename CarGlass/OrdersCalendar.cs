using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data.MySqlClient;
using QSWidgetLib;

namespace CarGlass
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrdersCalendar : Gtk.Bin
	{
		public CalendarItem[,] TimeMap;
		public Dictionary<int, string> OrdersTypes;
		private Button[,] CalendarButtons;
		private DateTime _StartDate;
		private int StartTime, EndTime;

		private Label[] HeadLabels;
		private Label[] HoursLabels;

		private int PopupPosX, PopupPosY;

		public event EventHandler<RefreshOrdersEventArgs> NeedRefreshOrders;
		public event EventHandler<NewOrderEventArgs> NewOrder;

		public class NewOrderEventArgs : EventArgs
		{
			public DateTime Date;
			public int Hour;
			public int OrderType;
			public bool result { get; set; }
		}

		public class RefreshOrdersEventArgs : EventArgs
		{
			public DateTime StartDate { get; set; }
			public int StartTime { get; set; }
			public int EndTime { get; set; }
		}

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

		public bool CreateNewOrder(DateTime date, int hour, int ordertype)
		{
			EventHandler<NewOrderEventArgs> handler = NewOrder;
			if (handler != null)
			{
				NewOrderEventArgs arg = new NewOrderEventArgs();
				arg.Date = date;
				arg.Hour = hour;
				arg.OrderType = ordertype;
				arg.result = false;
				handler(this, arg);
				if (arg.result)
					RefreshOrders();
				return arg.result;
			}
			return false;
		}
			
		public OrdersCalendar()
		{
			this.Build();

			TimeMap = new CalendarItem[7,24];
			CalendarButtons = new Button[7,24];

			HeadLabels = new Label[7];
			HoursLabels = new Label[24];
			for(uint i = 1; i <= 7; i++)
			{
				Label templabel = new Label();
				tableOrders.Attach(templabel , i, i + 1, 0, 1, AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
				HeadLabels[i-1] = templabel;
			};

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
				Label templabel = new Label(String.Format("{0:D2}:00", i));

				tableOrders.Attach(templabel, 0, 1, Position, Position + 1, AttachOptions.Shrink, AttachOptions.Expand, 0, 0);
				HoursLabels[i] = templabel;
				templabel.Show();
				//Добавляем кнопки заказов
				for(uint x = 1; x <= 7; x++)
				{
					Label label = new Label();
					label.Justify = Justification.Center;
					label.Ellipsize = Pango.EllipsizeMode.Middle;
					label.CanFocus = false;

					Button tempbut = new Button();
					tempbut.Relief = ReliefStyle.None;
					tempbut.Add(label);
					tempbut.HeightRequest = 32;
					tempbut.EnterNotifyEvent += OnButtonEnterNotifyEvent;
					tempbut.LeaveNotifyEvent += OnButtonLeaveNotifyEvent;
					tempbut.Clicked += OnButtonClick;
					tableOrders.Attach(tempbut, x, x + 1, Position, Position + 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
					CalendarButtons[x - 1, i] = tempbut;
					tempbut.Show();
				}
				Position++;
			}
		}

		public void ClearTimeMap()
		{
			TimeMap = new CalendarItem[7,24];
		}

		protected void OnButtonEnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (((Button)o).Relief == ReliefStyle.None && ((Button)o).Image == null)
				((Button)o).Image = new Image("gtk-add", IconSize.Button);
		}

		protected void OnButtonLeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (((Button)o).Image != null)
			{
				((Button)o).Image.Destroy();
				((Button)o).Image = null;
			}
		}

		protected void OnButtonRefreshClicked(object sender, EventArgs e)
		{
			OnNeedRefreshOrders();
		}

		protected void OnButtonClick(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			if(button.Relief == ReliefStyle.Normal)
			{
				for (int x = 0; x < 7; x++)
				{
					for (int y = StartTime; y <= EndTime; y++)
					{
						if (CalendarButtons[x, y] == button)
							TimeMap[x, y].Open();
					}
				}
			}
			else
			{
				for (int x = 0; x < 7; x++)
				{
					for (int y = StartTime; y <= EndTime; y++)
					{
						if (CalendarButtons[x, y] == button)
						{
							if(OrdersTypes == null || OrdersTypes.Count == 0)
								CreateNewOrder(_StartDate.AddDays(x), y, -1);
							else if(OrdersTypes.Count == 1)
								CreateNewOrder(_StartDate.AddDays(x), y, OrdersTypes.GetEnumerator().Current.Key);
							else
							{
								Gtk.Menu jBox = new Gtk.Menu();
								MenuItemId<int> MenuItem1;
								foreach(KeyValuePair<int, string> pair in OrdersTypes)
								{
									MenuItem1 = new MenuItemId<int>(pair.Value);
									MenuItem1.ID = pair.Key;
									MenuItem1.Activated += OnButtonPopupType;
									jBox.Add(MenuItem1);       
								}
								PopupPosX = x;
								PopupPosY = y;
								jBox.ShowAll();
								jBox.Popup();
							}
						}
					}
				}
			}
		}

		protected void OnButtonPopupType(object sender, EventArgs Arg)
		{
			MenuItemId<int> item = (MenuItemId<int>)sender;
			CreateNewOrder(_StartDate.AddDays(PopupPosX), PopupPosY, item.ID);
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
					Button temp = CalendarButtons[x, y];
					if(TimeMap[x, y] != null)
					{
						temp.Image = null;
						temp.Label = TimeMap[x, y].Text;
						temp.Relief = ReliefStyle.Normal;
						Gdk.Color col = new Gdk.Color();
						Gdk.Color.Parse(TimeMap[x, y].Color, ref col);
						temp.ModifyBg(StateType.Normal, col);
						Console.WriteLine(col.ToString());
					}
					else
					{
						temp.Image = null;
						temp.Label = null;
						temp.Relief = ReliefStyle.None;
						temp.ModifyBg(StateType.Normal);
					}
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

	}

}

