using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class StoreItemMap : ClassMap<StoreItem>
	{
		public StoreItemMap()
		{
			Table("store_items");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.EuroCode).Column("eurocode");
			Map(x => x.Cost).Column("cost");
			Map(x => x.Amount).Column("amount");
			Map(x => x.Placement).Column("placement");
			Map(x => x.Comment).Column("comment");

			References(x => x.CarBrand).Column("mark_id");
			References(x => x.CarModel).Column("model_id");
			References(x => x.CarWindow).Column("glass_id");
			References(x => x.Manufacturer).Column("manufacturer_id");
		}
	}
}
