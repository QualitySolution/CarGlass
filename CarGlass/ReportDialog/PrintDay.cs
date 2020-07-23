using System;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace CarGlass
{
	public partial class PrintDay : Gtk.Dialog
	{
		public PrintDay()
		{
			this.Build();

			dateCalendar.Date = DateTime.Today;

			AddCheckButton();
		}

		protected void AddCheckButton()
		{
			using(IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot())
			{
				var listOrderType = uow.Session.QueryOver<OrderTypeClass>().List();
				foreach(var type in listOrderType)
				{
					var title = type.Name;
					var check = new yCheckButton(title);
					check.Label = title;
					check.Tag = type.Id;
					vbox2.Add(check);
					ShowAll();
				}
			}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string type = "";

			foreach(yCheckButton check in vbox2.Children)
			{
				if(type.Length > 0 && check.Active)
					type += ", ";
				if(check.Active)
					type += check.Tag;
			}

			string date = String.Format("{0:u}", dateCalendar.Date).Substring(0, 10);

			string param = String.Format("type={0}&date={1}", type, date);
			ViewReportExt.Run("PrintDay", param);
		}
	}
}

