using System;
using System.Collections.Generic;
using QS.DomainModel.Entity;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Linq;
using QSProjectsLib;

namespace CarGlass.Domain
{
	public class EmployeeSalaryServiceType : PropertyChangedBase
	{
		private Service service;

		public virtual Service Service
		{
			get { return service; }
			set { SetField(ref service, value); }
		}

		public IList<decimal> listCost = new List<decimal>();

		private decimal summa;

		public virtual decimal Summa
		{
			get { return listCost.Sum(); }
			set { SetField(ref summa, value); }
		}

		private decimal summaAfterFormula;

		public virtual decimal SummaAfterFormula
		{
			get { return summaAfterFormula; }
			set { SetField(ref summaAfterFormula, value); }
		}

		string formula;
		public string Formula 
		{
			get { return formula; }
			set { SetField(ref formula, value); }
		}
		public EmployeeSalaryServiceType(Service ser)
		{
			service = ser;
		}

		public EmployeeSalaryServiceType()
		{
		}

		public void Calculation()
		{
			if(formula == null) return;
			String[] str = { "СУММА", "СУММ", "Сумма", "Сумм", "сумма", "сумм", "SUM", "sum", "Sum"};
			foreach(var ch in str)
				formula = formula.Replace(ch, Summa.ToString());
			formula = formula.Replace("=", "");
			formula = formula.Replace(",", ".");
			SummaAfterFormula = (decimal)Evaluator(formula);
		}

		 double Evaluator(string expression)
		{

			ICodeCompiler cs = (new CSharpCodeProvider().CreateCompiler());
			CompilerParameters cp = new CompilerParameters();
			//cp.ReferencedAssemblies.Add("system.dll");
			cp.GenerateExecutable = false; // создать DLL
			cp.GenerateInMemory = true;  // создать в памяти

			string code = string.Empty;
			code += "using System;";
			code += "namespace CSEvaluator";
			code += "{ public class Evaluate";
			code += "  { public  double GetResult(){ return(\r\n" + expression + "\r\n); }";
			code += "    private double sin(double x){ return(Math.Sin(x)); }";
			code += "    private double cos(double x){ return(Math.Cos(x)); }";
			code += "  }";
			code += "}";

			CompilerResults cr = cs.CompileAssemblyFromSource(cp, code);
			if(cr.Errors != null && cr.Errors.Count > 0) 
			{
				for(int i = 0; i < cr.Errors.Count; i++)
					Console.WriteLine("Col {0} - {1}", cr.Errors[i].Column, cr.Errors[i].ErrorText);
				MessageDialogWorks.RunWarningDialog($"Для услуги\n {Service.Name}\n указана неккоректная формулу");
				return (0.0);
			}

			try 
			{
				object ob = cr.CompiledAssembly.CreateInstance("CSEvaluator.Evaluate");
				return ((double)ob.GetType().InvokeMember("GetResult", BindingFlags.InvokeMethod, null, ob, new object[] { }));
			}
			catch(Exception ex) { MessageDialogWorks.RunWarningDialog($"Для услуги\n {Service.Name}\n указана неккоректная формулу"); Console.WriteLine(ex.Message); return (0.0); }
		}
	}
}
