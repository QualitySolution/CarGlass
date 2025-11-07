using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Extensions.Observable.Collections.List;

namespace CarGlass.Domain
{
    public class OrderTypeClass : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название")]
		public virtual string Name
		{
			get { return name; }
			set { SetField(ref name, value); }
		}

		private string nameAccusative;
		[Display(Name = "Название Винительный падеж")]
		public virtual string NameAccusative
		{
			get => nameAccusative;
			set => SetField(ref nameAccusative, value);
		}

		private bool isCalculateSalary;

		[Display(Name = "Считать ли ЗП")]
		public virtual bool IsCalculateSalary
		{
			get { return isCalculateSalary; }
			set { SetField(ref isCalculateSalary, value); }
		}

		private string positionInTabs;

		[Display(Name = "На каких вкладках отображать тип заказа")]
		public virtual string PositionInTabs
		{
			get { return positionInTabs; }
			set { SetField(ref positionInTabs, value); }
		}

		private bool isShowMainWidgets;

		[Display(Name = "Показывать виджеты: состояние заказа, марка, модель, год, телефон")]
		public virtual bool IsShowMainWidgets
		{
			get { return isShowMainWidgets; }
			set { SetField(ref isShowMainWidgets, value); }
		}

		private bool isShowAdditionalWidgets;

		[Display(Name = "Показывать виджеты: производитель, склад, еврокод")]
		public virtual bool IsShowAdditionalWidgets
		{
			get { return isShowAdditionalWidgets; }
			set { SetField(ref isShowAdditionalWidgets, value); } 
		}

		private bool isInstallType;

		[Display(Name = "Тип заказа относится к установке ")]
		public virtual bool IsInstallType
		{
			get { return isInstallType; }
			set { SetField(ref isInstallType, value); }
		}

		private bool isOtherType;

		[Display(Name = "Тип заказа относится к прочим ")]
		public virtual bool IsOtherType
		{
			get { return isOtherType; }
			set { SetField(ref isOtherType, value); }
		}

		#region Коллекции
		private IObservableList<ServiceOrderType> serviceOrderTypes = new ObservableList<ServiceOrderType>();
		[Display(Name = "список услуг с типами заказа, которым они принадлежат")]
		public virtual IObservableList<ServiceOrderType> ServiceOrderTypes {
			get => serviceOrderTypes;
			set => SetField(ref serviceOrderTypes, value);
		}
		#endregion
	}
}
