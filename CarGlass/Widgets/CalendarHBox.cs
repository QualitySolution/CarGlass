using System;
using Gtk;
using System.Collections.Generic;
using CarGlass.Domain;

namespace CarGlass
{
	public class CalendarHBox : HBox
	{
		private ItemButton emptyButton;
		public event EventHandler<NewOrderEventArgs> NewOrderClicked;
		public event EventHandler<NewSheduleWorkEventArgs> NewSheduleWorkClicked;
		public event EventHandler<NewNoteEventArgs> NewNoteClicked;
		private OrdersCalendar ParentCalendar;

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


		public CalendarHBox(OrdersCalendar calendar) : base ()
		{
			ParentCalendar = calendar;
			emptyButton = new ItemButton();
			emptyButton.ParentCalendar = ParentCalendar;
			emptyButton.isSetSheduleWork = false;
			emptyButton.NewOrderClicked += HandleNewOrderClicked;
			this.Add(emptyButton);
			Drag.DestSet(this, DestDefaults.Highlight, null, 0);
		}

		public CalendarHBox(OrdersCalendar calendar, string str) : base()
		{
			ParentCalendar = calendar;
			emptyButton = new ItemButton();
			emptyButton.ParentCalendar = ParentCalendar;

			if(str.Equals("newSheduleWork"))
			{
				emptyButton.isSetSheduleWork = true;
				emptyButton.NewSheduleWorkClicked += HandleNewSheduleWorkClicked;
			}
			else if(str.Equals("newNote"))
            {
				emptyButton.NewNoteClicked += HandleNewNoteClicked;
				emptyButton.isSetNote = true;
			}
			this.Add(emptyButton);
		}

		void HandleNewOrderClicked (object sender, NewOrderEventArgs e)
		{
			if (NewOrderClicked != null)
			{
				NewOrderClicked(this, e);
			}
		}

		void HandleNewSheduleWorkClicked(object sender, NewSheduleWorkEventArgs e)
		{
			NewSheduleWorkClicked?.Invoke(this, e);
		}

		void HandleNewNoteClicked(object sender, NewNoteEventArgs e)
		{
			NewNoteClicked?.Invoke(this, e);
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
						newButton.ParentCalendar = ParentCalendar;
						newButton.NewOrderClicked += HandleNewOrderClicked;
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

