using System;
using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class SettingsMap : ClassMap<Settings>
	{
		public SettingsMap()
		{
			Table("settings");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Parametr).Column("parametr");
			Map(x => x.Value).Column("value");
			Map(x => x.Description).Column("description");
			Map(x => x.DateEdit).Column("date_edit");
		}
	}
}
