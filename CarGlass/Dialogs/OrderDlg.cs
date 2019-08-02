using System;
using System.Collections.Generic;
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

		private Dictionary<string, bool> GlassInDB;

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

			//Загрузка списка стекл
			var sql = "SELECT id, name FROM glass";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			GlassInDB = new Dictionary<string, bool>();
			using (MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					checksGlass.AddCheckButton(rdr["id"].ToString(), rdr["name"].ToString());
					GlassInDB.Add(rdr["id"].ToString(), false);
				}
			}

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

			sql = "SELECT id, name, price FROM services WHERE order_type = @order_type ORDER BY ordinal";
			cmd = new MySqlCommand(sql, QSMain.connectionDB);
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
				
			// Запись стекл
			foreach(KeyValuePair<string, CheckButton> pair in checksGlass.CheckButtons)
			{
				if(pair.Value.Active && !GlassInDB[pair.Key])
				{
					sql = "INSERT INTO order_glasses (order_id, glass_id) VALUES (@order_id, @glass_id)";

					var cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@order_id", Entity.Id);
					cmd.Parameters.AddWithValue("@glass_id", pair.Key);
					cmd.ExecuteNonQuery();
				}
				if(!pair.Value.Active && GlassInDB[pair.Key])
				{
					sql = "DELETE FROM order_glasses WHERE order_id = @order_id AND glass_id = @glass_id";

					var cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@order_id", Entity.Id);
					cmd.Parameters.AddWithValue("@glass_id", pair.Key);
					cmd.ExecuteNonQuery();
				}
			}

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
			var sql = "SELECT * FROM order_glasses WHERE order_id = @id";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Entity.Id);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					checksGlass.CheckButtons[rdr["glass_id"].ToString()].Active = true;
					GlassInDB[rdr["glass_id"].ToString()] = true;
				}
			}

			sql = "SELECT * FROM order_pays WHERE order_id = @id";
			cmd = new MySqlCommand(sql, QSMain.connectionDB);
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
			buttonPrint.Sensitive = Entity.OrderType != OrderType.repair;
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

			MenuItem1 = new MenuItem("Установка");
			MenuItem1.Activated += OnPopupPrintOrderInstall;
			jBox.Add(MenuItem1);
			MenuItem1 = new MenuItem("Тонировка");
			MenuItem1.Activated += OnPopupPrintOrderTinting;
			jBox.Add(MenuItem1);
			MenuItem1 = new MenuItem("Бронировка");
			MenuItem1.Activated += OnPopupPrintOrderArmoring;
			jBox.Add(MenuItem1);
			MenuItem1 = new MenuItem("Полировка");
			MenuItem1.Activated += OnPopupPrintOrderPolishing;
			jBox.Add(MenuItem1);

			jBox.Add(new SeparatorMenuItem());
			MenuItem1 = new MenuItem("Товарный чек");
			MenuItem1.Activated += OnPopupPrintReceipt;
			jBox.Add(MenuItem1);       
			jBox.ShowAll();
			jBox.Popup();
		}

		protected void OnPopupPrintOrderInstall(object sender, EventArgs Arg)
		{
			QSProjectsLib.ViewReportExt.Run("order", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintOrderTinting(object sender, EventArgs Arg)
		{
			QSProjectsLib.ViewReportExt.Run("order2", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintOrderArmoring(object sender, EventArgs Arg)
		{
			QSProjectsLib.ViewReportExt.Run("order3", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintOrderPolishing(object sender, EventArgs Arg)
		{
			QSProjectsLib.ViewReportExt.Run("order4", String.Format("id={0}", Entity.Id));
		}

		protected void OnPopupPrintReceipt(object sender, EventArgs Arg)
		{
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

