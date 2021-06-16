using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CarGlass.Domain;
using CarGlass.Repository;
using Gamma.GtkWidgets;
using Gtk;
using MySql.Data.MySqlClient;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Project.Repositories;
using QSOrmProject;
using QSProjectsLib;

namespace CarGlass
{
	public partial class OrderDlg : FakeTDIEntityGtkDialogBase<WorkOrder>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		#region Внешние зависимости
		ILifetimeScope AutofacScope;
		IInteractiveMessage interactive;
		#endregion

		ListStore ServiceListStore = new Gtk.ListStore(
				typeof(int),    // 0 - service id
				typeof(bool),   // 1 - Активно
				typeof(string), // 2 - Наименование
				typeof(double), // 3 - Цена
				typeof(long)    // 4 - order_pay id
				,typeof(bool)	// первый исполнитель
				,typeof(bool)   // второй исполнитель
				,typeof(bool)	// третий исполнитель
			);
		IList<Employee> listPerformers = new List<Employee>();

		public OrderDlg(ushort pointNumber, ushort calendarNumber, OrderTypeClass Type, DateTime date, ushort hour)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<WorkOrder>();
			Entity.OrderTypeClass = Type;
			Entity.Date = date;
			Entity.Hour = hour;
			Entity.PointNumber = pointNumber;
			Entity.CalendarNumber = calendarNumber;
			Entity.CreatedBy = UserRepository.GetCurrentUser(UoW);

			ConfigureDlg();
		}

		public OrderDlg(WorkOrder order) : this(order.Id) { }

		public OrderDlg(int id)
		{
			if(QSMain.User.Permissions["worker"]) return;
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<WorkOrder>(id);

			ConfigureDlg();
			Fill();
		}

		void ConfigureDlg()
		{
			AutofacScope = MainClass.AppDIContainer.BeginLifetimeScope();
			interactive = AutofacScope.Resolve<IInteractiveMessage>();

			labelCreated.LabelProp = $"{Entity.CreatedDate} - {Entity.CreatedBy?.Name}";

			this.Title = String.Format(GetTitleFormat(Entity.OrderTypeClass), UoW.IsNew ? "???" : Entity.Id.ToString(), Entity.Date, Entity.Hour);

			comboStatus.ItemsList = OrderStateRepository.GetStates(UoW, Entity.OrderTypeClass);
			comboStatus.Binding.AddBinding(Entity, e => e.OrderState, w => w.SelectedItem).InitializeFromSource();

			comboMark.ItemsList = UoW.GetAll<CarBrand>();
			if (Entity.CarModel != null)
				comboMark.SelectedItem = Entity.CarModel.Brand;
			comboModel.Binding.AddBinding(Entity, e => e.CarModel, w => w.SelectedItem).InitializeFromSource();

			if(Entity.OrderTypeClass.IsShowAdditionalWidgets)
			{
				comboManufacturer.ShowSpecialStateNot = true;
				comboManufacturer.ItemsList = UoW.GetAll<Manufacturer>();

				comboStock.ShowSpecialStateNot = true;
				comboStock.ItemsList = UoW.GetAll<Warehouse>();
			}
			else
			{
				labelGlass.Visible = labelManufacturer.Visible = comboManufacturer.Visible = labelStock.Visible =
					comboStock.Visible = labelEurocode.Visible = entryEurocode.Visible = false;
			}

			comboYear.Model = new ListStore(typeof(string));
			comboYear.TextColumn = 0;

			for (int year = 1980; year <= DateTime.Today.Year; year++)
				comboYear.AppendText(year.ToString());
				
			comboYear.Binding.AddBinding(Entity, e => e.CarYearText, w => w.Entry.Text).InitializeFromSource();
			comboManufacturer.Binding.AddBinding(Entity, e => e.Manufacturer, w => w.SelectedItem).InitializeFromSource();
			comboStock.Binding.AddBinding(Entity, e => e.Stock, w => w.SelectedItem).InitializeFromSource();
			entryPhone.Binding.AddBinding(Entity, e => e.Phone, w => w.Text).InitializeFromSource();
			entryEurocode.Binding.AddBinding(Entity, e => e.Eurocode, w => w.Text).InitializeFromSource();

			textviewComment.Binding.AddBinding(Entity, e => e.Comment, w => w.Buffer.Text).InitializeFromSource();

			yradioInstallNone.BindValueWhenActivated = yradioTintingNone.BindValueWhenActivated
				= yradioArmoringNone.BindValueWhenActivated = yradioPastingNone.BindValueWhenActivated = Warranty.None;
			yradioInstall6Month.BindValueWhenActivated = yradioTinting6Month.BindValueWhenActivated
				= yradioArmoring6Month.BindValueWhenActivated = yradioPasting6Month.BindValueWhenActivated = Warranty.SixMonth;
			yradioInstall1Year.BindValueWhenActivated = yradioTinting1Year.BindValueWhenActivated
				= yradioArmoring1Year.BindValueWhenActivated = yradioPasting1Year.BindValueWhenActivated = Warranty.OneYear;
			yradioInstall2Year.BindValueWhenActivated = yradioTinting2Year.BindValueWhenActivated
				= yradioArmoring2Year.BindValueWhenActivated = yradioPasting2Year.BindValueWhenActivated = Warranty.TwoYear;
			yradioInstall3Year.BindValueWhenActivated = yradioTinting3Year.BindValueWhenActivated
				= yradioArmoring3Year.BindValueWhenActivated = yradioPasting3Year.BindValueWhenActivated = Warranty.ThreeYear;
			yradioInstallIndefinitely.BindValueWhenActivated = yradioTintingIndefinitely.BindValueWhenActivated
				= yradioArmoringIndefinitely.BindValueWhenActivated = yradioPastingIndefinitely.BindValueWhenActivated = Warranty.Indefinitely;
			yradioInstallNoWarranty.BindValueWhenActivated = yradioTintingNoWarranty.BindValueWhenActivated
				= yradioArmoringNoWarranty.BindValueWhenActivated = yradioPastingNoWarranty.BindValueWhenActivated = Warranty.NoWarranty;

			yradioInstallNone.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall6Month.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall1Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall2Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall3Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstallIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstallNoWarranty.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();

			yradioTintingNone.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting6Month.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting1Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting2Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting3Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTintingIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTintingNoWarranty.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();

			yradioArmoringNone.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring6Month.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring1Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring2Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring3Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoringIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoringNoWarranty.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();

			yradioPastingNone.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting6Month.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting1Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting2Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting3Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPastingIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPastingNoWarranty.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();

			CellRendererToggle CellPay = new CellRendererToggle();
			CellPay.Activatable = true;
			CellPay.Toggled += onCellPayToggled;

			Gtk.CellRendererSpin CellCost = new CellRendererSpin();
			CellCost.Editable = true;
			CellCost.Digits = 2;
			Adjustment adjCost = new Adjustment(0, 0, 100000000, 100, 1000, 0);
			CellCost.Adjustment = adjCost;
			CellCost.Edited += OnCostSpinEdited;

			treeviewCost.AppendColumn("", CellPay, "active", 1);
			treeviewCost.AppendColumn("Название", new CellRendererText(), "text", 2);
			treeviewCost.AppendColumn("Стоимость", CellCost, RenderPriceColumn);

			setInTablePerformers();

			((CellRendererToggle)treeviewCost.Columns[0].CellRenderers[0]).Activatable = true;

			treeviewCost.Model = ServiceListStore;
			treeviewCost.ShowAll();
			//fixme Изменить запрос
			var sql = "SELECT ser.id, ser.name, ser.price FROM services ser " +
				"JOIN service_order_type ordt on ser.id = ordt.id_service " +
				"JOIN order_type ord on ord.id = ordt.id_type_order " +
				"WHERE ord.name = @order_type ORDER BY ord.name";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@order_type", Entity.OrderTypeClass.Name.ToString());
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					ServiceListStore.AppendValues(rdr.GetInt32("id"),
						false,
						rdr.GetString("name"),
						DBWorks.GetDouble(rdr, "price", 0),
						(long)-1,
						false,false, false
					);
				}
			}

			ytreeOtherOrders.ColumnsConfig = ColumnsConfigFactory.Create<WorkOrder>()
				.AddColumn("Тип").AddTextRenderer(x => x.OrderTypeClass.Name) // x.OrderType.GetEnumTitle()
				.AddColumn("Состояние").AddTextRenderer(x => x.OrderState != null ? x.OrderState.Name : null)
					.AddSetter((c, x) => c.Background = x.OrderState != null ? x.OrderState.Color : null)
				.AddColumn("Дата").AddTextRenderer(x => x.Date.ToShortDateString())
				.AddColumn("Марка").AddTextRenderer(x => x.CarModel !=null ? x.CarModel.Brand.Name : null)
				.AddColumn("Модель").AddTextRenderer(x => x.CarModel != null ? x.CarModel.Name : null)
				.AddColumn("Еврокод").AddTextRenderer(x => x.Eurocode)
				.AddColumn("Производитель").AddTextRenderer(x => x.Manufacturer != null ? x.Manufacturer.Name : null)
				.AddColumn("Сумма").AddTextRenderer(x => x.Pays.Sum(p => p.Cost).ToString("C"))
				.AddColumn("Комментарий").AddTextRenderer(x => x.Comment)
				.AddColumn("Номер").AddTextRenderer(x => x.Id.ToString())
				.Finish();

			ytreeEuroCode.ColumnsConfig = ColumnsConfigFactory.Create<StoreItem>()
			.AddColumn("Еврокод").AddTextRenderer(x => x.EuroCode)
			.AddColumn("Производитель").AddTextRenderer(x => x.Manufacturer.Name)
			.AddColumn("Количество").AddNumericRenderer(x => x.Amount)
			.AddColumn("Цена").AddNumericRenderer(x=> x.Cost)
			.Finish();

			buttonPrint.Sensitive = !Entity.OrderTypeClass.IsOtherType;
			TestCanSave();
			SetEuroCode();

			Entity.PropertyChanged += Entity_PropertyChanged;
		}

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			TestCanSave();
        }

        private void setInTablePerformers()
		{
			listPerformers = getPerformers();
			List<CellRendererToggle> referActive = new List<CellRendererToggle>();
			if(listPerformers.Count() > 3)
			{
				interactive.ShowMessage(ImportanceLevel.Warning, "  Указано более трех исполнителей.\n Очистите график работ и проверьте,\n что в заказе не указаны лишние исполнители.");
				listPerformers.Clear();
			}

			int i = 5;
			foreach(var emp in listPerformers)
			{
				getCellActive(ref referActive);
				var column = treeviewCost.AppendColumn(emp.PersonNameWithInitials(), referActive[i - 5], "active", i);
				column.Alignment = 0.5f;
				column.Expand = true;
				i++;
			}
		}

		private void RenderPriceColumn(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double Price = (double)model.GetValue(iter, 3);
			(cell as Gtk.CellRendererSpin).Text = String.Format("{0:0.00}", Price);
		}

		void OnCostSpinEdited(object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString(out iter, args.Path))
				return;
			double Cost;
			if (double.TryParse(args.NewText, out Cost))
			{
				ServiceListStore.SetValue(iter, 3, Cost);
				CalculateTotal();
			}
		}

		void onCellPayToggled(object o, ToggledArgs args)
		{
			TreeIter iter;

			if (ServiceListStore.GetIter(out iter, new TreePath(args.Path)))
			{
				bool old = (bool)ServiceListStore.GetValue(iter, 1);
				ServiceListStore.SetValue(iter, 1, !old);
				if(!old == false)
				{
					ServiceListStore.SetValue(iter, 5, false);
					ServiceListStore.SetValue(iter, 6, false);
					ServiceListStore.SetValue(iter, 7, false);
				}
				if (!old && listPerformers.Count == 1)
					ServiceListStore.SetValue(iter, 5, true);
			}
			CalculateTotal();
		}

		void onCellActiveToggled(object o, ToggledArgs args)
		{
			setActiveToggled(o, args, 5);
		}
		void onCellActiveToggled2(object o, ToggledArgs args)
		{
			setActiveToggled(o, args, 6);
		}
		void onCellActiveToggled3(object o, ToggledArgs args)
		{
			setActiveToggled(o, args, 7);
		}

		void setActiveToggled(object o, ToggledArgs args, int column)
		{
			TreeIter iter;

			if(ServiceListStore.GetIter(out iter, new TreePath(args.Path)))
			{
				if(ServiceListStore.GetValue(iter, column).ToString() != null)
				{
					bool old = (bool)ServiceListStore.GetValue(iter, column);
					if ((bool)ServiceListStore.GetValue(iter, 1))
						ServiceListStore.SetValue(iter, column, !old);
				}
			}
		}

		private void CalculateTotal()
		{
			double Total = 0;
			foreach (object[] row in ServiceListStore)
			{
				if ((bool)row[1])
					Total += (double)row[3];
			}
			labelSum.LabelProp = String.Format("<span foreground=\"red\"><b>Итого: {0:C}</b></span>", Total);
			labelSum.QueueResize();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			PrepareSave();
			Save();
		}

		void PrepareSave()
		{
			foreach (object[] row in ServiceListStore)
			{
				var pay = Entity.Pays.FirstOrDefault(x => x.Service.Id == (int)row[0]);

				if ((bool)row[1])
				{
					if (pay == null)
					{
						pay = new WorkOrderPay(Entity, UoW.GetById<Service>((int)row[0]));
						Entity.Pays.Add(pay);
					}

					pay = getPerformers(pay, row);

					pay.Cost = Convert.ToDecimal(row[3]);
				}
				else if (pay != null)
				{
					Entity.Pays.Remove(pay);
				}

			}
		}

		private WorkOrderPay getPerformers(WorkOrderPay pay, object[] row)
		{
			if(listPerformers.Count > 0)
			{
				pay = getEmployeeServiceWorkforSave(row, pay, 5);
			}

			if(listPerformers.Count > 1)
			{
				pay = getEmployeeServiceWorkforSave(row, pay, 6);
			}

			if(listPerformers.Count > 2)
			{
				pay = getEmployeeServiceWorkforSave(row, pay, 7);
			}

			return pay;
		}

		private WorkOrderPay getEmployeeServiceWorkforSave(object[] row, WorkOrderPay pay, int column)
		{
			var employeeService = pay.EmployeeServiceWork.FirstOrDefault(x => x.Employee.Id == listPerformers[column - 5].Id);
			if((bool)row[column])
			{
				if(employeeService == null)
					pay.EmployeeServiceWork.Add(new EmployeeServiceWork(listPerformers[column - 5], pay, Entity.Date));
			}
			else if(employeeService != null)
			{
				pay.EmployeeServiceWork.Remove(employeeService);
			}
			return pay;
		}


		public override bool Save()
		{
			UoW.Save();

			logger.Info("Ok");
			Respond(ResponseType.Ok);

			return true;
		}

		private void Fill()
		{
			var sql = "SELECT * FROM order_pays op" +
				" LEFT JOIN employee_service_work  esw on op.id = esw.id_order_pay" +
				" WHERE order_id = @id";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Entity.Id);
			using (MySqlDataReader rdr = cmd.ExecuteReader())
			{
				TreeIter iter;
				while (rdr.Read())
				{
					if (ListStoreWorks.SearchListStore(ServiceListStore, rdr.GetInt32("service_id"), 0, out iter))
					{
						ServiceListStore.SetValue(iter, 1, true);
						ServiceListStore.SetValue(iter, 3, rdr.GetDouble("cost"));
						ServiceListStore.SetValue(iter, 4, (object)rdr.GetInt64("id"));
						if (rdr["id_employee"].ToString().Length > 0)
						{
							int i = 5;
							foreach(var emp in listPerformers)
							{
								if(emp.Id == rdr.GetInt32("id_employee"))
									break;
								i++;
							}
							ServiceListStore.SetValue(iter, i, true);
						}
					}
					else
					{
						ServiceListStore.AppendValues(rdr.GetInt32("service_id"),
							true,
							"исключенная услуга",
							rdr.GetDouble("cost"),
							(object)rdr.GetInt64("id")
						);
					}
				}
			}
			CalculateTotal();
			buttonDelete.Sensitive = true;
			logger.Info("Ok");

			TestCanSave();
		}

		protected void TestCanSave()
		{
			bool Statusok = Entity.OrderState != null;
			bool Carok = Entity.CarModel != null;
			bool NumberOk = Entity.Phone != null && Entity.Phone.Length == 16;
			bool YearOk = (Entity.CarYear == null) || (Entity.CarYear != null && Entity.CarYear > 1979 && Entity.CarYear <= DateTime.Now.Year);
			bool ModelOk = Entity.CarModel != null;
			buttonOk.Sensitive = (Statusok && Carok && NumberOk && YearOk && ModelOk) || Entity.OrderTypeClass.IsOtherType;
		}

		private string GetTitleFormat(OrderTypeClass type)
		{
			string str = "Тип заказа \'" + type.Name + "\' №{0} на {1:D} в {2} часов";
			return str;
		}

		private IList<Employee> getPerformers()
		{
			IList<Employee> listEmp = new List<Employee>();
			var sql = "SELECT DISTINCT emp.id, emp.first_name, emp.last_name, emp.patronymic FROM shedule_works sw " +
				" LEFT JOIN shedule_employee_works eshw on sw.id = eshw.id_shedule_works" +
				" LEFT JOIN employees emp on  emp.id = eshw.id_employee WHERE sw.date_work = @date and " +
				" sw.point_number = @point_number and calendar_number = @calendar_number" +
				" UNION select emp.id, emp.first_name, emp.last_name, emp.patronymic " +
				" FROM employee_service_work esw" +
				" LEFT JOIN employees emp on esw.id_employee = emp.id " +
				" lEFT JOIN order_pays op on esw.id_order_pay = op.id" +
				" WHERE op.order_id = @id_order";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@date", Entity.Date);
			cmd.Parameters.AddWithValue("@point_number", Entity.PointNumber);
			cmd.Parameters.AddWithValue("@calendar_number", Entity.CalendarNumber);
			cmd.Parameters.AddWithValue("@id_order", Entity.Id);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					listEmp.Add(new Employee(int.Parse(rdr.GetString("id")), rdr.GetString("first_name"), rdr.GetString("last_name")
					, rdr.GetString("patronymic")));
				 }
			}
			return listEmp;
		}

		protected void getCellActive(ref List<CellRendererToggle> referActive)
		{
			CellRendererToggle CellActive = new CellRendererToggle();
			CellActive.Activatable = true;
			
			switch(referActive.Count)
			{
				case 0:
					CellActive.Toggled += onCellActiveToggled;
					referActive.Add(CellActive);
					break;
				case 1:
					CellActive.Toggled += onCellActiveToggled2;
					referActive.Add(CellActive);
					break;
				case 2:
					CellActive.Toggled += onCellActiveToggled3;
					referActive.Add(CellActive);
					break;

			}
		}

		protected void OnButtonPrintClicked(object sender, EventArgs e)
		{
			Gtk.Menu jBox = new Gtk.Menu();
			MenuItem MenuItem1;

			MenuItem1 = new MenuItem("Заказ");
			MenuItem1.Activated += OnPopupPrintOrder;
			jBox.Add(MenuItem1);

			jBox.Add(new SeparatorMenuItem());
			MenuItem1 = new MenuItem("Товарный чек");
			MenuItem1.Activated += OnPopupPrintReceipt;
			jBox.Add(MenuItem1);
			jBox.ShowAll();
			jBox.Popup();
		}

		protected void OnPopupPrintOrder(object sender, EventArgs Arg)
		{
			PrepareSave();
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint(typeof(WorkOrder), "заказ-наряда"))
				Save();
			QSProjectsLib.ViewReportExt.Run("order", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintReceipt(object sender, EventArgs Arg)
		{
			PrepareSave();
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint(typeof(WorkOrder), "товарного чека"))
				Save();
			QSProjectsLib.ViewReportExt.Run("receipt", String.Format("id={0}&date={1:d}", Entity.Id, Entity.Date), true);
		}

		protected void OnButtonDeleteClicked(object sender, EventArgs e)
		{
			Delete winDelete = new Delete();
			if (winDelete.RunDeletion("orders", Entity.Id))
				Respond(ResponseType.Ok);
		}

		protected void OnComboMarkChanged(object sender, EventArgs e)
		{
			if (comboMark.SelectedItem == null)
			{
				comboModel.ItemsList = null;
				Entity.CarModel = null;
				return;
			}
			comboModel.ItemsList = CarModelRepository.GetCarModels(UoW, comboMark.SelectedItem as CarBrand);
			if(Entity.CarModel != null)
				comboModel.SelectedItem = Entity.CarModel;
		}

		protected void OnEntryPhoneChanged(object sender, EventArgs e)
		{
			IList<WorkOrder> list = null;
			if(entryPhone.Text.Length == 16)
			{
				list = WorkOrderRepository.GetOrdersByPhone(UoW, entryPhone.Text, Entity.Id);
				TestCanSave();
			}

			ytreeOtherOrders.ItemsDataSource = list;
			GtkScrolledWindowOtherOrders.Visible = list != null && list.Count > 0;

		}

		protected void OnYtreeOtherOrdersRowActivated(object o, RowActivatedArgs args)
		{
			var order = ytreeOtherOrders.GetSelectedObject<WorkOrder>();
			OpenTab<OrderDlg, WorkOrder>(order);
		}

		protected void OnEntryEurocodeChanged(object sender, EventArgs e)
		{
			SetEuroCode();
		}

		protected void OnYtreeEuroCodeRowActivated(object o, RowActivatedArgs args)
		{
			var eurocode = ytreeEuroCode.GetSelectedObject<StoreItem>();
			entryEurocode.Text = eurocode.EuroCode;
		}

		protected void SetEuroCode()
		{
			var stock = new StoreItemsRepository();
			IList<StoreItem> list = null;
			if(entryEurocode.Text.Length > 0 && entryEurocode.Text.Length < 5)
				list =  stock.GetSomeEurocodes(UoW, entryEurocode.Text);
			if (entryEurocode.Text.Length > 4)
				list = stock.GetAllEurocodes(UoW).Where(x=> x.EuroCode.Length > 5 && x.EuroCode.Substring(0, 5) == entryEurocode.Text.Substring(0, 5)).ToList();

			ytreeEuroCode.ItemsDataSource = list;
			GtkScrolledWindow2.Visible = ytreeEuroCode.Visible = list != null && list.Count > 0;
		}

		public override void Destroy()
		{
			base.Destroy();
			AutofacScope?.DisposeAsync();
		}
	}
}

