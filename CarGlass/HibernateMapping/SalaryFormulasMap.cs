using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class SalaryFormulasMap : ClassMap<Domain.SalaryFormulas>
	{
		public SalaryFormulasMap()
		{
			Table("salary_formulas");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			References(x => x.Service).Column("id_service").Not.Nullable();
			Map(x => x.Formula).Column("formula");
			Map(x => x.Comment).Column("comment");


		}
	}
}
