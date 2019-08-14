using System;
using CarGlass.Domain;
using CarGlass.Repository;
using Gtk;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using QS.Project.Repositories;
using QSOrmProject;
using QSProjectsLib;

namespace CarGlass
{
	public partial class OrderDlg : FakeTDIEntityGtkDialogBase<WorkOrder>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		ListStore ServiceListStore = new Gtk.ListStore(
				typeof(int),    // 0 - service id
				typeof(bool),   // 1 - Активно
				typeof(string), // 2 - Наименование
				typeof(double), // 3 - Цена
				typeof(long)    // 4 - order_pay id
			);

		public OrderDlg(ushort pointNumber, ushort calendarNumber, OrderType Type, DateTime date, ushort hour)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<WorkOrder>();
			Entity.OrderType = Type;
			Entity.Date = date;
			Entity.Hour = hour;
			Entity.PointNumber = pointNumber;
			Entity.CalendarNumber = calendarNumber;
			Entity.CreatedBy = UserRepository.GetCurrentUser(UoW);

			ConfigureDlg();
		}

		public OrderDlg(int id)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<WorkOrder>(id);

			ConfigureDlg();
			Fill();
		}

		void ConfigureDlg()
		{
			labelCreated.LabelProp = $"{Entity.CreatedDate} - {Entity.CreatedBy?.Name}";
			this.Title = String.Format(GetTitleFormat(Entity.OrderType), UoW.IsNew ? "???" : Entity.Id.ToString(), Entity.Date, Entity.Hour);

			comboStatus.ItemsList = OrderStateRepository.GetStates(UoW, Entity.OrderType);
			comboStatus.Binding.AddBinding(Entity, e => e.OrderState, w => w.SelectedItem).InitializeFromSource();

			comboMark.ItemsList = UoW.GetAll<CarBrand>();
			if (Entity.CarModel != null)
				comboMark.SelectedItem = Entity.CarModel.Brand;
			comboModel.Binding.AddBinding(Entity, e => e.CarModel, w => w.SelectedItem).InitializeFromSource();

			if(Entity.OrderType == OrderType.install)
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
				= yradioPolishingNone.BindValueWhenActivated = yradioArmoringNone.BindValueWhenActivated 
				= yradioPastingNone.BindValueWhenActivated = Warranty.None;
			yradioInstall1Year.BindValueWhenActivated = yradioTinting1Year.BindValueWhenActivated
				= yradioPolishing1Year.BindValueWhenActivated = yradioArmoring1Year.BindValueWhenActivated 
				= yradioPasting1Year.BindValueWhenActivated = Warranty.OneYear;
			yradioInstall2Year.BindValueWhenActivated = yradioTinting2Year.BindValueWhenActivated
				= yradioPolishing2Year.BindValueWhenActivated = yradioArmoring2Year.BindValueWhenActivated 
				= yradioPasting2Year.BindValueWhenActivated = Warranty.TwoYear;
			yradioInstall3Year.BindValueWhenActivated = yradioTinting3Year.BindValueWhenActivated 
				= yradioPolishing3Year.BindValueWhenActivated = yradioArmoring3Year.BindValueWhenActivated 
				= yradioPasting3Year.BindValueWhenActivated = Warranty.ThreeYear;
			yradioInstallIndefinitely.BindValueWhenActivated = yradioTintingIndefinitely.BindValueWhenActivated
				= yradioPolishingIndefinitely.BindValueWhenActivated = yradioArmoringIndefinitely.BindValueWhenActivated 
				= yradioPastingIndefinitely.BindValueWhenActivated = Warranty.Indefinitely;

			yradioInstallNone.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall1Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall2Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstall3Year.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();
			yradioInstallIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyInstall, w => w.BindedValue).InitializeFromSource();

			yradioTintingNone.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting1Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting2Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTinting3Year.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();
			yradioTintingIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyTinting, w => w.BindedValue).InitializeFromSource();

			yradioPolishingNone.Binding.AddBinding(Entity, e => e.WarrantyPolishing, w => w.BindedValue).InitializeFromSource();
			yradioPolishing1Year.Binding.AddBinding(Entity, e => e.WarrantyPolishing, w => w.BindedValue).InitializeFromSource();
			yradioPolishing2Year.Binding.AddBinding(Entity, e => e.WarrantyPolishing, w => w.BindedValue).InitializeFromSource();
			yradioPolishing3Year.Binding.AddBinding(Entity, e => e.WarrantyPolishing, w => w.BindedValue).InitializeFromSource();
			yradioPolishingIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyPolishing, w => w.BindedValue).InitializeFromSource();

			yradioArmoringNone.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring1Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring2Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoring3Year.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();
			yradioArmoringIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyArmoring, w => w.BindedValue).InitializeFromSource();

			yradioPastingNone.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting1Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting2Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPasting3Year.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();
			yradioPastingIndefinitely.Binding.AddBinding(Entity, e => e.WarrantyPasting, w => w.BindedValue).InitializeFromSource();

			CellRendererToggle CellPay = new CellRendererToggle();
			CellPay.Activatable = true;
			CellPay.Toggled += onCellPayToggled;

			Gtk.CellRendererSpin CellCost = new CellRendererSpin();
			CellCost.Editable = true;
			CellCost.Digits = 2;
			Adjustment adjCost = new Adjustment(0,0,100000000,100,1000,0);
			CellCost.Adjustment = adjCost;
			CellCost.Edited += OnCostSpinEdited;

			treeviewCost.AppendColumn ("", CellPay, "active", 1);
			treeviewCost.AppendColumn ("Название", new CellRendererText(), "text", 2);
			treeviewCost.AppendColumn ("Стоимость", CellCost, RenderPriceColumn);

			((CellRendererToggle)treeviewCost.Columns[0].CellRenderers[0]).Activatable = true;

			treeviewCost.Model = ServiceListStore;
			treeviewCost.ShowAll();

			var sql = "SELECT id, name, price FROM services WHERE order_type = @order_type ORDER BY ordinal";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@order_type", Entity.OrderType.ToString());
			using (MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					ServiceListStore.AppendValues(rdr.GetInt32("id"),
						false,
						rdr.GetString("name"),
						DBWorks.GetDouble(rdr, "price", 0),
						(long)-1
					);
				}
			}
			buttonPrint.Sensitive = Entity.OrderType != OrderType.repair && Entity.OrderType != OrderType.other;
			TestCanSave();
		}

		private void RenderPriceColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double Price = (double) model.GetValue (iter, 3);
			(cell as Gtk.CellRendererSpin).Text = String.Format("{0:0.00}", Price);
		}

		void OnCostSpinEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			double Cost;
			if (double.TryParse (args.NewText, out Cost)) 
			{
				ServiceListStore.SetValue (iter, 3, Cost);
				CalculateTotal ();
			}
		}

		void onCellPayToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;

			if (ServiceListStore.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) ServiceListStore.GetValue(iter,1);
				ServiceListStore.SetValue(iter, 1, !old);
			}
			CalculateTotal ();
		}

		private void CalculateTotal()
		{
			double Total = 0;
			foreach (object[] row in ServiceListStore)
			{
				if((bool)row[1])
					Total += (double) row[3];
			}
			labelSum.LabelProp = String.Format ("<span foreground=\"red\"><b>Итого: {0:C}</b></span>", Total);
			labelSum.QueueResize();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			Save();
		}

		public override bool Save()
		{
			UoW.Save();

			string sql;
			//Запись стоимости
			foreach(object[] row in ServiceListStore)
			{
				if((bool)row[1])
				{
					if((long) row[4] > 0)
						sql = "UPDATE order_pays SET order_id = @order_id, service_id = @service_id, cost = @cost WHERE id = @id";
					else
						sql = "INSERT INTO order_pays (order_id, service_id, cost) VALUES (@order_id, @service_id, @cost)";

					var cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", row[4]);
					cmd.Parameters.AddWithValue("@order_id", Entity.Id);
					cmd.Parameters.AddWithValue("@service_id", row[0]);
					cmd.Parameters.AddWithValue("@cost", row[3]);
					cmd.ExecuteNonQuery();
				}
				else if((long)row[4] > 0)
				{
					sql = "DELETE FROM order_pays WHERE id = @id";

					var cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", row[4]);
					cmd.ExecuteNonQuery();
				}
			}

			logger.Info("Ok");
			Respond (ResponseType.Ok);
			return true;
		}

		private void Fill()
		{
			var sql = "SELECT * FROM order_pays WHERE order_id = @id";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Entity.Id);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				TreeIter iter;
				while(rdr.Read())
				{
					if(ListStoreWorks.SearchListStore(ServiceListStore, rdr.GetInt32("service_id"), 0, out iter))
					{
						ServiceListStore.SetValue(iter, 1, true);
						ServiceListStore.SetValue(iter, 3, rdr.GetDouble("cost"));
						ServiceListStore.SetValue(iter, 4, (object)rdr.GetInt64("id"));
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

		protected	void TestCanSave ()
		{
			bool Statusok = Entity.OrderState != null;
			bool Carok = Entity.CarModel != null;
			buttonOk.Sensitive = (Statusok && Carok) || Entity.OrderType == OrderType.other;
		}

		private string GetTitleFormat (OrderType type)
		{
			string[] str = new string[]
			{
				"Заказ установки №{0} на {1:D} в {2} часов",
				"Заказ тонировки №{0} на {1:D} в {2} часов",
				"Заказ ремонта №{0} на {1:D} в {2} часов",
				"Заказ полировки №{0} на {1:D} в {2} часов",
				"Заказ бронировки №{0} на {1:D} в {2} часов",
				"Прочее №{0} на {1:D} в {2} часов",
			};
			return str[(int)type];
		}

		protected void OnComboStatusChanged(object sender, EventArgs e)
		{
			TestCanSave();
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
			if (UoWGeneric.HasChanges && CommonDialogs.SaveBeforePrint(typeof(WorkOrder), "заказ-наряда"))
				Save();
			QSProjectsLib.ViewReportExt.Run("order", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintReceipt(object sender, EventArgs Arg)
		{
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
				return;
			}
			comboModel.ItemsList = CarModelRepository.GetCarModels(UoW, comboMark.SelectedItem as CarBrand);
		}

		protected void OnComboModelChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}
	}
}

