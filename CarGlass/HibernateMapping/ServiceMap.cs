using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class ServiceMap : ClassMap<Domain.Service>
	{
		public ServiceMap()
		{
			Table("services");
			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Name).Column("name");
			Map(x => x.Price).Column("price");
			Map(x => x.Ordinal).Column("ordinal");
			HasMany(x => x.ServiceOrderTypes).Cascade.AllDeleteOrphan().Inverse().LazyLoad().KeyColumn("id_service");
		}
	}
}
