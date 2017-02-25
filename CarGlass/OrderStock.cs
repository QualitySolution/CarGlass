using System;
using NLog;
using QSProjectsLib;
using MySql.Data.MySqlClient;

namespace CarGlass
{
	public partial class OrderStock : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public bool NewItem;
		int ItemId;

		public OrderStock()
		{
			this.Build();
		}

		public void Fill(int id)
		{
			ItemId = id;
			NewItem = false;

			logger.Info("Запрос склада №{0}...", id);
			string sql = "SELECT stocks.* FROM stocks WHERE stocks.id = @id";
			QSMain.CheckConnectionAlive();
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{

					rdr.Read();

					labelId.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
					checkColor.Active = rdr["color"] != DBNull.Value;
					if(rdr["color"] != DBNull.Value)
					{
						Gdk.Color TempColor = new Gdk.Color();
						Gdk.Color.Parse(rdr.GetString ("color"), ref TempColor);
						colorbuttonMarker.Color = TempColor;
					}
				}
				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog("Ошибка получения информации о склада!", logger, ex);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			buttonOk.Sensitive = Nameok;
		}

		protected void OnCheckColorClicked(object sender, EventArgs e)
		{
			colorbuttonMarker.Sensitive = checkColor.Active;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO stocks (name, color) " +
					"VALUES (@name, @color)";
			}
			else
			{
				sql = "UPDATE stocks SET name = @name, color = @color WHERE id = @id";
			}
			logger.Info("Запись склада...");
			QSMain.CheckConnectionAlive();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", ItemId);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(checkColor.Active)
				{
					Gdk.Color c = colorbuttonMarker.Color;
					string ColorStr = String.Format("#{0:x4}{1:x4}{2:x4}", c.Red, c.Green, c.Blue);
					logger.Debug(ColorStr);
					cmd.Parameters.AddWithValue("@color", ColorStr);
				}
				else
					cmd.Parameters.AddWithValue("@color", DBNull.Value);

				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog("Ошибка записи склада!", logger, ex);
			}
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

	}
}

