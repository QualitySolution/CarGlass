using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using QS.DomainModel.UoW;
using QS.Utilities.Numeric;

namespace CarGlass.Models.SMS
{
	public class OrderMessagesModel
	{
		private readonly IUnitOfWorkFactory unitOfWorkFactory;
		private readonly WorkOrder workOrder;

		private readonly PhoneFormatter phoneFormatter = new PhoneFormatter(PhoneFormat.RussiaOnlyShort);

		public OrderMessagesModel(IUnitOfWorkFactory unitOfWorkFactory, WorkOrder workOrder)
		{
			this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
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


	}
}
