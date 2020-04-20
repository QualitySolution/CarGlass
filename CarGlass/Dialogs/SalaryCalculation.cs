using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gdk;
using Gtk;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace CarGlass.Dialogs 
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SalaryCalculation : WidgetOnDialogBase
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		IList<EmployeeServiceSalary> listEmployeeServiceSalaries = new List<EmployeeServiceSalary>();
		IList<Employee> listEmployees = new List<Employee>();
		public SalaryCalculation()
		{
			this.Build();
			createTable();

		}

		private void createTable()
		{
			ytreeMain.ColumnsConfig = ColumnsConfigFactory.Create<EmployeeServiceSalary>()
											.AddColumn("Фамилия").AddTextRenderer(x => x.Employee.LastName)
											.AddColumn("Имя").AddTextRenderer(x => x.Employee.FirstName)
											.AddColumn("Отчество").AddTextRenderer(x => x.Employee.Patronymic)
											.AddColumn("Количество выполнненых услуг").AddNumericRenderer(x => x.ColService)
											.AddColumn("Итоговая сумма").AddNumericRenderer(x => x.AllSumma)
											.Finish();

			ytreeService.ColumnsConfig = ColumnsConfigFactory.Create<EmployeeSalaryServiceType>()
								.AddColumn("Услуга").AddTextRenderer(x => x.Service.Name)
								.AddColumn("Количество").AddNumericRenderer(x => x.listCost.Count())
								.AddColumn("Общая стоимость").AddNumericRenderer(x => x.Summa)
								.AddColumn("Формула").AddTextRenderer(x => x.Formula)
								.AddColumn("Сумма").AddNumericRenderer(x => x.SummaAfterFormula)
								.Finish();

		}

		private void setData()
		{
			ytreeMain.ItemsDataSource = listEmployeeServiceSalaries;
			listEmployees = listEmployeeServiceSalaries.Select(x => x.Employee).ToList();
			listEmployees.Add(new Employee("все"));

			cmbEmployees.ItemsList = listEmployees.Select(x => x.FullName).ToList();
			cmbEmployees.SelectedItem = "все";
		}

		protected void OnBtnSettingClicked(object sender, EventArgs e)
		{
			var Dlg = new SalarySettingsDlg();
			Dlg.Show();
		}

		protected void OnBtnCalcClicked(object sender, EventArgs e)
		{
			DateTime start = daterangepicker1.StartDate;
			DateTime end = daterangepicker1.EndDate;
			if(end < start) return;

			Calculate(start, end);
		}

		public void Calculate(DateTime start, DateTime end)
		{
			listEmployeeServiceSalaries.Clear();
			EmployeeServiceWork employeeServiceWork = null;
			IList <EmployeeServiceWork> listEmployeeServiceWork;

			listEmployeeServiceWork = UoW.Session.QueryOver<EmployeeServiceWork>(() => employeeServiceWork).Where(x => x.DateWork >= start && x.DateWork <= end).List();

			if (!checkServiceFormulas(listEmployeeServiceWork))
			{
				MessageDialogWorks.RunWarningDialog("Не для всех услуг указаны формулы расчета.");
				return;
			}

			foreach(var order in listEmployeeServiceWork)
			{
				EmployeeServiceSalary employeeServiceSalary = listEmployeeServiceSalaries.FirstOrDefault(x => x.Employee.Id == order.Employee.Id); 
				if (employeeServiceSalary != null)
				{
					WorkOrderPay workOrderPay = UoW.GetById<WorkOrderPay>(order.WorkOrderPay.Id);
					var employeeServiceWorkType = employeeServiceSalary.listEmployeeSalarySirviceType.FirstOrDefault(x => x.Service.Id == workOrderPay.Service.Id);
					if(employeeServiceWorkType != null)
					{
						foreach(var emp in listEmployeeServiceSalaries)
							if(emp.Employee == order.Employee)
								foreach(var service in emp.listEmployeeSalarySirviceType)
									if(service.Service == order.WorkOrderPay.Service)
										service.listCost.Add(getCost(order, listEmployeeServiceWork));
					}
					else
					{
						EmployeeSalaryServiceType empServiceType = new EmployeeSalaryServiceType(order.WorkOrderPay.Service);
						empServiceType.listCost.Add(getCost(order, listEmployeeServiceWork));
						empServiceType.Formula = getFormula(order);
						foreach(var emp in listEmployeeServiceSalaries)
							if(emp.Employee == order.Employee)
								emp.listEmployeeSalarySirviceType.Add(empServiceType);
					}
				}
				else
				{
					Employee emp  = UoW.GetById<Employee>(order.Employee.Id);
					EmployeeServiceSalary empServiceSalary = new EmployeeServiceSalary(order.Employee);
					EmployeeSalaryServiceType empServiceType = new EmployeeSalaryServiceType(order.WorkOrderPay.Service);
					empServiceType.listCost.Add(getCost(order, listEmployeeServiceWork));
					empServiceType.Formula = getFormula(order);
					empServiceSalary.listEmployeeSalarySirviceType.Add(empServiceType);
					var r = empServiceType.SummaAfterFormula;
					listEmployeeServiceSalaries.Add(empServiceSalary);
				}

				setData();
			}

			foreach(var row in listEmployeeServiceSalaries)
			{
				foreach(var service in row.listEmployeeSalarySirviceType)
					service.Calculation();
				row.getAllSumma();
			}

		}

		private decimal getCost(EmployeeServiceWork order, IList<EmployeeServiceWork> listEmployeeServiceWork)
		{
			var col = listEmployeeServiceWork.Where(x => x.WorkOrderPay == order.WorkOrderPay).ToList().Count();
			return (order.WorkOrderPay.Cost / col);

		}

		private string getFormula(EmployeeServiceWork order)
		{
			IList<EmployeeCoeff> listEmployeeCoeffs;
			listEmployeeCoeffs = UoW.Session.QueryOver<EmployeeCoeff>().Where(x => x.Employee == order.Employee).List();
			var formulaName = UoW.Session.QueryOver<Domain.SalaryFormulas>().Where(x => x.Service.Id == order.WorkOrderPay.Service.Id).List()
								.FirstOrDefault(x => x.Service.Id == order.WorkOrderPay.Service.Id).Formula;
			string str =  "СУММ СУММА SUM сумм сумма sum Сумм Сумма Sum" ;
			foreach(var coef in listEmployeeCoeffs)
			{
				if (str.Contains(coef.Coeff.Name)) continue;
				if(formulaName.Contains(coef.Coeff.Name))
					formulaName = formulaName.Replace(coef.Coeff.Name, coef.Value );
			}

			var listCoeff = UoW.Session.QueryOver<Coefficients>().List();
			foreach(var coeff in listCoeff)
			{
				if(str.Contains(coeff.Name)) continue;
				if(formulaName.Contains(coeff.Name))
					formulaName = formulaName.Replace(coeff.Name, "1");
			}

			return formulaName;
		}

		private bool checkServiceFormulas(IList<EmployeeServiceWork> listEmployeeServiceWork)
		{
			bool isFormulasOK = true;
			Domain.SalaryFormulas salaryFormulas = null;
			var listFormulas = UoW.Session.QueryOver<Domain.SalaryFormulas>(() => salaryFormulas).List();
			foreach(var service in listEmployeeServiceWork)
				if(!listFormulas.Where(x => x.Service == service.WorkOrderPay.Service).Any())
				{
					isFormulasOK = false;
					break;
				}
			return isFormulasOK;
		}

		protected void OnYtreeMainRowActivated(object o, RowActivatedArgs args)
		{
			var row = ytreeMain.GetSelectedObject<EmployeeServiceSalary>();
			ytreeService.ItemsDataSource = row.listEmployeeSalarySirviceType;
		}

		protected void OnCmbEmployeesItemSelected(object sender, Gamma.Widgets.ItemSelectedEventArgs e)
		{
			if(e.SelectedItem.ToString().Equals("все"))
				ytreeMain.ItemsDataSource = listEmployeeServiceSalaries;
			else
				ytreeMain.ItemsDataSource = listEmployeeServiceSalaries.Where(x => x.Employee.FullName.Equals(e.SelectedItem.ToString())).ToList();
		}
	}
}
