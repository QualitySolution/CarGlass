using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class StockMap : ClassMap<Stock>
	{
		public StockMap()
		{
			Table("stocks");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");
			Map(x => x.Color).Column("color");
		}
	}
}
