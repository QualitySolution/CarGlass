using System;
using QSOrmProject;

namespace CarGlass.Domain
{
    public class CarBrand : PropertyChangedBase, IDomainObject
    {
        public virtual int Id { get; set; }

		string name;

		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value, () => Name); }
		}

        public CarBrand()
        {
        }
    }
}
