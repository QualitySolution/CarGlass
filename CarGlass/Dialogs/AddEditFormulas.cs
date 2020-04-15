using System;
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
	public partial class AddEditFormulas : Gtk.Dialog
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		Domain.SalaryFormulas salaryFormulas = null;
		Coefficients coefficients = null;
		IList<Domain.SalaryFormulas> listSalaryFormulas;
		IList<Coefficients> listCoefficients;


		public AddEditFormulas()
		{
			this.Build();
		}

		public AddEditFormulas(Domain.SalaryFormulas salaryFormulas)
		{
			this.Build();
			this.salaryFormulas = salaryFormulas;
			if(salaryFormulas.Id != 0)
				listSalaryFormulas = UoW.Session.QueryOver<Domain.SalaryFormulas>(() => salaryFormulas).Where(x => x.Id == salaryFormulas.Id).List();
			listCoefficients = UoW.Session.QueryOver<Coefficients>(() => coefficients).List();
			yentry.Text = salaryFormulas.Formula;
			ytreeCoeff.ItemsDataSource = listCoefficients;
			label2.Text = "Коэффициенты:";
			label3.Text = "Запишите формулу, используя коэффициенты из таблицы.\n" +
				"Если нужного коэффициента нет - добавьте его.";

			ytreeCoeff.ColumnsConfig = ColumnsConfigFactory.Create<Domain.Coefficients>()
															.AddColumn("Услуга").AddTextRenderer(x => x.Name).Editable()
															.AddColumn("Комментарий").AddTextRenderer(x => x.Comment).Editable()
															.Finish();

		}

		protected void OnBtnAddCoeffClicked(object sender, EventArgs e)
		{
			listCoefficients.Add(new Coefficients("-", "-"));
			ytreeCoeff.ItemsDataSource = listCoefficients;
		}

		protected void OnBtnDeleteCoeffClicked(object sender, EventArgs e)
		{
			Coefficients coeff = ytreeCoeff.GetSelectedObject<Coefficients>();
			listCoefficients.Remove(coeff);
			ytreeCoeff.ItemsDataSource = listCoefficients;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			var deleteList = listCoefficients.Where(x => x.Name == "-" || x.Name == "").ToList();
			foreach(var coeff in listCoefficients)
				if(deleteList.Contains(coeff))
					listCoefficients.Remove(coeff);
				else
				{
					UoW = UnitOfWorkFactory.CreateWithNewRoot<Coefficients>();
					UoW.Save(coeff);
					UoW.Commit();
				}
			salaryFormulas.Formula = yentry.Text;
			salaryFormulas.Comment = yentryComment.Text;
			var r = salaryFormulas;
			UoW = UnitOfWorkFactory.CreateWithNewRoot<Domain.SalaryFormulas>();
			UoW.Save(salaryFormulas);
			UoW.Commit();
			Respond(ResponseType.Ok);
		}

		protected void OnBtnSelectCoeffClicked(object sender, EventArgs e)
		{
			Coefficients coeff;
			if(ytreeCoeff.Selection.CountSelectedRows() > 0)
			{
				coeff = ytreeCoeff.GetSelectedObject<Coefficients>();
				yentry.Text = yentry.Text + coeff.Name;
			}
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}
