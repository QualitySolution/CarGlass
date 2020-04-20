using System;
using System.Linq;
using CarGlass.Domain;
using Gtk;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using QS.Project.Repositories;
using QSOrmProject;
using QSProjectsLib;

namespace CarGlass.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SheduleDlg : FakeTDIEntityGtkDialogBase<SheduleWorks>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		ListStore EmployeeWorkList = new Gtk.ListStore(
			typeof(int),    // 0 - service id
			typeof(bool),   // 1 - Активность
			typeof(string), // 2 - Фамилия 
			typeof(string), // 3 - Имя
			typeof(string)    // 4 - Отчество
			);

		public SheduleDlg(ushort pointNumber, ushort calendarNumber, DateTime date)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<SheduleWorks>();
			Entity.DateWork = date;
			Entity.DateCreate = DateTime.Now;
			Entity.PointNumber = pointNumber;
			Entity.CalendarNumber = calendarNumber;
			Entity.CreatedBy = UserRepository.GetCurrentUser(UoW);

			ConfigureDlg();

		}
		public SheduleDlg(SheduleWorks shWork) : this(shWork.Id) { }

		public SheduleDlg(int id)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<SheduleWorks>(id);
			ConfigureDlg();
			Fill();
		}

		void ConfigureDlg()
		{
			this.Title = "График работы на " + Entity.DateWork.ToLongDateString();

			CellRendererToggle CellPay = new CellRendererToggle();
			CellPay.Activatable = true;
			CellPay.Toggled += onCellPayToggled;

			CellRendererText CellText = new CellRendererText();
			CellText.Editable = true;
			CellText.Edited += CellText_Edited;

			CellRendererText CellText2 = new CellRendererText();
			CellText2.Editable = true;
			CellText2.Edited += CellText_Edited2;

			CellRendererText CellText3 = new CellRendererText();
			CellText3.Editable = true;
			CellText3.Edited += CellText_Edited3;

			table.AppendColumn("", CellPay, "active", 1);
			table.AppendColumn("Фамилия", CellText, "text", 2);
			table.AppendColumn("Имя", CellText2, "text", 3);
			table.AppendColumn("Отчество", CellText3, "text", 4);

			((CellRendererToggle)table.Columns[0].CellRenderers[0]).Activatable = true;

			table.Model = EmployeeWorkList;
			table.ShowAll();

			var sql = "SELECT emp.id, emp.first_name, emp.last_name, emp.patronymic, st.id_status, stEmp.code FROM employees emp " +
				" LEFT JOIN (select history1.id_status,  history1.id_employee as id_employee" +
				" from employee_status_history history1 where history1.date_create = " +
				" (select max(date_create) from employee_status_history where id_employee = history1.id_employee) ) as st on st.id_employee = emp.id" +
				" LEFT JOIN status_employee stEmp on stEmp.id = st.id_status" +
				" where stEmp.code != 0 " +
				" ORDER BY emp.last_name";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while(rdr.Read())
				{
					EmployeeWorkList.AppendValues(rdr.GetInt32("id"),
						false,
						rdr.GetString("last_name"),
						rdr.GetString("first_name"),
						rdr.GetString("patronymic")
					);
				}
			}
			buttonOk.Sensitive = false;

		}

		void onCellPayToggled(object o, ToggledArgs args)
		{
			TreeIter iter;

			if(EmployeeWorkList.GetIter(out iter, new TreePath(args.Path)))
			{
				bool old = (bool)EmployeeWorkList.GetValue(iter, 1);
				EmployeeWorkList.SetValue(iter, 1, !old);
			}
			CalculateTotal();
		}

		void CellText_Edited(object o, EditedArgs args)
		{
			TreeIter iter;
			if(EmployeeWorkList.GetIter(out iter, new TreePath(args.Path)))
				EmployeeWorkList.SetValue(iter, 2, args.NewText);
		}

		void CellText_Edited2(object o, EditedArgs args)
		{
			TreeIter iter;
			if(EmployeeWorkList.GetIter(out iter, new TreePath(args.Path)))
				EmployeeWorkList.SetValue(iter, 3, args.NewText);
		}

		void CellText_Edited3(object o, EditedArgs args)
		{
			TreeIter iter;
			if(EmployeeWorkList.GetIter(out iter, new TreePath(args.Path)))
				EmployeeWorkList.SetValue(iter, 4, args.NewText);
		}


		private void CalculateTotal()
		{
			double Total = 0;
			foreach(object[] row in EmployeeWorkList)
			{
				if((bool)row[1])
					Total++; ;
			}
			labelSum.LabelProp = String.Format("<span foreground=\"red\"><b>Выбрано: {0}</b></span>", Total);
			labelSum.QueueResize();
			buttonOk.Sensitive = Total > 0 && Total < 4;

		}

		private void Fill()
		{
			var sql = "select shw.id as shw_id, emp.id as emp_id, emp.first_name, emp.last_name, emp.patronymic from employees emp " +
				" left join shedule_employee_works shew on shew.id_employee = emp.id" +
				" left join shedule_works shw on shew.id_shedule_works = shw.id" +
				" where shw.id = @id" +
				" order by shw.id desc, emp.first_name;";
			var cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Entity.Id);
			using(MySqlDataReader rdr = cmd.ExecuteReader())
			{
				TreeIter iter;
				while(rdr.Read())
				{
					if(ListStoreWorks.SearchListStore(EmployeeWorkList, rdr.GetInt32("emp_id"), 0, out iter))
					{
						EmployeeWorkList.SetValue(iter, 1, true);
						EmployeeWorkList.SetValue(iter, 2, rdr["last_name"].ToString());
						EmployeeWorkList.SetValue(iter, 3, rdr["first_name"].ToString());
						EmployeeWorkList.SetValue(iter, 4, rdr["patronymic"].ToString());
					}
				}
			}
			CalculateTotal();
			logger.Info("Ok");

		}



		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			PrepareSave();
			Save();
		}

		void PrepareSave()
		{
			foreach(object[] row in EmployeeWorkList)
			{
				var empWork = Entity.SheduleEmployeeWorks.FirstOrDefault(x => x.Employee.Id == (int)row[0]);
				var emp = UoW.GetById<Employee>((int)row[0]);

				if((bool)row[1])
				{
					if(empWork == null)
					{
						empWork = new SheduleEmployeeWork(Entity, emp);
						Entity.SheduleEmployeeWorks.Add(empWork);
					}

				}
				else if(empWork != null)
				{
					Entity.SheduleEmployeeWorks.Remove(empWork);
				}
			}

		}

		public override bool Save()
		{
			UoW.Save();
			logger.Info("Save shedule works on " + Entity.DateWork);
			Respond(ResponseType.Ok);
			return true;
		}
	}
}
