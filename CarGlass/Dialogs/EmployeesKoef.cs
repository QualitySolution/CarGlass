using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gtk;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace CarGlass.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class EmployeesKoef : Gtk.Bin
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		EmployeeCoeff employeeCoeff = null;
		Employee employee = null;
		Coefficients coefficients = null;
		IList<EmployeeCoeff> listEmployeeCoeff;
		IList<Employee> listEmployees;
		IList<Coefficients> listCoefficients;
		public EmployeesKoef()
		{
			this.Build();
			CreateTable();
		}

		void CreateTable()
		{
			listEmployeeCoeff = UoW.Session.QueryOver<EmployeeCoeff>(() => employeeCoeff).List();
			listEmployees = UoW.Session.QueryOver<Employee>(() => employee).List();
			listCoefficients = UoW.Session.QueryOver<Coefficients>(() => coefficients).List();

			foreach(var emp in listEmployees)
				foreach(var coeff in listCoefficients)
					if(listEmployeeCoeff.Where(x => x.Employee == emp && x.Coeff == coeff).Count() < 1)
						listEmployeeCoeff.Add(new EmployeeCoeff(emp, coeff));

			ytree.ItemsDataSource = listEmployeeCoeff;
			ytree.ColumnsConfig = ColumnsConfigFactory.Create<EmployeeCoeff>()
												.AddColumn("ФИО").AddTextRenderer(x => x.Employee.FullName)
												.AddColumn("Коэффициент").AddTextRenderer(x => x.Coeff.Name)
												.AddColumn("Значение").AddTextRenderer(x => x.Value).Editable()
												.Finish();

			listEmployees.Add(new Employee("все"));
			listCoefficients.Add(new Coefficients("все"));


			comboEmployees.ItemsList = listEmployees.Select(x => x.FullName).ToList();
			comboCoeff.ItemsList = listCoefficients.Select(x => x.Name).ToList();
			comboEmployees.SelectedItem = "все";
			comboCoeff.SelectedItem = "все";
		}

		protected void OnComboMarkItemSelected(object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			var r = listEmployees;
			if(e.SelectedItem.ToString().Equals("все"))
				ytree.ItemsDataSource = listEmployeeCoeff;
			else
				ytree.ItemsDataSource = listEmployeeCoeff.Where(x => x.Employee.FullName.Equals(e.SelectedItem.ToString())).ToList();
		}

		protected void OnComboMark1ItemSelected(object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			if (e.SelectedItem.ToString().Equals("все"))
				ytree.ItemsDataSource = listEmployeeCoeff;
			else
				ytree.ItemsDataSource = listEmployeeCoeff.Where(x => x.Coeff.Name.Equals(e.SelectedItem.ToString())).ToList();
		}

		protected void OnBtnSaveClicked(object sender, EventArgs e)
		{
			foreach(var row in listEmployeeCoeff)
			{
				if(row.Value == null)
					row.Value = "1";
				row.Value.Replace(",", ".");
				UoW.Save(row);

			}
			UoW.Commit();
			MessageDialogWorks.RunInfoDialog("Сохранено.");
		}
	}

}
