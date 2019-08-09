using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class WorkOrderPayMap : ClassMap<WorkOrderPay>
	{
		public WorkOrderPayMap()
		{
			Table("order_pays");
			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Cost).Column("cost");

			References(x => x.WorkOrder).Column("order_id");
			References(x => x.Service).Column("service_id");
		}
	}
}
