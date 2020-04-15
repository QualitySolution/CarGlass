using System;
using System.Collections.Generic;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gtk;
using QS.Dialog.Gtk;

namespace CarGlass.Dialogs 
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SalaryCalculation : WidgetOnDialogBase
	{
		IList<EmployeeServiceSalary> listEmployeeServiceSalary = new List<EmployeeServiceSalary>();
		public SalaryCalculation()
		{
			this.Build();

		}

		private void createTable()
		{
			ytreeMain.ColumnsConfig = ColumnsConfigFactory.Create<EmployeeServiceSalary>()
											.AddColumn("Фамилия").AddTextRenderer(x => x.Employee.LastName).Editable()
											.AddColumn("Имя").AddTextRenderer(x => x.Employee.FirstName).Editable()
											.AddColumn("Отчество").AddTextRenderer(x => x.Employee.Patronymic).Editable()
											.AddColumn("Количество выполнненых услуг").AddNumericRenderer(x => x.ColService)
											.AddColumn("Итоговая сумма").AddNumericRenderer(x => x.AllSumma)
											.Finish();
			ytreeMain.ItemsDataSource = listEmployeeServiceSalary;
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
			Calculate(start, end);
		}

		private void Calculate(DateTime start, DateTime end)
		{
			EmployeeServiceWork employeeServiceWork = null;
			WorkOrderPay workOrderPay = null;
			IList<EmployeeServiceWork> listEmployeeServiceWork;
			IList<WorkOrderPay> listWorkOrderPay;

			listEmployeeServiceWork = UoW.Session.QueryOver<EmployeeServiceWork>(() => employeeServiceWork).Where(x => x.DateWork >= start && x.DateWork <= end).List();
			foreach(var order in listEmployeeServiceWork)
			{
				listWorkOrderPay = UoW.Session.QueryOver<WorkOrderPay>(() => workOrderPay).Where(x => x.Id == order.Id).List();
				foreach(var service in listWorkOrderPay)
				{
					UoW.GetById<Service>((int)service.Id);
				}
			}

		}
	}
}
