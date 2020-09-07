using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using QS.DomainModel.Entity;

namespace CarGlass.Domain
{
	public class Settings : PropertyChangedBase, IDomainObject
	{
		public virtual int Id { get; set; }

		private string parametr;
		[Display(Name = "Название")]
		public virtual string Parametr
		{
			get { return parametr; }
			set { SetField(ref parametr, value); }
		}

		private string description;
		[Display(Name = "Описание")]
		public virtual string Description
		{
			get { return description; }
			set { SetField(ref description, value); }
		}

		private string valueSettting;
		[Display(Name = "Значение")]
		public virtual string ValueSettting
		{
			get { return valueSettting; }
			set { SetField(ref valueSettting, value); }
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
