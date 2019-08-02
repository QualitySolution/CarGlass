using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class OrderStateMap : ClassMap<OrderState>
	{
		public OrderStateMap()
		{
			Table("status");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");
			Map(x => x.Color).Column("color");
			Map(x => x.UsedForTypesPlainText).Column("usedtypes");

		}
	}
}
