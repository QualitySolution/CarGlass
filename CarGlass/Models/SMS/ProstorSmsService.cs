using System;
using System.Collections.Generic;
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

		public StatusResult[] GetStatuses(string[] messageIds)
		{
			List<StatusResult> result = new List<StatusResult>();
			List<string> toSubmit = new List<string>();
			foreach(string id in messageIds)
			{
				toSubmit.Add(id);
				if(toSubmit.Count == 200)
				{
					result.AddRange(RunStatusQuery(toSubmit));
					toSubmit.Clear();
				}
			}
			if(toSubmit.Count > 0)
				result.AddRange(RunStatusQuery(toSubmit));
			return result.ToArray();
		}

		/// <summary>
		/// Отправка запроса максимум 200 id.
		/// </summary>
		private List<StatusResult> RunStatusQuery(List<string> messageIds)
		{
			var list = new List<StatusResult>();
			var request = new RestRequest("status/", Method.GET);
			foreach(var id in messageIds)
				request.AddQueryParameter("id", id);

			var response = client.Get(request);
			var result = response.Content;
			var statuses = result.Split('\n');

			foreach(var statusString in statuses)
			{
				var parts = statusString.Split(';');
				list.Add(new StatusResult { MessageId = parts[0], Status = parts[1] });
			}

			return list;
		}
	}
}
