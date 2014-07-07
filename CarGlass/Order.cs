using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using Gtk;

namespace CarGlass
{
	public partial class Order : Gtk.Dialog
	{
		public enum OrderType {install, tinting, repair};
		public bool NewItem;
		private DateTime Date;
		private int Hour;

		private OrderType CurrentType;
		ListStore ServiceListStore;
		int Item_id = -1;

		private Dictionary<string, bool> GlassInDB;

		public Order(OrderType Type, DateTime date, int hour) : this (Type)
		{
			Date = date;
			Hour = hour;
		
			this.Title = String.Format(GetTitleFormat(CurrentType), "???", Date, Hour);
		}

		public Order(OrderType Type)
		{
			this.Build();
			CurrentType = Type;

			QSMain.CheckConnectionAlive();
			string sql = "SELECT name, id FROM status WHERE usedtypes LIKE '%" + CurrentType.ToString() + "%'";
			ComboWorks.ComboFillUniversal(comboStatus, sql, "{0}", null, 1, ComboWorks.ListMode.OnlyItems);
			ComboWorks.ComboFillReference(comboMark, "marks", ComboWorks.ListMode.OnlyItems);

			if(CurrentType == OrderType.install)
			{
				ComboWorks.ComboFillReference(comboManufacturer, "manufacturers", ComboWorks.ListMode.WithNo);
				ComboWorks.ComboFillReference(comboStock, "stocks", ComboWorks.ListMode.WithNo);
			}
			else
			{
				labelGlass.Visible = labelManufacturer.Visible = comboManufacturer.Visible = labelStock.Visible =
					comboStock.Visible = labelEurocode.Visible = entryEurocode.Visible = false;
			}

			//Загрузка списка стекл
			sql = "SELECT id, name FROM glass";
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

			//Загрузка списка услуг
			ServiceListStore = new Gtk.ListStore (typeof(int), 	// 0 - service id
				typeof(bool),	// 1 - Активно
				typeof(string),	// 2 - Наименование
				typeof(double),	// 3 - Цена
				typeof(long)	// 4 - order_pay id
			);

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
			cmd.Parameters.AddWithValue("@order_type", CurrentType.ToString());
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
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO orders (date, hour, type, car_model_id, car_year, phone, status_id, manufacturer_id, stock_id, eurocode, comment) " +
					"VALUES (@date, @hour, @type, @car_model_id, @car_year, @phone, @status_id, @manufacturer_id, @stock_id, @eurocode, @comment)";
			}
			else
			{
				sql = "UPDATE orders SET car_model_id = @car_model_id, car_year = @car_year, phone = @phone, status_id = @status_id, " +
					"manufacturer_id = @manufacturer_id, stock_id = @stock_id, eurocode = @eurocode, comment = @comment WHERE id = @id";
			}
			MainClass.StatusMessage("Запись заказа...");
			QSMain.CheckConnectionAlive();
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);

				cmd.Parameters.AddWithValue("@id", Item_id);
				cmd.Parameters.AddWithValue("@date", Date);
				cmd.Parameters.AddWithValue("@hour", Hour);
				cmd.Parameters.AddWithValue("@type", CurrentType.ToString());
				cmd.Parameters.AddWithValue("@car_model_id", ComboWorks.GetActiveId(comboModel));
				cmd.Parameters.AddWithValue("@car_year", DBWorks.ValueOrNull(comboYear.ActiveText != "", comboYear.ActiveText));
				cmd.Parameters.AddWithValue("@phone", DBWorks.ValueOrNull(entryPhone.Text != "" && entryPhone.Text != "+7" , entryPhone.Text));
				cmd.Parameters.AddWithValue("@status_id",  ComboWorks.GetActiveId(comboStatus));
				cmd.Parameters.AddWithValue("@manufacturer_id", ComboWorks.GetActiveIdOrNull(comboManufacturer));
				cmd.Parameters.AddWithValue("@stock_id", ComboWorks.GetActiveIdOrNull(comboStock));
				cmd.Parameters.AddWithValue("@eurocode", DBWorks.ValueOrNull(entryEurocode.Text != "", entryEurocode.Text));
				cmd.Parameters.AddWithValue("@comment", DBWorks.ValueOrNull(textviewComment.Buffer.Text != "", textviewComment.Buffer.Text));
				cmd.ExecuteNonQuery();

				if(NewItem)
					Item_id = (int) cmd.LastInsertedId;

				// Запись стекл
				foreach(KeyValuePair<string, CheckButton> pair in checksGlass.CheckButtons)
				{
					if(pair.Value.Active && !GlassInDB[pair.Key])
					{
						sql = "INSERT INTO order_glasses (order_id, glass_id) VALUES (@order_id, @glass_id)";

						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@order_id", Item_id);
						cmd.Parameters.AddWithValue("@glass_id", pair.Key);
						cmd.ExecuteNonQuery();
					}
					if(!pair.Value.Active && GlassInDB[pair.Key])
					{
						sql = "DELETE FROM order_glasses WHERE order_id = @order_id AND glass_id = @glass_id";

						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@order_id", Item_id);
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

						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@id", row[4]);
						cmd.Parameters.AddWithValue("@order_id", Item_id);
						cmd.Parameters.AddWithValue("@service_id", row[0]);
						cmd.Parameters.AddWithValue("@cost", row[3]);
						cmd.ExecuteNonQuery();
					}
					else if((long)row[4] > 0)
					{
						sql = "DELETE FROM order_pays WHERE id = @id";

						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@id", row[4]);
						cmd.ExecuteNonQuery();
					}
				}

				MainClass.StatusMessage("Ok");
				trans.Commit();
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				trans.Rollback();
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи заказа!");
				QSMain.ErrorMessage(this, ex);
			}

		}

		public void Fill(int id)
		{
			Item_id = id;
			NewItem = false;

			MainClass.StatusMessage(String.Format ("Запрос заказа №{0}...", id));
			string sql = "SELECT orders.*, models.name as model, models.mark_id FROM orders " +
				"LEFT JOIN models ON models.id = orders.car_model_id " +
				"WHERE orders.id = @id";
			QSMain.CheckConnectionAlive();
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				int mark_id, model_id;

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					rdr.Read();

					this.Title = String.Format(GetTitleFormat(CurrentType), rdr["id"].ToString(), rdr.GetDateTime("date"), rdr.GetInt32("hour"));
					Date = rdr.GetDateTime("date");

					ComboWorks.SetActiveItem(comboStatus, rdr.GetInt32("status_id"));
					model_id = rdr.GetInt32("car_model_id");
					mark_id = rdr.GetInt32("mark_id");

					comboYear.Entry.Text = rdr["car_year"].ToString();
					entryPhone.Text = DBWorks.GetString(rdr, "phone", "+7");

					if(CurrentType == OrderType.install)
					{
						ComboWorks.SetActiveItem(comboManufacturer, DBWorks.GetInt(rdr, "manufacturer_id", -1));
						ComboWorks.SetActiveItem(comboStock, DBWorks.GetInt(rdr, "stock_id", -1));
						entryEurocode.Text = DBWorks.GetString(rdr, "eurocode", "");
					}
					textviewComment.Buffer.Text = DBWorks.GetString(rdr, "comment", "");
				}
				ComboWorks.SetActiveItem(comboMark, mark_id);
				ComboWorks.SetActiveItem(comboModel, model_id);

				sql = "SELECT * FROM order_glasses WHERE order_id = @id";
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", id);
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
				cmd.Parameters.AddWithValue("@id", id);
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
				buttonPrint.Sensitive = CurrentType != OrderType.repair;
				buttonDelete.Sensitive = true;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о заказе!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Statusok = comboStatus.Active >= 0;
			bool Carok = ComboWorks.GetActiveId(comboModel) > 0;
			buttonOk.Sensitive = Statusok && Carok;
		}

		private string GetTitleFormat (OrderType type)
		{
			string[] str = new string[]
			{
				"Заказ установки №{0} на {1:D} в {2} часов",
				"Заказ тонировки №{0} на {1:D} в {2} часов",
				"Заказ ремонта №{0} на {1:D} в {2} часов"
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

			MenuItem1 = new MenuItem("Заказ наряд");
			MenuItem1.Activated += OnPopupPrintOrder;
			jBox.Add(MenuItem1);       
			MenuItem1 = new MenuItem("Товарный чек");
			MenuItem1.Activated += OnPopupPrintReceipt;
			jBox.Add(MenuItem1);       
			jBox.ShowAll();
			jBox.Popup();
		}

		protected void OnPopupPrintOrder(object sender, EventArgs Arg)
		{
			if(CurrentType == OrderType.tinting)
				QSProjectsLib.ViewReportExt.Run("order2", String.Format("id={0}", Item_id));
			else
				QSProjectsLib.ViewReportExt.Run("order", String.Format("id={0}", Item_id));
		}

		protected void OnPopupPrintReceipt(object sender, EventArgs Arg)
		{
			QSProjectsLib.ViewReportExt.Run("receipt", String.Format("id={0}&date={1:d}", Item_id, Date), true);
		}

		protected void OnButtonDeleteClicked(object sender, EventArgs e)
		{
			Delete winDelete = new Delete();
			if (winDelete.RunDeletion("orders", Item_id))
				Respond(ResponseType.Ok);
		}

		protected void OnComboMarkChanged(object sender, EventArgs e)
		{
			string sql = "SELECT name, id FROM models WHERE mark_id = @id ";
			MySqlParameter[] Param = { new MySqlParameter("@id", ComboWorks.GetActiveId(comboMark)) };
			ComboWorks.ComboFillUniversal(comboModel, sql, "{0}", Param, 1, ComboWorks.ListMode.OnlyItems);
		}

		protected void OnComboModelChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

	}
}

