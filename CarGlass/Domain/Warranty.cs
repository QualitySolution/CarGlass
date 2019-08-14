using System;
namespace CarGlass.Domain
{
	public enum Warranty
	{
		None,
		OneYear,
		TwoYear,
		ThreeYear,
		Indefinitely,
		NoWarranty
	}

	public class WarrantyStringType : NHibernate.Type.EnumStringType
	{
		public WarrantyStringType() : base(typeof(Warranty)) { }
	}
}
