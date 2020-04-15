using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class EmployeeServiceWorkMap : ClassMap<EmployeeServiceWork>
	{
		public EmployeeServiceWorkMap()
		{
			Table("employee_service_work");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.DateWork).Column("date_work");
			References(x => x.Employee).Column("id_employee").Not.Nullable();
			References(x => x.WorkOrderPay).Column("id_order_pay").Not.Nullable();
		}
	}
}
