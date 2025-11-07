using CarGlass.Domain;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping
{
	public class NoteMap : ClassMap<Note>
	{
		public NoteMap()
		{
			Table("note");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Date).Column("date");
			Map(x => x.PointNumber).Column("point_number");
			Map(x => x.CalendarNumber).Column("calendar_number");
			Map(x => x.Message).Column("message");
		}
	}
}
