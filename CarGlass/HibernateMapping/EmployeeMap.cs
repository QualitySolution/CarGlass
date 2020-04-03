using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class EmployeeMap : ClassMap<Employee>
	{
		public EmployeeMap()
		{
			Table("employees");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.FirstName).Column("first_name");
			Map(x => x.LastName).Column("last_name");
			Map(x => x.Patronymic).Column("patronymic");

		}
	}
}
