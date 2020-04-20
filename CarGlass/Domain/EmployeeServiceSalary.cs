using System;
using System.Collections.Generic;
using QS.DomainModel.Entity;
namespace CarGlass.Domain
{
	public class EmployeeServiceSalary :  PropertyChangedBase
	{
		Employee employee;

		public virtual Employee Employee
		{
			get { return employee; }
			set { SetField(ref employee, value, () => Employee); }
		}

		public IList<EmployeeSalaryServiceType> listEmployeeSalarySirviceType = new List<EmployeeSalaryServiceType>(); // но только с определнным сервисом

		private decimal allSumma;
		public virtual decimal AllSumma
		{
			get {

				return allSumma; }
			set { SetField(ref allSumma, value); }
		}

		public virtual int ColService
		{
			get { return listEmployeeSalarySirviceType.Count; }

		}
		public EmployeeServiceSalary()
		{
		}

		public EmployeeServiceSalary(Employee emp)
		{
			employee = emp;
		}

		public void getAllSumma()
		{
			foreach(var service in listEmployeeSalarySirviceType)
				allSumma += service.SummaAfterFormula;

		}
	}
}
