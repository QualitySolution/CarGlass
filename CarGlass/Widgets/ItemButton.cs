using System;
using System.Collections.Generic;
using Gtk;
using NLog;
using QSWidgetLib;

namespace CarGlass
{
	public class ItemButton : Button
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private CalendarItem item;
		private Pango.Layout PangoText;
		private Pango.Layout PangoTag;
		public OrdersCalendar ParentCalendar;
		public event EventHandler<NewOrderEventArgs> NewOrderClicked;

		public CalendarItem Item
		{
			get
			{
				return item;
			}
			set
			{
				item = value;
				UpdateButton();
			}
		}

		public ItemButton() : base ()
		{
			HeightRequest = 32;
			PangoText = new Pango.Layout(this.PangoContext);
			PangoTag = new Pango.Layout(this.PangoContext);
			Relief = ReliefStyle.None;
		}

		protected override bool OnEnterNotifyEvent(Gdk.EventCrossing evnt)
		{
			if (this.Relief == ReliefStyle.None && this.Image == null)
				this.Image = new Image("gtk-add", IconSize.Button);
			return base.OnEnterNotifyEvent(evnt);
		}

		protected override bool OnLeaveNotifyEvent(Gdk.EventCrossing evnt)
		{
			if (this.Image != null)
			{
				this.Image.Destroy();
				this.Image = null;
			}
			return base.OnLeaveNotifyEvent(evnt);
		}

		protected override void OnClicked()
		{
			if (item != null)
				item.Open();
			else
			{
				if(ParentCalendar.OrdersTypes == null || ParentCalendar.OrdersTypes.Count == 0)
					OnNewOrderClicked(-1);
				else if(ParentCalendar.OrdersTypes.Count == 1)
					OnNewOrderClicked(ParentCalendar.OrdersTypes.GetEnumerator().Current.Key);
				else
				{
					Gtk.Menu jBox = GetNewOrderTypesMenu();
					jBox.ShowAll();
					jBox.Popup();
				}
			}

			base.OnClicked();
		}

		protected void OnNewOrderClicked(int ordertypeid)
		{
			EventHandler<NewOrderEventArgs> handler = NewOrderClicked;
			if (handler != null)
			{
				NewOrderEventArgs e = new NewOrderEventArgs();
				e.OrderType = ordertypeid;
				handler(this, e);
			}
		}

		protected override bool OnExposeEvent(Gdk.EventExpose evnt)
		{
			logger.Debug("Button Explose");
			if(item != null)
			{
				int triangleSize = 25;
				Gdk.Rectangle targetRectangle = this.Allocation;
				evnt.Window.DrawRectangle(Style.BackgroundGC(State), true, targetRectangle);
				Style.PaintLayout(Style, evnt.Window, State, true, targetRectangle, this, null, targetRectangle.X, targetRectangle.Y, PangoText);
				if (item.TagColor != "")
				{
					Gdk.Color col = new Gdk.Color();
					Gdk.Color.Parse(item.TagColor, ref col);
					Gdk.GC TagGC = new Gdk.GC(evnt.Window);
					TagGC.RgbFgColor = col;

					Gdk.Point[] triangle = new Gdk.Point[]
					{
						new Gdk.Point(targetRectangle.X + targetRectangle.Width, targetRectangle.Top),
						new Gdk.Point(targetRectangle.X + targetRectangle.Width - triangleSize, targetRectangle.Top),
						new Gdk.Point(targetRectangle.X + targetRectangle.Width, targetRectangle.Top + triangleSize)
					};
					evnt.Window.DrawPolygon(TagGC, true, triangle);

					if (Item.Tag != "")
					{
						Pango.Rectangle logicExt, inkExt;
						PangoTag.GetPixelExtents(out inkExt, out logicExt);
						evnt.Window.DrawLayout(Style.WhiteGC, targetRectangle.Right - triangleSize * 5/16 - logicExt.Width/2, targetRectangle.Top + triangleSize * 5/ 16 - logicExt.Height/2, PangoTag);
					}
				}
				return true;
			}

			return base.OnExposeEvent(evnt);
		}

		private void UpdateButton()
		{
			if(item != null)
			{
				this.Image = null;
				PangoText.SetText(item.Text);
				this.TooltipText = item.FullText;
				this.Relief = ReliefStyle.Normal;
				Drag.DestUnset(this);
				Drag.SourceSet(this, Gdk.ModifierType.Button1Mask, null, Gdk.DragAction.Move );
				Gdk.Color col = new Gdk.Color();
				Gdk.Color.Parse(item.Color, ref col);
				logger.Debug("a={0} - {1} - {2}", col.Red, col.Green, col.Blue);
				this.ModifyBg(StateType.Normal, col);
				byte r = (byte) Math.Min(((double)col.Red / ushort.MaxValue) * byte.MaxValue + 30 , byte.MaxValue);
				byte g = (byte) Math.Min(((double)col.Green / ushort.MaxValue) * byte.MaxValue + 30 , byte.MaxValue);
				byte b = (byte) Math.Min(((double)col.Blue / ushort.MaxValue) * byte.MaxValue + 30 , byte.MaxValue);
				col = new Gdk.Color(r, g, b);
				this.ModifyBg(StateType.Prelight, col);
				logger.Debug("b={0} - {1} - {2}", col.Red, col.Green, col.Blue);
				//Tag
				PangoTag.SetText(item.Tag);
			}
			else
			{
				this.Image = null;
				this.TooltipText = null;
				Drag.SourceUnset(this);
				this.Relief = ReliefStyle.None;
				this.ModifyBg(StateType.Normal);
			}

		}

		protected override bool OnButtonReleaseEvent(Gdk.EventButton evnt)
		{
			if(evnt.Button == 3)
			{
				Gtk.Menu jBox = new Gtk.Menu();
				MenuItem MenuItem1;
				MenuItemId<int> MenuItem2;

				if(ParentCalendar.OrdersTypes == null || ParentCalendar.OrdersTypes.Count == 0)
				{
					MenuItem1 = new MenuItem("Новый заказ");
					MenuItem1.Activated += OnButtonPopupAdd;
					jBox.Add(MenuItem1);       
				}
				else if(ParentCalendar.OrdersTypes.Count == 1)
				{
					MenuItem2 = new MenuItemId<int>("Новый заказ");
					MenuItem2.ID = ParentCalendar.OrdersTypes.GetEnumerator().Current.Key;
					MenuItem2.ButtonPressEvent += OnButtonPopupAddWithType;
					jBox.Add(MenuItem2);       
				}
				else
				{
					MenuItem1 = new MenuItem("Новый заказ");
					MenuItem1.Submenu = GetNewOrderTypesMenu();
					jBox.Add(MenuItem1);       
				}

				MenuItem1 = new MenuItem("Удалить");
				MenuItem1.Sensitive = item != null;
				MenuItem1.Activated += OnButtonPopupDelete;
				jBox.Add(MenuItem1);       
				jBox.ShowAll();
				jBox.Popup();
			}

			return base.OnButtonReleaseEvent(evnt);
		}

		private Gtk.Menu GetNewOrderTypesMenu()
		{
			Gtk.Menu jBox2 = new Gtk.Menu();
			MenuItemId<int> MenuItem2;
			foreach(KeyValuePair<int, string> pair in ParentCalendar.OrdersTypes)
			{
				MenuItem2 = new MenuItemId<int>(pair.Value);
				MenuItem2.ID = pair.Key;
				MenuItem2.ButtonPressEvent += OnButtonPopupAddWithType;
				jBox2.Add(MenuItem2);       
			}
			return jBox2;
		}

		protected void OnButtonPopupDelete(object sender, EventArgs Arg)
		{
			item.Delete();
		}

		protected void OnButtonPopupAdd(object sender, EventArgs Arg)
		{
			OnNewOrderClicked(-1);
		}

		protected void OnButtonPopupAddWithType(object sender, ButtonPressEventArgs Arg)
		{
			MenuItemId<int> item = (MenuItemId<int>)sender;
			OnNewOrderClicked(item.ID);
		}

	}
}