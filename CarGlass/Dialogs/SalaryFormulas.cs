using System;
using CarGlass.Domain;
using CarGlass.Representation;
using Gamma.GtkWidgets;
using Gtk;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using System.Linq;

namespace CarGlass.Dialogs
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SalaryFormulas : WidgetOnDialogBase
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		public SalaryFormulas()
		{
			this.Build();
			createTableFormulas();
		}

		private void createTableFormulas()
		{
			//Service service = null;
			Domain.SalaryFormulas salaryFormulas = null;

			var itemsQueryService = UoW.Session.QueryOver<Service>().Where(x => x.ListServiceOrderType.Count > 0 && x.ListServiceOrderType[0].OrderTypeClass.IsCalculateSalary).List();
			var itemsQuerySalaryFormulas = UoW.Session.QueryOver<Domain.SalaryFormulas>(() => salaryFormulas).List();

			foreach (var serv in itemsQueryService)
				if(itemsQuerySalaryFormulas.Where(x => x.Service == serv).Count() < 1)
					itemsQuerySalaryFormulas.Add(new Domain.SalaryFormulas(serv, "", ""));

			ytreeFormulas.ColumnsConfig = ColumnsConfigFactory.Create<Domain.SalaryFormulas>()
															.AddColumn("Услуга").AddTextRenderer(x => x.Service.Name)
															.AddColumn("Формула").AddTextRenderer(x => x.Formula)
															.AddColumn("Комментарий").AddTextRenderer(x => x.Comment)
															.Finish();
			ytreeFormulas.ItemsDataSource = itemsQuerySalaryFormulas;
			ytreeFormulas.Selection.Changed += Selection_Changed;

		}


		void Selection_Changed(object sender, EventArgs e)
		{
			btnEditFormula.Sensitive = ytreeFormulas.Selection.CountSelectedRows() > 0;
		}


		protected void OnTreeviewFormulasRowActivated(object o, RowActivatedArgs args)
		{
			OnBtnEditFormulaClicked(o, args);
		}

		protected void OnBtnEditFormulaClicked(object sender, EventArgs e)
		{
			Domain.SalaryFormulas w = ytreeFormulas.GetSelectedObject<Domain.SalaryFormulas>();
			AddEditFormulas addEditFormulas = new AddEditFormulas(w);
			addEditFormulas.Show();
			createTableFormulas();
		}

	}
}
