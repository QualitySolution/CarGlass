using CarGlass.Domain;
using FluentNHibernate.Mapping;
namespace CarGlass.HibernateMapping
{
	public class ServiceOrderTypeMap : ClassMap<ServiceOrderType>
	{
		public ServiceOrderTypeMap()
		{
			Table("service_order_type");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			References(x => x.Service).Column("id_service").Not.Nullable();
			References(x => x.OrderTypeClass).Column("id_type_order").Not.Nullable();
		}
	}
}
