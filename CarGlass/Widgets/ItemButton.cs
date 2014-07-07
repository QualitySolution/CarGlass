using System;
using Gtk;
using NLog;

namespace CarGlass
{
	public class ItemButton : Button
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private CalendarItem item;
		private Pango.Layout PangoText;
		public event EventHandler NewOrderClicked;

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
				OnNewOrderClicked();
			base.OnClicked();
		}

		protected void OnNewOrderClicked()
		{
			EventHandler handler = NewOrderClicked;
			if (handler != null)
			{
				EventArgs e = new EventArgs();
				handler(this, e);
			}
		}

		protected override bool OnExposeEvent(Gdk.EventExpose evnt)
		{
			logger.Debug("Button Explose");
			if(item != null)
			{
				Gdk.Rectangle targetRectangle = this.Allocation;
				evnt.Window.DrawRectangle(Style.BackgroundGC (State), true, targetRectangle);
				Style.PaintLayout(Style, evnt.Window, State, true, targetRectangle, this, null, targetRectangle.X, targetRectangle.Y, PangoText);
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

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{

			base.OnSizeAllocated(allocation);
		}
	}
}