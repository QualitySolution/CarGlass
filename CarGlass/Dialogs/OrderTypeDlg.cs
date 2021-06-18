using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gtk;
using NLog;
using QS.DomainModel.UoW;
using QSProjectsLib;
namespace CarGlass.Dialogs
{
	public partial class OrderTypeDlg : Gtk.Dialog
	{

		private static Logger logger = LogManager.GetCurrentClassLogger();

		public bool NewItem;
		int ItemId;
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		List<Service> listService = new List<Service>();
		OrderTypeClass Entity = new OrderTypeClass();

		public OrderTypeDlg()
		{
			this.Build();
			Configure();
		}

		public OrderTypeDlg(int id)
		{
			this.Build();
			Fill(id);
			Configure();
		}

		private void Configure()
		{
			entryNameAccusative.Binding.AddBinding(Entity, e => e.NameAccusative, w => w.Text).InitializeFromSource();
		}

		private void Fill(int id)
		{
			ItemId = id;
			NewItem = false;

			logger.Info("Запрос типа заказов №{0}...", id);

			Entity = UoW.GetById<OrderTypeClass>(ItemId);
			var l = UoW.GetAll<ServiceOrderType>()
				.Where(x => x.OrderTypeClass.Id == ItemId)
				.ToList();
			foreach(var ser in l)
				listService.Add(UoW.Session.QueryOver<Service>().List().First(x=> x == ser.Service));
			entryNumberOrder.Text = Entity.Id.ToString();
			entryNameOrder.Text = Entity.Name;

			checkbuttonCalculationSalary.Active = Entity.IsCalculateSalary;
			if(Entity.PositionInTabs == null)
				Entity.PositionInTabs = "";

			checkbuttonInstallationSuburban.Active = Entity.PositionInTabs.Contains(checkbuttonInstallationSuburban.Label);
			checkbuttonSuburbanTinting.Active = Entity.PositionInTabs.Contains(checkbuttonSuburbanTinting.Label);
			checkbuttonOrder.Active = Entity.PositionInTabs.Contains(checkbuttonOrder.Label);
			checkbuttonTintedEntry.Active = Entity.PositionInTabs.Contains(checkbuttonTintedEntry.Label);
			checkbuttonInstall.Active = Entity.IsInstallType;
			checkbuttonOther.Active = Entity.IsOtherType;

			checkbuttonMainParameters.Active = Entity.IsShowMainWidgets;
			checkbuttonAdditionalParameters.Active = Entity.IsShowAdditionalWidgets;

			logger.Info("Ok");
			this.Title = $"Редактирование типа заказа: \"{Entity.Name}\" ";
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
				if(Entity.ListServiceOrderType.FirstOrDefault(x => x.Service == ser) == null)
					Entity.ListServiceOrderType.Add(new ServiceOrderType(ser, this.Entity));
			}
			createTable();
			ServiceSelect.Destroy();

		}

		protected void OnButtonDeleteServiceClicked(object sender, EventArgs e)
		{
			var ser = ytreeviewService.GetSelectedObject<Service>();
			listService.Remove(ser);
			var serOrderType = Entity.ListServiceOrderType.FirstOrDefault(x => x.Service == ser);
			if(serOrderType != null)
				Entity.ListServiceOrderType.Remove(serOrderType);
			ytreeviewService.ItemsDataSource = listService;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			Entity.Name = entryNameOrder.Text;
			Entity.IsCalculateSalary = checkbuttonCalculationSalary.Active;
			Entity.PositionInTabs = "";

			if(checkbuttonInstallationSuburban.Active)
				Entity.PositionInTabs += checkbuttonInstallationSuburban.Label + " ";

			if(checkbuttonSuburbanTinting.Active) 
				Entity.PositionInTabs += checkbuttonSuburbanTinting.Label + " ";

			if(checkbuttonOrder.Active)
				Entity.PositionInTabs += checkbuttonOrder.Label + " ";

			if(checkbuttonTintedEntry.Active)
				Entity.PositionInTabs += checkbuttonTintedEntry.Label + " ";

			Entity.IsShowMainWidgets = checkbuttonMainParameters.Active;
			Entity.IsShowAdditionalWidgets = checkbuttonAdditionalParameters.Active;
			Entity.IsInstallType = checkbuttonInstall.Active;
			Entity.IsOtherType = checkbuttonOther.Active;

			UoW.Save(Entity);
			UoW.Commit();
		}

		public override void Destroy()
		{
			UoW.Dispose();
			base.Destroy();
		}
	}
}
