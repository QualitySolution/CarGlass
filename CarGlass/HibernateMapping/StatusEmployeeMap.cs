using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class StatusEmployeeMap : ClassMap<StatusEmployee>
	{
		public StatusEmployeeMap()
		{
			Table("status_employee");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Code).Column("code");
			Map(x => x.Name).Column("name");
			Map(x => x.Comment).Column("comment");
		}
	}
}
