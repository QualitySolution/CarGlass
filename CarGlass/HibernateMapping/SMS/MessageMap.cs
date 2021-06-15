using System;
using CarGlass.Domain.SMS;
using FluentNHibernate.Mapping;

namespace CarGlass.HibernateMapping.SMS
{
	public class MessageMap : ClassMap<Message>
	{
		public MessageMap()
		{
			Table("sms_history");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Phone).Column("phone");
			Map(x => x.MessageId).Column("message_id");
			Map(x => x.SentTime).Column("sent_time");
			Map(x => x.LastStatus).Column("last_status");
			Map(x => x.LastStatusTime).Column("last_status_time");
			Map(x => x.Text).Column("text");

			References(x => x.WorkOrder).Column("order_id");
		}
	}
}
