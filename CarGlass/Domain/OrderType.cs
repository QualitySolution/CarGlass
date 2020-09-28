using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
    //public enum OrderType
    //{
    //    [Display(Name = "Установка стекл")]
    //    install,
    //    [Display(Name = "Тонировка")]
    //    tinting,
    //    [Display(Name = "Ремонт сколов")]
    //    repair,
    //    [Display(Name = "Полировка")]
    //    polishing,
    //    [Display(Name = "Бронировка")]
    //    armoring,
    //    [Display(Name = "Прочее")]
    //    other
    //}

    //public class OrderTypeStringType : NHibernate.Type.EnumStringType
    //{
    //    public OrderTypeStringType() : base(typeof(OrderType)) { }
    //}

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

		IList<ServiceOrderType> listServiceOrderType = new List<ServiceOrderType>();
		[Display(Name = "список услуг с типами заказа, которым они принадлежат")]
		public virtual IList<ServiceOrderType> ListServiceOrderType
		{
			get => listServiceOrderType;
			set => SetField(ref listServiceOrderType, value);
		}

		GenericObservableList<ServiceOrderType> observableServiceOrderType;
		//FIXME Костыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<ServiceOrderType> ObservableServiceOrderType
		{
			get
			{
				if(observableServiceOrderType == null)
					observableServiceOrderType = new GenericObservableList<ServiceOrderType>(ListServiceOrderType);
				return observableServiceOrderType;
			}
		}

		#endregion
	}
}
