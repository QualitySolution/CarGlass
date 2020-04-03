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

		public Employee()
		{
		}

		public Employee(int id, string firstName, string lastName, string Patronymic)
		{
			IUnitOfWork uow = UnitOfWorkFactory.CreateWithNewRoot<Employee>(this);
			Id = id;
			this.firstName = firstName;
			this.lastName = lastName;
			this.patronymic = Patronymic;
			uow.Save(this);

		}
	}
}
