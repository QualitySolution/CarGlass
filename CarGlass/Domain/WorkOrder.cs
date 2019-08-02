using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;
using QS.Project.Domain;

namespace CarGlass.Domain
{
	public class WorkOrder : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

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

		private OrderType orderType;

		[Display(Name = "Тип заказа")]
		public virtual OrderType OrderType
		{
			get { return orderType; }
			set { SetField(ref orderType, value); }
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

		private DateTime? createdDate;

		[Display(Name = "Время создания")]
		public virtual DateTime? CreatedDate
		{
			get { return createdDate; }
			set { SetField(ref createdDate, value); }
		}

		private UserBase createdBy;

		[Display(Name = "notset")]
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

		private ushort carYear;

		[Display(Name = "Год автомобиля")]
		public virtual ushort CarYear
		{
			get { return carYear; }
			set { SetField(ref carYear, value); }
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

		private Stock stock;

		[Display(Name = "Склад")]
		public virtual Stock Stock
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

		public WorkOrder()
		{
		}

	}
}
