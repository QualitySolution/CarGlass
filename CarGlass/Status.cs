using System;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using System.Collections.Generic;

namespace CarGlass
{
	public partial class Status : Gtk.Dialog
	{
		public bool NewItem;
		int Status_id;

		public Status()
		{
			this.Build();

			checklistTypes.AddCheckButton("install", "Установка");
			checklistTypes.AddCheckButton("tinting", "Тонировка");
			checklistTypes.AddCheckButton("repair", "Ремонт сколов");
		}

		public void Fill(int id)
		{
			Status_id = id;
			NewItem = false;

			MainClass.StatusMessage(String.Format ("Запрос статуса №{0}...", id));
			string sql = "SELECT status.* FROM status WHERE status.id = @id";
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

					//Читаем лист типов заказов
					string[] types = rdr["usedtypes"].ToString().Split(new char[] {','} );
					foreach(string ordertype in types)
					{
						if(checklistTypes.CheckButtons.ContainsKey(ordertype))
							checklistTypes.CheckButtons[ordertype].Active = true;
					}
				}
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о статусе!");
				QSMain.ErrorMessage(this,ex);
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
				sql = "INSERT INTO status (name, color, usedtypes) " +
					"VALUES (@name, @color, @usedtypes)";
			}
			else
			{
				sql = "UPDATE status SET name = @name, color = @color, usedtypes = @usedtypes WHERE id = @id";
			}
			MainClass.StatusMessage("Запись статуса...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Status_id);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(checkColor.Active)
				{
					Gdk.Color c = colorbuttonMarker.Color;
					string ColorStr = String.Format("#{0:X}{1:X}{2:X}", c.Red, c.Green, c.Blue);
					Console.WriteLine(ColorStr);
					cmd.Parameters.AddWithValue("@color", ColorStr);
				}
				else
					cmd.Parameters.AddWithValue("@color", DBNull.Value);
				string types = "";
				foreach(KeyValuePair<string, Gtk.CheckButton> Pair in checklistTypes.CheckButtons)
				{
					if(Pair.Value.Active)
						types += Pair.Key + ",";
				}
				cmd.Parameters.AddWithValue("@usedtypes", types.TrimEnd(','));

				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи статуса!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

	}
}