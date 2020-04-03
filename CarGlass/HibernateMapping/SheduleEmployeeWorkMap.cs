using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class SheduleEmployeeWorkMap : ClassMap<SheduleEmployeeWork>
	{
		public SheduleEmployeeWorkMap()
		{
			Table("shedule_employee_works");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			References(x => x.SheduleWorks).Column("id_shedule_works").Not.Nullable();
			References(x => x.Employee).Column("id_employee").Not.Nullable();
		}
	}
}
