using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using NHibernate;
using QS.DomainModel.UoW;

namespace CarGlass.Repository
{
	public static class WorkOrderRepository
	{
		public static IList<WorkOrder> GetOrders(IUnitOfWork uow, DateTime? startDate, DateTime? endDate)
		{
			WorkOrderPay workOrderPayAlias = null;

			var query = CreateBaseQuery(uow, startDate, endDate)
				.Fetch(SelectMode.Fetch, x => x.CarModel)
				.Fetch(SelectMode.Fetch, x => x.CarModel.Brand)
				.Fetch(SelectMode.Fetch, x => x.Stock)
				.Fetch(SelectMode.Fetch, x => x.Manufacturer)
				.Fetch(SelectMode.Fetch, x => x.OrderState)
				.Future();

			CreateBaseQuery(uow, startDate, endDate)
				.Left.JoinAlias(x => x.Pays, () => workOrderPayAlias)
				.Fetch(SelectMode.Fetch, x => x.Pays)
				.Future();

			return query.ToList();
		}

		private static IQueryOver<WorkOrder, WorkOrder> CreateBaseQuery(IUnitOfWork uow, DateTime? startDate, DateTime? endDate)
		{
			var query = uow.Session.QueryOver<WorkOrder>();
			if (startDate.HasValue)
				query.Where(x => x.Date >= startDate.Value.Date);
			if (endDate.HasValue)
				query.Where(x => x.Date < endDate.Value.Date);

			query.JoinQueryOver(x => x.OrderTypeClass).Where(x => x.IsOtherType == false);

			return query;
		}

		public static IList<WorkOrder> GetOrdersByPhone(IUnitOfWork uow, string phone, int excludeId)
		{
			WorkOrderPay workOrderPayAlias = null;

			var query = uow.Session.QueryOver<WorkOrder>()
				.Where(x => x.Phone == phone)
				.Where(x => x.Id != excludeId)
				.OrderBy(x => x.Date).Desc
				.Fetch(SelectMode.Fetch, x => x.CarModel)
				.Fetch(SelectMode.Fetch, x => x.CarModel.Brand)
				//.Fetch(SelectMode.Fetch, x => x.Stock)
				.Fetch(SelectMode.Fetch, x => x.Manufacturer)
				.Fetch(SelectMode.Fetch, x => x.OrderState)
				.Future();

			uow.Session.QueryOver<WorkOrder>()
				.Where(x => x.Phone == phone)
				.Where(x => x.Id != excludeId)
				.OrderBy(x => x.Date).Desc
				.Left.JoinAlias(x => x.Pays, () => workOrderPayAlias)
				.Fetch(SelectMode.Fetch, x => x.Pays)
				.Future();

			return query.ToList();
		}
	}
}
