using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using Gamma.GtkWidgets;
using Gtk;
using QS.Dialog.Gtk;
using QS.DomainModel.UoW;
using QSOrmProject;
using QSProjectsLib;

namespace CarGlass.Dialogs
{
	public partial class AddEditFormulas : Gtk.Dialog
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		Domain.SalaryFormulas salaryFormulas = new Domain.SalaryFormulas();
		IList<Coefficients> listCoefficients;
		IList<Coefficients> listCoefficientsDelete = new List<Coefficients>();
		bool isEditCoeff;

		public AddEditFormulas()
		{
			this.Build();
		}

		public AddEditFormulas(Domain.SalaryFormulas SalaryFormula)
		{
			this.Build();
			this.Title = "Редактирование формулы";
			if(SalaryFormula.Id != 0)
				this.salaryFormulas = UoW.Session.QueryOver<Domain.SalaryFormulas>().List().FirstOrDefault(x => x.Id == SalaryFormula.Id);

			salaryFormulas.Service = SalaryFormula.Service;
			listCoefficients = UoW.Session.QueryOver<Coefficients>().List();
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
			isEditCoeff = true;
		}

		protected void OnBtnDeleteCoeffClicked(object sender, EventArgs e)
		{
			Coefficients coeff = ytreeCoeff.GetSelectedObject<Coefficients>();
			if(coeff == null) return;
			if(checkCoeffBeforeDelete(coeff.Name))
			{
				MessageDialogWorks.RunWarningDialog("Коэффициент присутствует в формулах.\n Удаление невозможно.");
				return;
			}
			listCoefficients.Remove(coeff);
			listCoefficientsDelete.Add(coeff);
			ytreeCoeff.ItemsDataSource = listCoefficients;
			isEditCoeff = true;
		}

		private bool checkCoeffBeforeDelete(string coeff)
		{
			Domain.SalaryFormulas salaryFormulas = null;
			IList<Domain.SalaryFormulas> listFormulas = UoW.Session.QueryOver<Domain.SalaryFormulas>(() => salaryFormulas).List();
			foreach(var formula in listFormulas)
				if(formula.Formula.Contains(coeff))
				{
					string str = "- =/*+()";
					int index = formula.Formula.IndexOf(coeff);
					int indexAfter = index + coeff.Length + 1;

					string strAfter, strBefore;
					if(indexAfter > formula.Formula.Length)
						strAfter = " ";
					else strAfter = formula.Formula.ToArray()[indexAfter].ToString();
					if(index == 0)
						strBefore = " ";
					else strBefore = formula.Formula.ToArray()[--index].ToString();

					if(str.Contains(strBefore) && str.Contains(strAfter))
						return true;// удалять нельзя
					return false;
				}

			return false;
		}


		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			if (!CheckFormula())
			{
				MessageDialogWorks.RunWarningDialog("В формуле присутсвуют коэффициенты,\n которых нет в справочнике.\n Сохранение невозможно.");
				return;
			}

			if(isEditCoeff)
				SaveCoeff();

			SaveSalaryFormula();
			Respond(ResponseType.Ok);
			this.Destroy();
		}

		private bool CheckFormula()
		{
			string text = yentry.Text;
			foreach(var coeff in listCoefficients)
				if(text.Contains(coeff.Name))
					text = text.Replace(coeff.Name, "");
			foreach(var ch in text)
				if(char.IsLetter(ch))
					return false;
			return true;
		}

		private void SaveSalaryFormula()
		{
            salaryFormulas.Formula = yentry.Text;
			salaryFormulas.Comment = yentryComment.Text;
			UoW.Save(salaryFormulas);
			UoW.Commit();

		}

		protected void SaveCoeff()
        {
			foreach(var del in listCoefficientsDelete)
				UoW.Delete(del);

			UoW.Commit();
			var deleteList = listCoefficients.Where(x => x.Name == "-" || x.Name == "").ToList();
			foreach(var coeff in listCoefficients)
				if(deleteList.Contains(coeff))
					listCoefficients.Remove(coeff);
				else
				{
					UoW.Save(coeff);
					UoW.Commit();
				}

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
