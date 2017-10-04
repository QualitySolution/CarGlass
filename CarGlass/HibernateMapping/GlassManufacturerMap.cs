using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class GlassManufacturerMap: ClassMap<GlassManufacturer>
	{
		public GlassManufacturerMap()
		{
			Table("manufacturers");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");
		}
	}
}
