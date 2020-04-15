using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class EmployeeCoeff : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		string value;

		public virtual string Value
		{
			get { return value; }
			set { SetField(ref this.value, value, () => Value); }
		}

		Employee employee;

		public virtual Employee Employee
		{
			get { return employee; }
			set { SetField(ref employee, value, () => Employee); }
		}

		Coefficients coeff;
		public virtual Coefficients Coeff
		{
			get { return coeff; }
			set { SetField(ref coeff, value, () => Coeff); }
		}


		public EmployeeCoeff()
		{
		}

		public EmployeeCoeff(Employee emp, Coefficients coeff)
		{
			this.Employee = emp;
			this.Coeff = coeff;
		}
	}
}
