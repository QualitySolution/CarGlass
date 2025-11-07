using QS.DomainModel.Entity;
namespace CarGlass.Domain
{
	public class Status : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		string name;
		public virtual string Name
		{
			get { return name; } 
			set { SetField(ref name, value, () => Name); }
		}

		string color;
		public virtual string Color
		{
			get { return color; }
			set { SetField(ref color, value, () => Color); }
		}

		string usedtypes;
		public virtual string Usedtypes
		{
			get { return usedtypes; }
			set { SetField(ref usedtypes, value, () => Usedtypes); }
		}

		public Status()
		{
		}
	}
}
