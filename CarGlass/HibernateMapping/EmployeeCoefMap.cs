using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class EmployeeCoefMap : ClassMap<EmployeeCoeff>
	{
		public EmployeeCoefMap()
		{
			Table("employee_coeff");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Value).Column("value");
			References(x => x.Employee).Column("id_employee").Not.Nullable();
			References(x => x.Coeff).Column("id_coeff").Not.Nullable();


		}
	}
}
