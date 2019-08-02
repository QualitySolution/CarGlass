using System.Collections.Generic;
using CarGlass.Domain;
using QS.DomainModel.UoW;
using NHibernate.Criterion;

namespace CarGlass.Repository
{
	public static class OrderStateRepository
	{
		public static IList<OrderState> GetStates(IUnitOfWork uow, OrderType orderType)
		{
			return uow.Session.QueryOver<OrderState>()
				.Where(x => x.UsedForTypesPlainText.IsLike(orderType.ToString(), MatchMode.Anywhere))
				.List();
		}
	}
}
