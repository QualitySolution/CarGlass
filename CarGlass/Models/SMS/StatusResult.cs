using System;
namespace CarGlass.Models.SMS
{
	public class StatusResult
	{
		public string Status;
		public string MessageId;

		public static string GetStatusRus(string status)
		{
			switch(status)
			{
				case "accepted": return "Сообщение принято сервисом";
				case "queued": return "Сообщение находится в очереди";
				case "delivered": return "Сообщение доставлено";
				case "delivery error": return "Ошибка доставки SMS (абонент в течение времени доставки находился вне зоны\nдействия сети или номер абонента заблокирован)";
				case "smsc submit": return "Сообщение доставлено в SMSC";
				case "smsc reject": return "Сообщение отвергнуто SMSC (номер заблокирован или не существует)";
				case "incorrect id": return "Неверный идентификатор сообщения";
				default: return "Неизвестный статус: " + status;
			}
		}
	}
}
