using CarGlass.Domain;
using Gamma.ColumnConfig;
using Gamma.GtkWidgets;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QSOrmProject.RepresentationModel;

namespace CarGlass.Representation
{
	public class StoreItemsVM : RepresentationModelEntityBase<StoreItem, StoreItemsVMNode>
	{
		CarWindow onlyItemType;

		public CarWindow OnlyItemType
		{
			get
			{
				return onlyItemType;
			}

			set
			{
				onlyItemType = value;
				UpdateNodes();
			}
		}

		#region IRepresentationModel implementation

		public override void UpdateNodes()
		{
			StoreItemsVMNode resultAlias = null;
			CarBrand brandAlias = null;
			CarModel modelAlias = null;
			GlassManufacturer manufacturerAlias = null;
			StoreItem storeItemAlias = null;

			var itemsQuery = UoW.Session.QueryOver<StoreItem>(() => storeItemAlias);
			if (OnlyItemType != null)
				itemsQuery.Where(() => storeItemAlias.CarWindow == OnlyItemType);

			var itemsList = itemsQuery
				.JoinAlias(m => m.CarBrand, () => brandAlias)
				.JoinAlias(x => x.CarModel, () => modelAlias)
				.JoinAlias(x => x.Manufacturer, () => manufacturerAlias)
				.SelectList(list => list
				   .Select(() => storeItemAlias.Id).WithAlias(() => resultAlias.Id)
				            .Select(() => storeItemAlias.EuroCode).WithAlias(() => resultAlias.EuroCode)
				            .Select(() => modelAlias.Name).WithAlias(() => resultAlias.Model)
				            .Select(() => brandAlias.Name).WithAlias(() => resultAlias.Brand)
				            .Select(() => manufacturerAlias.Name).WithAlias(() => resultAlias.Manufacturer)
				            .Select(() => storeItemAlias.Cost).WithAlias(() => resultAlias.Cost)
				            .Select(() => storeItemAlias.Amount).WithAlias(() => resultAlias.Amount)
				            .Select(() => storeItemAlias.Placement).WithAlias(() => resultAlias.Placement)
				            .Select(() => storeItemAlias.Comment).WithAlias(() => resultAlias.Comment)
				)
				.OrderBy(() => storeItemAlias.EuroCode).Asc
				.TransformUsing(Transformers.AliasToBean<StoreItemsVMNode>())
				.List<StoreItemsVMNode>();

			SetItemsSource(itemsList);
		}

		IColumnsConfig treeViewConfig = ColumnsConfigFactory.Create<StoreItemsVMNode>()
															.AddColumn("Еврокод").AddTextRenderer(x => x.EuroCode)
						  //.AddColumn("Стекло").Tag(StoreColumns.Glass).AddTextRenderer(x => x.CarWindow != null ? x.CarWindow.Name : String.Empty)
		                                                    .AddColumn("Автомобиль").AddTextRenderer(x => x.ModelFull)
		                                                    .AddColumn("Производитель").AddTextRenderer(x => x.Manufacturer)
						  .AddColumn("Цена").AddNumericRenderer(x => x.Cost)
						  .AddColumn("Наличие").AddNumericRenderer(x => x.Amount)
						  .AddColumn("Место").AddTextRenderer(x => x.Placement)
						  .AddColumn("Комментарий").AddTextRenderer(x => x.Comment)
						  .Finish();

		public override IColumnsConfig ColumnsConfig {
			get { return treeViewConfig; }
		}

		#endregion

		#region implemented abstract members of RepresentationModelEntityBase

		protected override bool NeedUpdateFunc(StoreItem updatedSubject)
		{
			return true;
		}

		#endregion

		public StoreItemsVM() : this(UnitOfWorkFactory.CreateWithoutRoot ())
		{
			//CreateRepresentationFilter = () => new EmployeeFilter(UoW);
		}

		public StoreItemsVM(IUnitOfWork uow) : base ()
		{
			this.UoW = uow;
		}
	}

	public class StoreItemsVMNode
	{
		public int Id { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string EuroCode { get; set; }

		public string Brand { get; set; }

		public string Model { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string ModelFull { get { return $"{Brand} {Model}"; } }

		[UseForSearch]
		[SearchHighlight]
		public string Manufacturer { get; set; }

		public decimal Cost { get; set; }

		public int Amount { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string Placement { get; set; }

		[UseForSearch]
		[SearchHighlight]
		public string Comment { get; set; }
	}
}
