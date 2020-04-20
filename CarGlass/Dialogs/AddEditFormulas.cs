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
		Domain.SalaryFormulas salaryFormulas = null;
		Coefficients coefficients = null;
		IList<Domain.SalaryFormulas> listSalaryFormulas;
		IList<Coefficients> listCoefficients;
		IList<Coefficients> listCoefficientsDelete = new List<Coefficients>();

		public AddEditFormulas()
		{
			this.Build();
		}

		public AddEditFormulas(Domain.SalaryFormulas salaryFormulas)
		{
			this.Build();
			this.Title = "Редактирование формулы";
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
			if(checkCoeffBeforeDelete(coeff.Name))
			{
				MessageDialogWorks.RunWarningDialog("Коэффициент присутствует в формулах.\n Удаление невозможно.");
				return;
			}
			listCoefficients.Remove(coeff);
			listCoefficientsDelete.Add(coeff);
			ytreeCoeff.ItemsDataSource = listCoefficients;
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
			Save();
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

		private void Save()
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
