using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;

namespace CarGlass
{
	public partial class Service : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public bool NewItem;
		int Serviceid;

		public Service()
		{
			this.Build();
		}

		public void Fill(int id)
		{
			Serviceid = id;
			NewItem = false;

			MainClass.StatusMessage(String.Format ("Запрос услуги №{0}...", id));
			string sql = "SELECT services.* FROM services WHERE services.id = @id";
			QSMain.CheckConnectionAlive();
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				MySqlDataReader rdr = cmd.ExecuteReader();

				rdr.Read();

				labelID.Text = rdr["id"].ToString();
				entryName.Text = rdr["name"].ToString();
				comboType.Active = (int) Enum.Parse(typeof(Order.OrderType), rdr["order_type"].ToString());
				spinPrice.Value = DBWorks.GetDouble(rdr, "price", 0);

				rdr.Close();
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				MainClass.StatusMessage("Ошибка получения информации о услуге!");
				logger.Error(ex.ToString());
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool Typeok = comboType.Active >= 0;
			buttonOk.Sensitive = Nameok && Typeok;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO services (name, order_type, price) " +
					"VALUES (@name, @order_type, @price)";
			}
			else
			{
				sql = "UPDATE services SET name = @name, order_type = @order_type, price = @price WHERE id = @id";
			}
			MainClass.StatusMessage("Запись услуги...");
			QSMain.CheckConnectionAlive();
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);

				cmd.Parameters.AddWithValue("@id", Serviceid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@order_type", ((Order.OrderType) comboType.Active).ToString() );
				cmd.Parameters.AddWithValue("@price", DBWorks.ValueOrNull(spinPrice.Value > 0, spinPrice.Value));

				cmd.ExecuteNonQuery();
				Serviceid = Convert.ToInt32(cmd.LastInsertedId);

				if(NewItem)
				{
					sql = "UPDATE services SET ordinal = @id WHERE id = @id";
					cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
					cmd.Parameters.AddWithValue("@id", Serviceid);
					cmd.ExecuteNonQuery();
				}

				trans.Commit();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				trans.Rollback();
				MainClass.StatusMessage("Ошибка записи услуги!");
				logger.Error(ex.ToString());
				QSMain.ErrorMessage(this,ex);
			}
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

