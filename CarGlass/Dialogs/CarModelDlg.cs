using System;
using CarGlass.Domain;
using Gtk;
using MySqlConnector;
using QSOrmProject;
using QSProjectsLib;

namespace CarGlass
{
	public partial class CarModelDlg : FakeTDIEntityGtkDialogBase<CarModel>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public bool NewItem;
		int Itemid;
		int Mark_id = -1;

		public CarModelDlg()
		{
			this.Build();
		}

		public CarModelDlg(CarModel obj) : this(obj.Id) { }

		public CarModelDlg(int id) : this()
		{
			Fill(id);
		}

		public void Fill(int id)
		{
			Itemid = id;
			NewItem = false;

			logger.Info("Запрос модели №{0}...", id);
			string sql = "SELECT models.*, marks.name as mark FROM models " +
				"LEFT JOIN marks ON marks.id = models.mark_id WHERE models.id = @id";
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

					entryMark.Text = rdr["mark"].ToString();
					Mark_id = DBWorks.GetInt(rdr, "mark_id", -1);

					logger.Info("Ok");
				}
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog("Ошибка получения информации о модели!", logger, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool MarkOk = Mark_id >= 0;
			buttonOk.Sensitive = Nameok && MarkOk;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
           if (Save())
				Respond(Gtk.ResponseType.Ok);
		}

		public override bool Save()
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO models (name, mark_id) " +
					"VALUES (@name, @mark_id)";
			}
			else
			{
				sql = "UPDATE models SET name = @name, mark_id = @mark_id WHERE id = @id";
			}
			logger.Info("Запись модели...");
			QSMain.CheckConnectionAlive();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Itemid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@mark_id", DBWorks.ValueOrNull(Mark_id > 0, Mark_id));
		
				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				return true;
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog("Ошибка записи модели!", logger, ex);
			}
			return false;
		}

		protected void OnButtonMarkEditClicked(object sender, EventArgs e)
		{
			Reference MarkSelect = new Reference();
			MarkSelect.SetMode(true,true,true,true,false);
			MarkSelect.FillList("marks","Марка автомобиля", "Марки автомобилей");
			MarkSelect.Show();
			int result = MarkSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Mark_id = MarkSelect.SelectedID;
				entryMark.Text = MarkSelect.SelectedName;
			}
			MarkSelect.Destroy();
			TestCanSave();
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}
	}
}

