using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
    public class CarBrandMap: ClassMap<CarBrand>
    {

        public CarBrandMap()
        {
			Table("marks");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Name).Column("name");
        }
    }
}
