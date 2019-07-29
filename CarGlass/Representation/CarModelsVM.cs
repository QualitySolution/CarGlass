using CarGlass.Domain;
using Gamma.ColumnConfig;
using Gamma.GtkWidgets;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QSOrmProject.RepresentationModel;

namespace CarGlass.Representation
{
	public class CarModelsVM : RepresentationModelEntityBase<CarModel, CarModelsVMNode>
	{
		public CarBrand OnlyBrand { get; set; }

		#region IRepresentationModel implementation

		public override void UpdateNodes()
		{
			CarModelsVMNode resultAlias = null;
			CarBrand brandAlias = null;
			CarModel modelAlias = null;

			var modelsQuery = UoW.Session.QueryOver<CarModel>(() => modelAlias);
			if (OnlyBrand != null)
				modelsQuery.Where(() => modelAlias.Brand == OnlyBrand);

			var modelsList = modelsQuery
				.JoinAlias(m => m.Brand, () => brandAlias)
				.SelectList(list => list
				   .Select(() => modelAlias.Id).WithAlias(() => resultAlias.Id)
				            .Select(() => modelAlias.Name).WithAlias(() => resultAlias.ModelName)
				            .Select(() => brandAlias.Name).WithAlias(() => resultAlias.Brand)
				)
				.TransformUsing(Transformers.AliasToBean<CarModelsVMNode>())
				.List<CarModelsVMNode>();

			SetItemsSource(modelsList);
		}

		IColumnsConfig treeViewConfig = ColumnsConfigFactory.Create<CarModelsVMNode>()
		                                                    .AddColumn("Марка").AddTextRenderer(node => node.Brand)
		                                                    .AddColumn("Модель").AddTextRenderer(node => node.ModelName)
															.Finish();

		public override IColumnsConfig ColumnsConfig {
			get { return treeViewConfig; }
		}

		#endregion

		#region implemented abstract members of RepresentationModelEntityBase

		protected override bool NeedUpdateFunc(CarModel updatedSubject)
		{
			return true;
		}

		#endregion

		public CarModelsVM() : this(UnitOfWorkFactory.CreateWithoutRoot ())
		{
			//CreateRepresentationFilter = () => new EmployeeFilter(UoW);
		}

		public CarModelsVM(IUnitOfWork uow) : base ()
		{
			this.UoW = uow;
		}
	}

	public class CarModelsVMNode
	{
		public int Id { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string ModelName { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string Brand { get; set; }
	}
}
