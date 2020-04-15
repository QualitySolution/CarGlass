using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class Coefficients : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		string name;

		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value, () => Name); }
		}

		string comment;

		public virtual string Comment
		{
			get { return comment; }
			set { SetField(ref comment, value, () => Comment); }
		}

		public Coefficients()
		{
		}

		public Coefficients(string name, string comment)
		{
			this.Name = name;
			this.Comment = comment;
		}
		public Coefficients(string name)
		{
			this.name = name;
		}
	}
}
