using System;
using QS.BaseParameters;
using RestSharp;
using RestSharp.Authenticators;

namespace CarGlass.Models.SMS
{
	public class ProstorSmsService
	{
		readonly RestClient client;
		const string baseUrl = "http://api.prostor-sms.ru/messages/v2/";

		public ProstorSmsService(ParametersService parametersService)
		{
			string login = parametersService.Dynamic.ProstorSMSLogin;
			if(String.IsNullOrWhiteSpace(login))
				throw new Exception("Параметр базы ProstorSMSLogin должен быть указан.");

			string password = parametersService.Dynamic.ProstorSMSPassword;
			if(String.IsNullOrWhiteSpace(password))
				throw new Exception("Параметр базы ProstorSMSPassword должен быть указан.");

			client = new RestClient(baseUrl);
			client.Authenticator = new HttpBasicAuthenticator(login, password);
		}

		public SendResult SendMessage(string phone, string text)
		{
			var request = new RestRequest("send/", Method.GET);
			request.AddQueryParameter("phone", phone);
			request.AddQueryParameter("text", text);

			var response = client.Get(request);
			var result = response.Content;
			if(result.Contains(";"))
			{
				var parts = result.Split(';');
				return new SendResult { Status = parts[0], MessageId = parts[1] };
			}
			return new SendResult { Status = result };
		}
	}

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
					default: return "Не известная ошибка: " + Status;
				}
			}
		}
	}
}
