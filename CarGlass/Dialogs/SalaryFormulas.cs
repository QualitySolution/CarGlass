using System;
using CarGlass.Domain;
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
		public SalaryFormulas()
		{
			this.Build();
			createTableFormulas();
		}

		private void createTableFormulas()
		{
			using(IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot())
			{
				Domain.SalaryFormulas salaryFormulas = null;
				var itemsQueryService = uow.Session.QueryOver<Service>().Where(x => x.ListServiceOrderType != null).List().Where(x => x.ListServiceOrderType.Count > 0).ToList()
				.Where(x => x.ListServiceOrderType[0].OrderTypeClass.IsCalculateSalary).ToList();
				var itemsQuerySalaryFormulas = uow.Session.QueryOver<Domain.SalaryFormulas>(() => salaryFormulas).List();

				foreach(var serv in itemsQueryService)
					if(itemsQuerySalaryFormulas.Where(x => x.Service.Id == serv.Id).Count() < 1)
						itemsQuerySalaryFormulas.Add(new Domain.SalaryFormulas(serv, "", ""));

				ytreeFormulas.ColumnsConfig = ColumnsConfigFactory.Create<Domain.SalaryFormulas>()
																.AddColumn("Услуга").AddTextRenderer(x => x.Service.Name)
																.AddColumn("Формула").AddTextRenderer(x => x.Formula)
																.AddColumn("Комментарий").AddTextRenderer(x => x.Comment)
																.Finish();
				ytreeFormulas.ItemsDataSource = itemsQuerySalaryFormulas;
				ytreeFormulas.Selection.Changed += Selection_Changed;
			}
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
			AddEditFormulas frmAddEditFormulas = new AddEditFormulas(w);
			frmAddEditFormulas.Show();
			if(frmAddEditFormulas.Run() == (int)ResponseType.Ok)	
				createTableFormulas();
		}

	}
}
