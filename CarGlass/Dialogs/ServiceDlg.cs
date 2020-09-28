using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;
using CarGlass.Domain;
using QS.DomainModel.UoW;
using System.Linq;


namespace CarGlass
{
	public partial class ServiceDlg : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		public bool NewItem;
		Service service = new Service();

		public ServiceDlg()
		{
			this.Build();
			comboboxOrderType.ItemsList = UoW.Session.QueryOver<OrderTypeClass>().List();

		}

		public void Fill(int id)
		{
			NewItem = false;
			service = UoW.Session.QueryOver<Service>().List().FirstOrDefault(x => x.Id == id);
			logger.Info("Запрос услуги №{0}...", id);
            labelID.Text = service.Id.ToString();
            entryName.Text = service.Name;
			if(service.ListServiceOrderType.Count > 0)
			{
				OrderTypeClass orderTypeClass = service.ListServiceOrderType.First().OrderTypeClass;
				comboboxOrderType.SelectedItem = orderTypeClass;
			}
			spinPrice.Value = double.Parse(service.Price.ToString()); 
			logger.Info("Ok");
            this.Title = entryName.Text;

            TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool TypeOK = comboboxOrderType.SelectedItem != null;
			buttonOk.Sensitive = Nameok && TypeOK;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			service.Name = entryName.Text;
			if(service.ListServiceOrderType.Count < 1)
			{
				ServiceOrderType serOrderType = new ServiceOrderType(service, (OrderTypeClass)comboboxOrderType.SelectedItem);
				service.ListServiceOrderType.Add(serOrderType);

			}
			else
			{
				service.ListServiceOrderType[0].OrderTypeClass = (OrderTypeClass)comboboxOrderType.SelectedItem;
				service.ListServiceOrderType[0].Service = service;
			}
			service.Price = decimal.Parse(spinPrice.Value.ToString());

			UoW.Save(service);
			UoW.Commit();
			logger.Info("Ok");

			Respond(ResponseType.Ok);

		}

		protected void OnEntryNameChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboTypeChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}
	}
}

