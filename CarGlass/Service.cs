using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace CarGlass
{
	public partial class Service : Gtk.Dialog
	{
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
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о услуге!");
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
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Serviceid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@order_type", ((Order.OrderType) comboType.Active).ToString() );
				cmd.Parameters.AddWithValue("@price", DBWorks.ValueOrNull(spinPrice.Value > 0, spinPrice.Value));

				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи услуги!");
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

