using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class Manufacturer : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название")]
		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value); }
		}

		public Manufacturer()
		{
		}
	}
}
