namespace CarGlass.Models.SMS
{
	public class SendResult
	{
		public string Status;
		public string MessageId;
		public string RusStatus
		{
			get
			{
				switch(Status)
				{
					case "accepted": return "Сообщение принято сервисом";
					case "invalid mobile phone": return "Неверно задан номер тефона (формат +71234567890)";
					case "text is empty": return "Отсутствует текст";
					case "sender address invalid": return "Неверная (незарегистрированная) подпись отправителя";
					case "wapurl invalid": return "Неправильный формат wap-push ссылки";
					case "invalid schedule time format": return "Неверный формат даты отложенной отправки сообщения";
					case "invalid status queue name": return "Неверное название очереди статусов сообщений";
					case "not enough balance": return "Баланс пуст (проверьте баланс)";
					default: return "Неизвестная ошибка: " + Status;
				}
			}
		}

		public bool HasError => Status != "accepted";
	}
}
