using System.Collections.Generic;
using CarGlass.Domain;
using QS.DomainModel.UoW;
using NHibernate.Criterion;

namespace CarGlass.Repository
{
	public static class CarModelRepository
	{
		public static IList<CarModel> GetCarModels(IUnitOfWork uow, CarBrand brand)
		{
			return uow.Session.QueryOver<CarModel>()
				.Where(x => x.Brand == brand)
				.List();
		}
	}
}
