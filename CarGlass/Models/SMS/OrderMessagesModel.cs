using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using CarGlass.Domain.SMS;
using QS.DomainModel.UoW;
using QS.Services;
using QS.Utilities.Numeric;

namespace CarGlass.Models.SMS
{
	public class OrderMessagesModel
	{
		private readonly IUnitOfWorkFactory unitOfWorkFactory;
		private readonly IUserService userService;
		private readonly WorkOrder workOrder;

		private readonly PhoneFormatter phoneFormatter = new PhoneFormatter(PhoneFormat.RussiaOnlyShort);

		public OrderMessagesModel(IUnitOfWorkFactory unitOfWorkFactory, IUserService userService, WorkOrder workOrder)
		{
			this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
			this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.workOrder = workOrder;
		}

		public string CustomerPhone => String.IsNullOrWhiteSpace(workOrder.Phone) || workOrder.Phone.Length != 16 ? null
			: phoneFormatter.FormatString(workOrder.Phone);

		#region Сообщение
		public List<Service> SelectedServices;

		public string DefaultMessage => $"Вы записаны на {ServiceName} на {Date} в {Time} по адресу {Address}. Стоимость работ {Summa}";

		private string ServiceName => workOrder.OrderTypeClass.NameAccusative;
		private string Date => workOrder.Date.ToShortDateString();
		private string Time => $"{workOrder.Hour:D2}:00" ;
		private string Summa => $"{workOrder.Pays.Sum(x => x.Cost):N0} руб.";
		private string Address
		{
			get
			{
				switch(workOrder.PointNumber)
				{
					case 1:
						return "Вырицкое шоссе 1-Б";
					case 2:
						return "Ленинградское шоссе 15";
					default:
						return null;
				}
			}
		}
		#endregion

		#region Сохранение истории

		public void SaveSentMesage(string text, string messageId, string status)
		{
			using(var uow = unitOfWorkFactory.CreateWithNewRoot<SentMessage>())
			{
				var entity = uow.Root;
				entity.WorkOrder = workOrder;
				entity.Text = text;
				entity.MessageId = messageId;
				entity.Phone = CustomerPhone;
				entity.SentTime = DateTime.Now;
				entity.User = userService.GetCurrentUser(uow);
				entity.LastStatus = status;
				entity.LastStatusTime = DateTime.Now;

				uow.Save(entity);
			}
		}

		#endregion
	}
}
