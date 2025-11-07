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
			Map(x => x.Parameter).Column("parametr");
			Map(x => x.ValueSetting).Column("value");
			Map(x => x.Description).Column("description");
			Map(x => x.DateEdit).Column("date_edit");
		}
	}
}
