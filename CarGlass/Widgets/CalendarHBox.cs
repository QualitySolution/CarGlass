using System;
using Gtk;
using System.Collections.Generic;

namespace CarGlass
{
	public class CalendarHBox : HBox
	{
		private ItemButton emptyButton;
		public event EventHandler NewOrderClicked;

		List<CalendarItem> listItems;

		public List<CalendarItem> ListItems
		{
			get
			{
				return listItems;
			}
			set
			{
				listItems = value;
				UpdateItemsList();
			}
		}

		public CalendarHBox() : base ()
		{
			emptyButton = new ItemButton();
			emptyButton.NewOrderClicked += HandleNewOrderClicked;
			this.Add(emptyButton);
			Drag.DestSet(this, DestDefaults.Highlight, null, 0);
		}

		void HandleNewOrderClicked (object sender, EventArgs e)
		{
			if (NewOrderClicked != null)
				NewOrderClicked(this, e);
		}

		void UpdateItemsList()
		{
			if(listItems == null || listItems.Count == 0)
			{
				if(this.Children.Length == 0 || this.Children[0] != emptyButton)
				{
					RemoveAllWidgets();
					this.Add(emptyButton);
				}
			}
			else
			{
				if(listItems.Count != this.Children.Length || this.Children[0] == emptyButton)
				{
					RemoveAllWidgets();
					foreach(CalendarItem item in listItems)
					{
						ItemButton newButton = new ItemButton();
						newButton.Item = item;
						this.Add(newButton);
					}
				}
				else
				{
					int i = 0;
					foreach(CalendarItem item in listItems)
					{
						((ItemButton)Children[i]).Item = item;
						i++;
					}
				}
			}
			ShowAll();
		}

		void RemoveAllWidgets()
		{
			foreach(Widget wid in this.AllChildren)
			{
				Remove(wid);
				if (wid != emptyButton)
					wid.Destroy();
			}
		}

		public void SetPreviewItem(CalendarItem item)
		{
			emptyButton.Item = item;
			bool exist = false;
			foreach(Widget wid in this.Children)
			{
				if (wid == emptyButton)
					exist = true;
			}
			if (!exist)
				this.Add(emptyButton);
		}

		public void UnsetPreview()
		{
			emptyButton.Item = null;
			if (this.Children.Length > 1)
				this.Remove(emptyButton);
		}
	}
}

