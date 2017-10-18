﻿using System;
using System.Collections.Generic;
using CarGlass.Domain;
using Gamma.Utilities;
using MySql.Data.MySqlClient;
using NLog;
using QSProjectsLib;

namespace CarGlass
{
	public partial class Status : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public bool NewItem;
		int Status_id;

		public Status()
		{
			this.Build();

			foreach(OrderType type in Enum.GetValues(typeof(OrderType)))
				checklistTypes.AddCheckButton(
					type.ToString(), type.GetEnumTitle());	
		}

		public void Fill(int id)
		{
			Status_id = id;
			NewItem = false;

			logger.Info("Запрос статуса №{0}...", id);
			string sql = "SELECT status.* FROM status WHERE status.id = @id";
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

					//Читаем лист типов заказов
					string[] types = rdr["usedtypes"].ToString().Split(new char[] {','} );
					foreach(string ordertype in types)
					{
						if(checklistTypes.CheckButtons.ContainsKey(ordertype))
							checklistTypes.CheckButtons[ordertype].Active = true;
					}
				}
				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog("Ошибка получения информации о статусе!", logger, ex);
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
			logger.Info("Запись статуса...");
			QSMain.CheckConnectionAlive();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Status_id);
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
				string types = "";
				foreach(KeyValuePair<string, Gtk.CheckButton> Pair in checklistTypes.CheckButtons)
				{
					if(Pair.Value.Active)
						types += Pair.Key + ",";
				}
				cmd.Parameters.AddWithValue("@usedtypes", types.TrimEnd(','));

				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog("Ошибка записи статуса!", logger, ex);
			}
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

	}
}