using System;
using System.Collections.Generic;
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
	}
}
