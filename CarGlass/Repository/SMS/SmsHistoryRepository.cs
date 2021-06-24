using System;
using CarGlass.Domain.SMS;
using NHibernate.Criterion;
using QS.DomainModel.UoW;

namespace CarGlass.Repository.SMS
{
	public class SmsHistoryRepository
	{
		public int MessageCount(IUnitOfWork uow, int orderId)
		{
			return uow.Session.QueryOver<SentMessage>()
				.Where(x => x.WorkOrder.Id == orderId)
				.Select(Projections.RowCount())
				.SingleOrDefault<int>();
		}
	}
}
