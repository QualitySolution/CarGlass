﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CarGlass.Domain
{
	public enum OrderType
	{ 
		[Display(Name = "Установка стекл")]
		install,
		[Display(Name = "Тонировка")]
		tinting,
		[Display(Name = "Ремонт сколов")]
		repair,
		[Display(Name = "Полировка")]
		polishing,
		[Display(Name = "Бронировка")]
		armoring
	}
}