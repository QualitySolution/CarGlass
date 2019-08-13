using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class WorkOrderMap : ClassMap<WorkOrder>
	{
		public WorkOrderMap()
		{
			Table("orders");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Date).Column("date");
			Map(x => x.Hour).Column("hour");
			Map(x => x.OrderType).Column("type");
			Map(x => x.PointNumber).Column("point_number");
			Map(x => x.CalendarNumber).Column("calendar_number");
			Map(x => x.CreatedDate).Column("created_date");
			Map(x => x.CarYear).Column("car_year");
			Map(x => x.Phone).Column("phone");
			Map(x => x.Eurocode).Column("eurocode");
			Map(x => x.Comment).Column("comment");

			Map(x => x.WarrantyInstall).Column("warranty_install").CustomType<WarrantyStringType>();
			Map(x => x.WarrantyTinting).Column("warranty_tinting").CustomType<WarrantyStringType>();
			Map(x => x.WarrantyPolishing).Column("warranty_polishing").CustomType<WarrantyStringType>();
			Map(x => x.WarrantyArmoring).Column("warranty_armoring").CustomType<WarrantyStringType>();
			Map(x => x.WarrantyPasting).Column("warranty_pasting").CustomType<WarrantyStringType>();

			References(x => x.CreatedBy).Column("created_by");
			References(x => x.CarModel).Column("car_model_id");
			References(x => x.OrderState).Column("status_id");
			References(x => x.Manufacturer).Column("manufacturer_id");
			References(x => x.Stock).Column("stock_id");

			HasMany(x => x.Pays).Cascade.AllDeleteOrphan().Inverse().LazyLoad().KeyColumn("order_id");
		}
	}
}
