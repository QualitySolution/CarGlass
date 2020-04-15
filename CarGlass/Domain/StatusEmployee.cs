using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;

namespace CarGlass.Domain
{
	public class StatusEmployee : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		int code;
		public virtual int Code
		{
			get { return code; }
			set { SetField(ref code, value, () => Code); }
		}

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

		public StatusEmployee()
		{
		}
	}
}
