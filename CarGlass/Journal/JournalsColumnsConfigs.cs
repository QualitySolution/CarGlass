using System;
using CarGlass.Journal.ViewModels.SMS;
using Gamma.ColumnConfig;
using QS.Journal.GtkUI;

namespace CarGlass.Journal
{
	public static class JournalsColumnsConfigs
	{
		public static void RegisterColumns()
		{
			#region SMS

			TreeViewColumnsConfigFactory.Register<MessagesJournalViewModel>(
				() => FluentColumnsConfig<MessagesJournalNode>.Create()
					.AddColumn("Время отправки").AddTextRenderer(node => node.StatusTimeText)
					.AddColumn("Пользователь").AddTextRenderer(node => node.UserName)
					.AddColumn("Абонент").AddTextRenderer(node => node.Phone).SearchHighlight()
					.AddColumn("Сообщение").AddTextRenderer(x => x.Text).SearchHighlight()
					.AddColumn("Статус обновлен").AddTextRenderer(x => x.StatusTimeText)
					.AddColumn("Последний статус").AddTextRenderer(x => x.StatusRusText)
					.Finish()
			);

			#endregion
		}
	}
}
