using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class SheduleWorksMap : ClassMap<SheduleWorks>
	{
		public SheduleWorksMap()
		{
			Table("shedule_works");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.DateWork).Column("date_work");
			Map(x => x.DateCreate).Column("date_create");
			Map(x => x.PointNumber).Column("point_number");
			Map(x => x.CalendarNumber).Column("calendar_number");

			References(x => x.CreatedBy).Column("id_creator").Not.Nullable();

			HasMany(x => x.SheduleEmployeeWorks)
			.Inverse()
			.KeyColumn("id_shedule_works").Not.KeyNullable()
			.Cascade.AllDeleteOrphan()
			.LazyLoad();

		}
	}
}
