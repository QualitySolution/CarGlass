using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class OrderTypeMap : ClassMap<Domain.OrderTypeClass>
	{
		public OrderTypeMap()
		{
			Table("order_type");
			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Name).Column("name");
			Map(x => x.NameAccusative).Column("name_accusative");
			Map(x => x.IsCalculateSalary).Column("is_calculate_salary");
			Map(x => x.PositionInTabs).Column("position_in_tabs");
			Map(x => x.IsShowMainWidgets).Column("is_show_main_widgets");
			Map(x => x.IsShowAdditionalWidgets).Column("is_show_additional_widgets");
			Map(x => x.IsInstallType).Column("is_install_type");
			Map(x => x.IsOtherType).Column("is_other_type");
			HasMany(x => x.ServiceOrderTypes).Cascade.AllDeleteOrphan().Inverse().LazyLoad().KeyColumn("id_type_order");
		}
	}
}
