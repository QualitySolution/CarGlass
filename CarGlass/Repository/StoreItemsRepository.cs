using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using NHibernate.Criterion;
using QS.DomainModel.UoW;

namespace CarGlass.Repository
{
	public class StoreItemsRepository
	{
		public IList<string> GetEurocodes(IUnitOfWork uow)
		{
			return uow.Session.QueryOver<StoreItem>()
				.Select(Projections.Distinct(Projections.Property<StoreItem>(x => x.EuroCode)))
				.List<string>();
		}

		public IList<StoreItem> GetSomeEurocodes(IUnitOfWork uow, string euroCode)
		{
			return uow.GetAll<StoreItem>().Where(x => x.EuroCode.Contains(euroCode)).ToList();
		}

		public IList<StoreItem> GetAllEurocodes(IUnitOfWork uow)
		{
			return uow.GetAll<StoreItem>().ToList();
		}
	}
}
