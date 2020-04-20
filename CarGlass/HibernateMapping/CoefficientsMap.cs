using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;
namespace CarGlass.HibernateMapping
{
	public class CoefficientsMap : ClassMap<Coefficients>
	{
		public CoefficientsMap()
		{
			Table("coefficients");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");
			Map(x => x.Comment).Column("comment");
		}
	}
}
