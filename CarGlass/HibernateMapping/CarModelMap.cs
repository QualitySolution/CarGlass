using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
    public class CarModelMap : ClassMap<CarModel>
    {
        public CarModelMap()
        {
			Table("equipment");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");

            References(x => x.Brand).Column("mark_id");
        }
    }
}
