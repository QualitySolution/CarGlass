using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class Employee : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		string firstName;

		public virtual string FirstName
		{
			get { return firstName; }
			set { SetField(ref firstName, value, () => FirstName); }
		}

		string lastName;

		public virtual string LastName
		{
			get { return lastName; }
			set { SetField(ref lastName, value, () => LastName); }
		}

		string patronymic;

		public virtual string Patronymic
		{
			get { return patronymic; }
			set { SetField(ref patronymic, value, () => Patronymic); }
		}

		string fullName;

		public virtual string FullName
		{
			get { return String.Format("{0} {1} {2}", LastName, FirstName, Patronymic).Trim(); }
		}

		public virtual string PersonNameWithInitials()
		{
			string result = String.Empty;
			if(!String.IsNullOrWhiteSpace(LastName))
				result += String.Format("{0} ", LastName);
			if(!String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName))
				result += String.Format("{0}.", FirstName[0]);
			if(String.IsNullOrWhiteSpace(LastName))
				result += String.Format("{0}", FirstName);
			if(!String.IsNullOrWhiteSpace(Patronymic) && !String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName))
				result += String.Format("{0}.", Patronymic[0]);
			return result;
		}

		StatusEmployee statusEmployee;

		public virtual StatusEmployee StatusEmployee
		{
			get { return statusEmployee; }
			set { SetField(ref statusEmployee, value, () => StatusEmployee); }
		}

		public Employee()
		{
		}

		public Employee(int id, string firstName, string lastName, string patronymic)
		{
			Id = id;
			this.firstName = firstName;
			this.lastName = lastName;
			this.patronymic = patronymic;
		}

		public Employee(string firstName, string lastName = "", string patronymic = "")
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.Patronymic = patronymic;
		}
	}
}
