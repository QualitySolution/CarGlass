using System;
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
	}
}
