using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Extensions.Observable.Collections.List;
using QS.Project.Domain;

namespace CarGlass.Domain
{
	[Appellative(Nominative = "заказ", Prepositional = "заказе")]
	public class WorkOrder : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		#region Свойства

		private DateTime date;

		[Display(Name = "Дата")]
		public virtual DateTime Date
		{
			get { return date; }
			set { SetField(ref date, value); }
		}

		private ushort hour;

		[Display(Name = "Час")]
		public virtual ushort Hour
		{
			get { return hour; }
			set { SetField(ref hour, value); }
		}

		private ushort pointNumber;

		[Display(Name = "Точка")]
		public virtual ushort PointNumber
		{
			get { return pointNumber; }
			set { SetField(ref pointNumber, value); }
		}

		private ushort calendarNumber;

		[Display(Name = "Календарь")]
		public virtual ushort CalendarNumber
		{
			get { return calendarNumber; }
			set { SetField(ref calendarNumber, value); }
		}

		private DateTime? createdDate = DateTime.Now;

		[Display(Name = "Время создания")]
		public virtual DateTime? CreatedDate
		{
			get { return createdDate; }
			set { SetField(ref createdDate, value); }
		}

		private UserBase createdBy;

		[Display(Name = "Автор")]
		public virtual UserBase CreatedBy
		{
			get { return createdBy; }
			set { SetField(ref createdBy, value); }
		}

		private CarModel carModel;

		[Display(Name = "Модель автомобиля")]
		public virtual CarModel CarModel
		{
			get { return carModel; }
			set { SetField(ref carModel, value); }
		}

		private ushort? carYear;

		[Display(Name = "Год автомобиля")]
		[Range(1901, 2155, ErrorMessage = "Год автомобиля должен быть в диапазоне от 1901 до 2155")]
		public virtual ushort? CarYear
		{
			get { return carYear; }
			set { SetField(ref carYear, value); }
		}

		public virtual string CarYearText
		{
			get => String.Format("{0}", CarYear);
			set
			{
				ushort val;
				if (!String.IsNullOrWhiteSpace(value) && ushort.TryParse(value, out val))
					CarYear = val;
				else
					CarYear = null;
			}
		}

		private string phone;

		[Display(Name = "Телефон")]
		public virtual string Phone
		{
			get { return phone; }
			set { SetField(ref phone, value); }
		}

		private OrderState orderState;

		[Display(Name = "Статус заказа")]
		public virtual OrderState OrderState
		{
			get { return orderState; }
			set { SetField(ref orderState, value); }
		}

		private Manufacturer manufacturer;

		[Display(Name = "Производитель")]
		public virtual Manufacturer Manufacturer
		{
			get { return manufacturer; }
			set { SetField(ref manufacturer, value); }
		}

		private Warehouse stock;

		[Display(Name = "Склад")]
		public virtual Warehouse Stock
		{
			get { return stock; }
			set { SetField(ref stock, value); }
		}

		private string eurocode;

		[Display(Name = "Еврокод")]
		public virtual string Eurocode
		{
			get { return eurocode; }
			set { SetField(ref eurocode, value); }
		}

		private string comment;

		[Display(Name = "Комментарий")]
		public virtual string Comment
		{
			get { return comment; }
			set { SetField(ref comment, value); }
		}

		private OrderTypeClass orderTypeClass;

		[Display(Name = "Тип заказа")]
		public virtual OrderTypeClass OrderTypeClass
		{
			get { return orderTypeClass; }
			set {SetField(ref orderTypeClass, value); }
		}

		#region Гарантия

		private Warranty warrantyInstall;

		[Display(Name = "Гарантия на установку")]
		public virtual Warranty WarrantyInstall
		{
			get { return warrantyInstall; }
			set { SetField(ref warrantyInstall, value); }
		}

		private Warranty warrantyTinting;

		[Display(Name = "Гарантия на тонировку")]
		public virtual Warranty WarrantyTinting
		{
			get { return warrantyTinting; }
			set { SetField(ref warrantyTinting, value); }
		}

		private Warranty warrantyArmoring;

		[Display(Name = "Гарантия на бронировку")]
		public virtual Warranty WarrantyArmoring
		{
			get { return warrantyArmoring; }
			set { SetField(ref warrantyArmoring, value); }
		}

		private Warranty warrantyPasting;

		[Display(Name = "Гарантия на оклейку")]
		public virtual Warranty WarrantyPasting
		{
			get { return warrantyPasting; }
			set { SetField(ref warrantyPasting, value); }
		}

		#endregion

		#endregion

		#region Коллекции
		private IObservableList<WorkOrderPay> pays = new ObservableList<WorkOrderPay>();
		[Display(Name = "список услуг с типами заказа, которым они принадлежат")]
		public virtual IObservableList<WorkOrderPay> Pays {
			get => pays;
			set => SetField(ref pays, value);
		}
		#endregion

		public WorkOrder()
		{
		}

	}
}
