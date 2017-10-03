using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class CarWindowMap: ClassMap<CarWindow>
    {

        public CarWindowMap()
        {
			Table("glass");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
            Map(x => x.Name).Column("name");
        }
    }
}
