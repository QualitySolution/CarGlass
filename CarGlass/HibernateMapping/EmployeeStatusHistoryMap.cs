using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class EmployeeStatusHistoryMap : ClassMap<EmployeeStatusHistory>
	{
		public EmployeeStatusHistoryMap()
		{
			Table("employee_status_history");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.DateCreate).Column("date_create");
			References(x => x.Employee).Column("id_employee").Not.Nullable();
			References(x => x.Status).Column("id_status").Not.Nullable();
		}
	}
}
