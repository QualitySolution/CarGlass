using System;
using QS.DomainModel.Entity;
using QSOrmProject;

namespace CarGlass.Domain
{
    public class CarModel : PropertyChangedBase, IDomainObject
    {

		public virtual int Id { get; set; }

		string name;

		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value, () => Name); }
		}

        private CarBrand brand;

        public virtual CarBrand Brand
        {
            get { return brand; }
            set { SetField(ref brand, value, () => Brand); }
        }

        public CarModel()
        {
        }
    }
}
