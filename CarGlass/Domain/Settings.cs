using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class Settings : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private string parameter;
		[Display(Name = "Название")]
		public virtual string Parameter
		{
			get { return parameter; }
			set { SetField(ref parameter, value); }
		}

		private string description;
		[Display(Name = "Описание")]
		public virtual string Description
		{
			get { return description; }
			set { SetField(ref description, value); }
		}

		private string valueSetting;
		[Display(Name = "Значение")]
		public virtual string ValueSetting
		{
			get { return valueSetting; }
			set { SetField(ref valueSetting, value); }
		}

		private DateTime dateEdit;
		[Display(Name = "Дата изменения")]
		public virtual DateTime DateEdit
		{
			get { return dateEdit; }
			set { SetField(ref dateEdit, value); }
		}

		public Settings()
		{
		}

	}
}
