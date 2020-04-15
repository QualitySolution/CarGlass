﻿using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gtk;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using QSOrmProject;

namespace CarGlass.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class EmployeesCatalog : Gtk.Bin
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		Employee employee = null;
		EmployeeStatusHistory employeeStatusHistory = null;
		StatusEmployee empStatus = null;
		int ActiveCode = 1;

		IList<Employee> listEmployees;
		IList<StatusEmployee> listStatusEmployee;
		IList<EmployeeStatusHistory> listEmployeeStatusHistory;
		IList<EmployeeStatusHistory> listNewEmployeeStatusHistory = new List<EmployeeStatusHistory>();

		public EmployeesCatalog()
		{
			this.Build();
			Configure();
		}

		void Configure()
		{
			listEmployees = UoW.Session.QueryOver<Employee>(() => employee).List();
			listEmployeeStatusHistory = UoW.Session.QueryOver<EmployeeStatusHistory>(() => employeeStatusHistory).List();
			listStatusEmployee = UoW.Session.QueryOver<StatusEmployee>(() => empStatus).List();

			foreach(var emp in listEmployees)
			{
				var list = listEmployeeStatusHistory.Where(x => x.Employee == emp).ToList();
				if(list.Count > 0)
				{
					DateTime date = list.Max(x => x.DateCreate);
					var statushistory = listEmployeeStatusHistory.FirstOrDefault(x => x.Employee == emp && x.DateCreate == date);
					emp.StatusEmployee = statushistory.Status;
				}
				else
				{

					StatusEmployee inWork = listStatusEmployee.FirstOrDefault(x => x.Code == 1);
					var st = new EmployeeStatusHistory(emp, inWork);
					UoW.Save(st);
					UoW.Commit();
					emp.StatusEmployee = st.Status;
				}
			}

			createTable();

		}

		private void createTable()
		{
			ytree.ItemsDataSource = listEmployees.Where(x => x.StatusEmployee.Code == ActiveCode).ToList();
			ytree.ColumnsConfig = ColumnsConfigFactory.Create<Employee>()
											.AddColumn("Фамилия").AddTextRenderer(x => x.LastName).Editable()
											.AddColumn("Имя").AddTextRenderer(x => x.FirstName).Editable()
											.AddColumn("Отчество").AddTextRenderer(x => x.Patronymic).Editable()
											.AddColumn("Статус").AddTextRenderer(x => x.StatusEmployee.Name)
											.Finish();
		}

		private void setStatus(int status)
		{
			var row = ytree.GetSelectedObject<Employee>();
			StatusEmployee st = listStatusEmployee.FirstOrDefault(x => x.Code == status);
			foreach(var emp in listEmployees)
				if(emp == row)
				{
					emp.StatusEmployee = st;
					listNewEmployeeStatusHistory.Add(new EmployeeStatusHistory(emp, st));
				}
			createTable();
		}

		protected void OnBtnAddClicked(object sender, EventArgs e)
		{
			bool isAdd = true;
			foreach(var emp in listEmployees)
			{
				if(emp.FirstName == "-" || emp.FirstName == "")
					isAdd = false;
			}

			if(isAdd)
			{
				Employee s = new Employee("-", "-", "-");
				s.StatusEmployee = listStatusEmployee.First(x => x.Code == 1);
				listEmployees.Add(s);
				createTable();
			}
		}

		protected void OnBtnAcceptClicked(object sender, EventArgs e)
		{
			setStatus(1);
		}

		protected void OnBtnDismissClicked(object sender, EventArgs e)
		{
			setStatus(0);
		}

		protected void OnBtnSaveClicked(object sender, EventArgs e)
		{
			var delEmp = listEmployees.FirstOrDefault(x => x.FirstName == "-" || x.FirstName == "");
			listEmployees.Remove(delEmp);
			foreach(var emp in listEmployees)
				UoW.Save(emp);
			UoW.Commit();

			foreach(var stHis in listNewEmployeeStatusHistory)
				UoW.Save(stHis);
			UoW.Commit();

			createTable();
		}

		protected void OnYcheckbutton1Clicked(object sender, EventArgs e)
		{
			ActiveCode = ycheckbutton1.Active ? 1 : 0;
			btnAccept.Sensitive = ycheckbutton1.Active;
			btnDismiss.Sensitive = !ycheckbutton1.Active;
			if(ycheckbutton1.Active)
				ycheckbutton1.Label = "работающие";
			else
				ycheckbutton1.Label = "уволенные";
			createTable();
		}
	}
}