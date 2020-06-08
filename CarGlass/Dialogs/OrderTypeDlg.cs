using System;
using System.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using NLog;
using QSProjectsLib;
using CarGlass.Domain;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using Gtk;
namespace CarGlass.Dialogs
{
	public partial class OrderTypeDlg : Gtk.Dialog
	{

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public bool NewItem;
		int ItemId;
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		List<Service> listService = new List<Service>();
		OrderTypeClass orderTypeClass = new OrderTypeClass();

		public OrderTypeDlg()
		{
			this.Build();
			    
		}
		public void Fill(int id)
		{
			ItemId = id;
			NewItem = false;

			logger.Info("Запрос типа заказов №{0}...", id);

			orderTypeClass = UoW.Session.QueryOver<OrderTypeClass>().List().FirstOrDefault(x => x.Id == ItemId);
			var l = UoW.Session.QueryOver<ServiceOrderType>().List().Where(x => x.OrderTypeClass.Id == ItemId).ToList();
			foreach(var ser in l)
				listService.Add(UoW.Session.QueryOver<Service>().List().First(x=> x == ser.Service));
			entryNumberOrder.Text = orderTypeClass.Id.ToString();
			entryNameOrder.Text = orderTypeClass.Name.ToString();

			checkbuttonCalculationSalary.Active = orderTypeClass.IsCalculateSalary;
			if(orderTypeClass.PositionInTabs == null)
				orderTypeClass.PositionInTabs = "";

			checkbuttonInstallationSuburban.Active = orderTypeClass.PositionInTabs.Contains(checkbuttonInstallationSuburban.Label);
			checkbuttonSuburbanTinting.Active = orderTypeClass.PositionInTabs.Contains(checkbuttonSuburbanTinting.Label);
			checkbuttonOrder.Active = orderTypeClass.PositionInTabs.Contains(checkbuttonOrder.Label);
			checkbuttonTintedEntry.Active = orderTypeClass.PositionInTabs.Contains(checkbuttonTintedEntry.Label);
			checkbuttonInstall.Active = orderTypeClass.IsInstallType;
			checkbuttonOther.Active = orderTypeClass.IsOtherType;

			checkbuttonMainParameters.Active = orderTypeClass.IsShowMainWidgets;
			checkbuttonAdditionalParameters.Active = orderTypeClass.IsShowAdditionalWidgets;

			logger.Info("Ok");
			this.Title = $"Редактирование типа заказа: \"{orderTypeClass.Name}\" ";
			createTable();
			TestCanSave();
		}

		public void createTable()
		{
			ytreeviewService.ItemsDataSource = listService;
			ytreeviewService.ColumnsConfig = ColumnsConfigFactory.Create<Service>()
			.AddColumn("Id").AddNumericRenderer(x => x.Id)
			.AddColumn("Наименование").AddTextRenderer(x => x.Name)
			.AddColumn("Цена").AddNumericRenderer(x => x.Price)
			.Finish();
		}

		public void TestCanSave()
		{
			buttonOk.Sensitive = entryNameOrder.Text.Length > 0;
		}

		protected void OnEntryNameOrderActivated(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnButtonAddServiceClicked(object sender, EventArgs e)
		{
			Reference ServiceSelect = new Reference();
			ServiceSelect.SetMode(false, true, false, false, false);
			ServiceSelect.SqlSelect = "SELECT ser.id, ser.name FROM services ser " +
                "LEFT JOIN service_order_type ordt on ordt.id_service = ser.id where ordt.id is null; ";
			ServiceSelect.FillList("services", "Услуги", "Услуг");
			ServiceSelect.Show();
			int result = ServiceSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Service ser = UoW.Session.QueryOver<Service>().List().First(x => x.Id == ServiceSelect.SelectedID);
				if(!listService.Contains(ser))
					listService.Add(ser);
				if(orderTypeClass.ListServiceOrderType.FirstOrDefault(x => x.Service == ser) == null)
					orderTypeClass.ListServiceOrderType.Add(new ServiceOrderType(ser, this.orderTypeClass));
			}
			createTable();
			ServiceSelect.Destroy();

		}

		protected void OnButtonDeleteServiceClicked(object sender, EventArgs e)
		{
			var ser = ytreeviewService.GetSelectedObject<Service>();
			listService.Remove(ser);
			var serOrderType = orderTypeClass.ListServiceOrderType.FirstOrDefault(x => x.Service == ser);
			if(serOrderType != null)
				orderTypeClass.ListServiceOrderType.Remove(serOrderType);
			ytreeviewService.ItemsDataSource = listService;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			orderTypeClass.Name = entryNameOrder.Text;
			orderTypeClass.IsCalculateSalary = checkbuttonCalculationSalary.Active;
			orderTypeClass.PositionInTabs = "";

			if(checkbuttonInstallationSuburban.Active)
				orderTypeClass.PositionInTabs += checkbuttonInstallationSuburban.Label + " ";

			if(checkbuttonSuburbanTinting.Active) 
				orderTypeClass.PositionInTabs += checkbuttonSuburbanTinting.Label + " ";

			if(checkbuttonOrder.Active)
				orderTypeClass.PositionInTabs += checkbuttonOrder.Label + " ";

			if(checkbuttonTintedEntry.Active)
				orderTypeClass.PositionInTabs += checkbuttonTintedEntry.Label + " ";

			orderTypeClass.IsShowMainWidgets = checkbuttonMainParameters.Active;
			orderTypeClass.IsShowAdditionalWidgets = checkbuttonAdditionalParameters.Active;
			orderTypeClass.IsInstallType = checkbuttonInstall.Active;
			orderTypeClass.IsOtherType = checkbuttonOther.Active;

			UoW.Save(orderTypeClass);
			UoW.Commit();
		}


    }
}
