using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace CarGlass.Domain.SMS
{
	public class Message : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private WorkOrder workOrder;
		[Display(Name = "Заказ")]
		public virtual WorkOrder WorkOrder
		{
			get => workOrder;
			set => SetField(ref workOrder, value);
		}

		private string phone;
		[Display(Name = "Телефон")]
		public virtual string Phone
		{
			get => phone;
			set => SetField(ref phone, value);
		}

		private string messageId;
		[Display(Name = "ID Сообщения")]
		public virtual string MessageId
		{
			get => messageId;
			set => SetField(ref messageId, value);
		}

		private DateTime sentTime;
		[Display(Name = "Время отправки")]
		public virtual DateTime SentTime
		{
			get => sentTime;
			set => SetField(ref sentTime, value);
		}

		private string lastStatus;
		[Display(Name = "Последний состояние")]
		public virtual string LastStatus
		{
			get => lastStatus;
			set => SetField(ref lastStatus, value);
		}

		private DateTime? lastStatusTime;
		[Display(Name = "Время получения последнего состояния")]
		public virtual DateTime? LastStatusTime
		{
			get => lastStatusTime;
			set => SetField(ref lastStatusTime, value);
		}

		private string text;
		[Display(Name = "Сообщение")]
		public virtual string Text
		{
			get => text;
			set => SetField(ref text, value);
		}

	}
}
