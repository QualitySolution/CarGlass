using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarGlass.Domain;
using CarGlass.Repository;
using Gamma.Utilities;
using Gtk;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QS.Dialog.GtkUI;
using QS.DomainModel.UoW;
using QS.Utilities;
using QS.Utilities.Numeric;

namespace CarGlass.Dialogs
{
	public partial class ExportExcelDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		//Это по сути разовый режим работы. оставлен на случай вдруг пригодится.
		private bool updatePhoneFormat = false;

		public virtual IUnitOfWork UoW { get; protected set; }

		public ExportExcelDlg()
		{
			this.Build();
			UoW = UnitOfWorkFactory.CreateWithoutRoot();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			#region Выбираем файл
			var fileChooser = new Gtk.FileChooserDialog(
					"Сохранить как...",
					(Window)this.Toplevel,
					Gtk.FileChooserAction.Save, 
					"Отмена", ResponseType.Cancel,
					"Сохранить", ResponseType.Accept);

			Gtk.FileFilter excel2007 = new Gtk.FileFilter();
			excel2007.AddPattern("*.xlsx");
			excel2007.Name = ".xlsx (Excel 2007)";

			//Gtk.FileFilter excel2003 = new Gtk.FileFilter();
			//excel2003.AddPattern("*.xls");
			//excel2003.Name = ".xls (Excel 2003)";

			fileChooser.AddFilter(excel2007);
			//fileChooser.AddFilter(excel2003);

			string filename;
			bool result = fileChooser.Run() == (int)ResponseType.Accept;
			filename = fileChooser.Filename;
			fileChooser.Destroy();
			if (!result)
				return;

			#endregion
			#region Загружаем заказы

			buttonOk.Sensitive = buttonCancel.Sensitive = false;
			progressbar1.Text = "Загрузка списка заказов...";
			progressbar1.Adjustment.Upper = 1;
			GtkHelper.WaitRedraw();

			if (!filename.EndsWith(".xlsx") )//&& !filename.EndsWith(".xls"))
				filename += ".xlsx";

			var orders = WorkOrderRepository.GetOrders(UoW, daterange.StartDateOrNull, daterange.EndDateOrNull);
			logger.Info($"Загружено {orders.Count} заказов.");
			progressbar1.Adjustment.Upper = orders.Count + 2;

			progressbar1.Text = "Формирование файла...";
			GtkHelper.WaitRedraw();

			#endregion
			#region Базовые настройки

			var workbook = new XSSFWorkbook();
			var sheet = workbook.CreateSheet("Заказы");

			//Заголовок
			var headerStyle = workbook.CreateCellStyle();
			var headerFont = workbook.CreateFont();
			headerFont.FontName = "Calibri";
			headerFont.FontHeightInPoints = 11;
			headerFont.IsBold = true;
			headerStyle.SetFont(headerFont);

			var newDataFormat = workbook.CreateDataFormat();
			var dateCellStyle = workbook.CreateCellStyle();
			dateCellStyle.DataFormat = newDataFormat.GetFormat("dd.MM.yyyy");

			//Ширина измеряется в количестве симвовлов * 256
			sheet.SetColumnWidth(0, 7 * 256);
			sheet.SetColumnWidth(1, 14 * 256);
			sheet.SetColumnWidth(2, 16 * 256);
			sheet.SetColumnWidth(3, 5 * 256);
			sheet.SetColumnWidth(4, 20 * 256);
			sheet.SetColumnWidth(5, 11 * 256);
			sheet.SetColumnWidth(6, 14 * 256);
			sheet.SetColumnWidth(7, 15 * 256);
			sheet.SetColumnWidth(8, 6 * 256);
			sheet.SetColumnWidth(9, 20 * 256);
			sheet.SetColumnWidth(10, 12 * 256);
			sheet.SetColumnWidth(11, 15 * 256);
			sheet.SetColumnWidth(12, 10 * 256);

			#endregion
			#region параметры экспорта

			string[] columnTiles = new string[] {
				"Номер",
				"Марка",
				"Модель",
				"Год",
				"Еврокод",
				"Производитель",
				"Склад",
				"Телефон",
				"Сумма работ",
				"Коментарий",
				"Состояние заказа",
				"Вид работ",
				"Дата работы"
			};

			Action<WorkOrder, ICell>[] SetValuesFuncs = new Action<WorkOrder, ICell>[]
			{
			(o, c) => c.SetCellValue(o.Id),
			(o, c) => c.SetCellValue(o.CarModel?.Brand?.Name),
			(o, c) => c.SetCellValue(o.CarModel?.Name),
			(o, c) => c.SetCellValue(o.CarYearText),
			(o, c) => c.SetCellValue(o.Eurocode),
			(o, c) => c.SetCellValue(o.Manufacturer?.Name),
			(o, c) => c.SetCellValue(o.Stock?.Name),
			(o, c) => c.SetCellValue(o.Phone),
			(o, c) => c.SetCellValue((double)o.Pays.Sum(x => x.Cost)),
			(o, c) => c.SetCellValue(o.Comment),
			(o, c) => c.SetCellValue(o.OrderState?.Name),
			(o, c) => c.SetCellValue(o.OrderTypeClass?.Name),
			(o, c) => {
					c.SetCellValue(o.Date);
					c.CellStyle = dateCellStyle;
				},
			};

			#endregion

			var headerRow = sheet.CreateRow(0);
			for (var i = 0; i < columnTiles.Length; i++)
			{
				var cell = headerRow.CreateCell(i);
				cell.SetCellValue(columnTiles[i]);
				cell.CellStyle = headerStyle;
			}

			for (var row = 1; row <= orders.Count; row++)
			{
				progressbar1.Text = $"Заказ {row} из {orders.Count}";
				progressbar1.Adjustment.Value++;
				GtkHelper.WaitRedraw();

				var dataRow = sheet.CreateRow(row);
				for (var i = 0; i < columnTiles.Length; i++)
				{
					var cell = dataRow.CreateCell(i);
					SetValuesFuncs[i](orders[row-1], cell);
				}
			}

			if(updatePhoneFormat)
				PhoneUpdate(workbook, orders);

			progressbar1.Text = "Записываем фаил...";
			progressbar1.Adjustment.Value++;
			GtkHelper.WaitRedraw();

			try
			{
				using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					workbook.Write(file);
				}
			}
			catch (IOException ex)
			{
				if (ex.HResult == -2147024864)
				{
					MessageDialogHelper.RunErrorDialog("Указанный файл уже открыт в другом приложении. Оно заблокировало доступ к файлу.");
					return;
				}
				throw ex;
			}
			progressbar1.Text = "Готово";
			progressbar1.Adjustment.Value++;
			GtkHelper.WaitRedraw();

			if (checkOpenAfterSave.Active)
			{
				progressbar1.Text = "Открываем Excel";
				GtkHelper.WaitRedraw();
				logger.Info("Открываем во внешем приложении...");
				System.Diagnostics.Process.Start(filename);
			}
			logger.Info("Ок");
			Respond(ResponseType.Ok);
		}

		void PhoneUpdate(XSSFWorkbook workbook, IList<WorkOrder> orders)
		{
			progressbar1.Adjustment.Value = 1;
			var sheet = workbook.CreateSheet("Телефоны");

			//Заголовок
			var headerStyle = workbook.CreateCellStyle();
			var headerFont = workbook.CreateFont();
			headerFont.FontName = "Calibri";
			headerFont.FontHeightInPoints = 11;
			headerFont.IsBold = true;
			headerStyle.SetFont(headerFont);

			var newDataFormat = workbook.CreateDataFormat();
			var dateCellStyle = workbook.CreateCellStyle();
			dateCellStyle.DataFormat = newDataFormat.GetFormat("dd.MM.yyyy");

			//Ширина измеряется в количестве симвовлов * 256
			sheet.SetColumnWidth(0, 20 * 256);
			sheet.SetColumnWidth(1, 20 * 256);
			sheet.SetColumnWidth(2, 20 * 256);

			#region параметры экспорта

			string[] columnTiles = new string[] {
				"Старый",
				"Новый",
				"Действие",
			};

			#endregion

			var headerRow = sheet.CreateRow(0);
			for(var i = 0; i < columnTiles.Length; i++)
			{
				var cell = headerRow.CreateCell(i);
				cell.SetCellValue(columnTiles[i]);
				cell.CellStyle = headerStyle;
			}

			var PhoneFormatter = new PhoneFormatter(PhoneFormat.RussiaOnlyHyphenated);

			for(var row = 1; row <= orders.Count; row++)
			{
				progressbar1.Text = $"Телефон {row} из {orders.Count}";
				progressbar1.Adjustment.Value++;
				GtkHelper.WaitRedraw();

				var order = orders[row-1];
				var dataRow = sheet.CreateRow(row);
				var cellOld = dataRow.CreateCell(0);
				var cellNew = dataRow.CreateCell(1);
				var cellAction = dataRow.CreateCell(2);
				cellOld.SetCellValue(order.Phone);
				var formated = order.Phone != null ? PhoneFormatter.FormatString(order.Phone) : order.Phone;
				if(formated == order.Phone)
				{
					cellAction.SetCellValue("Без изменений");
				}
				else if(formated.Length == 16)
				{
					cellAction.SetCellValue("Отформатирован");
					order.Phone = formated;
					UoW.Save(order);
				}
				else
					cellAction.SetCellValue("Пропущен");

				cellNew.SetCellValue(order.Phone);
				logger.Debug($"{row} - {order.Id}");
			}
			logger.Debug("Комит в базу");
			UoW.Commit();
		}
	}
}
