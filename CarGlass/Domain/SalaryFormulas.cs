using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class SalaryFormulas : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		string formula;

		public virtual string Formula
		{
			get { return formula; }
			set { SetField(ref formula, value, () => Formula); }
		}

		Service service;
		public virtual Service Service
		{
			get { return service; }
			set { SetField(ref service, value, () => Service); }
		}

		string comment;

		public virtual string Comment
		{
			get { return comment; }
			set { SetField(ref comment, value, () => Comment); }
		}


		public SalaryFormulas(Service service, string formula, string comment)
		{
			this.Service = service;
			this.Formula = formula;
			this.Comment = comment;
		}

		public SalaryFormulas()
		{
		}
	}
}
