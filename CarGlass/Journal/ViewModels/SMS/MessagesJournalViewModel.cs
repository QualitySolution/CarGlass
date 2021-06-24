using System;
using System.Linq;
using CarGlass.Domain;
using CarGlass.Domain.SMS;
using CarGlass.Models.SMS;
using NHibernate;
using NHibernate.Transform;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Project.Domain;
using QS.Project.Journal;
using QS.Project.Journal.DataLoader;
using QS.Services;
using QS.ViewModels.Extension;

namespace CarGlass.Journal.ViewModels.SMS
{
	public class MessagesJournalViewModel : JournalViewModelBase, IWindowDialogSettings
	{
		private int onlyOrderId;
		private readonly ProstorSmsService prostorSmsService;

		public MessagesJournalViewModel(IUnitOfWorkFactory unitOfWorkFactory, IInteractiveService interactiveService, INavigationManager navigation, ProstorSmsService prostorSmsService, WorkOrder workOrder = null) : base(unitOfWorkFactory, interactiveService, navigation)
		{
			this.prostorSmsService = prostorSmsService ?? throw new ArgumentNullException(nameof(prostorSmsService));

			if(workOrder != null && workOrder.Id > 0)
			{
				onlyOrderId = workOrder.Id;
				TabName = $"Cообщения по заказу №{workOrder.Id} на {workOrder.Date:D} в {workOrder.Hour} часов";
			}
			else
				TabName = "Отправленные сообщения";


			var dataLoader = new ThreadDataLoader<MessagesJournalNode>(unitOfWorkFactory);
			dataLoader.AddQuery(ItemsQuery);
			DataLoader = dataLoader;

			CreateActions();

			UpdateOnChanges(typeof(SentMessage));
		}

		protected IQueryOver<SentMessage> ItemsQuery(IUnitOfWork uow)
		{
			MessagesJournalNode resultAlias = null;
			UserBase userAlias = null;


			var query = uow.Session.QueryOver<SentMessage>();
			if(onlyOrderId > 0)
				query.Where(x => x.WorkOrder.Id == onlyOrderId);

			return	query.Left.JoinAlias(x => x.User, () => userAlias)
				.Where(GetSearchCriterion<SentMessage>(
					x => x.Phone,
					x => x.Text
					))
				.SelectList((list) => list
					.Select(x => x.Id).WithAlias(() => resultAlias.Id)
					.Select(() => userAlias.Name).WithAlias(() => resultAlias.UserName)
					.Select(x => x.SentTime).WithAlias(() => resultAlias.SentTime)
					.Select(x => x.Text).WithAlias(() => resultAlias.Text)
					.Select(x => x.Phone).WithAlias(() => resultAlias.Phone)
					.Select(x => x.LastStatusTime).WithAlias(() => resultAlias.StatusTime)
					.Select(x => x.LastStatus).WithAlias(() => resultAlias.LastStatus)
				)
				.OrderBy(x => x.SentTime).Desc
				.TransformUsing(Transformers.AliasToBean<MessagesJournalNode>());
		}

		#region Действия
		void CreateActions()
		{
			var updateStatusAction = new JournalAction("Запросить статус доставки",
					(selected) => selected.Any(),
					(selected) => true,
					(selected) => UpdateStatus(selected.Cast<MessagesJournalNode>().ToArray())
					);
			NodeActionsList.Add(updateStatusAction);
		}

		void UpdateStatus(MessagesJournalNode[] messagesNodes)
		{
			using(var uow = UnitOfWorkFactory.CreateWithoutRoot())
			{
				var messages = uow.GetById<SentMessage>(messagesNodes.Select(x => x.Id));
				var newStatuses = prostorSmsService.GetStatuses(messages.Select(x => x.MessageId).ToArray());
				foreach(var newStatus in newStatuses)
				{
					var message = messages.First(x => x.MessageId == newStatus.MessageId);
					message.LastStatus = newStatus.Status;
					message.LastStatusTime = DateTime.Now;
					uow.Save(message);
				}
				uow.Commit();
			}
			Refresh();
		}
		#endregion

		#region Жесткий хак для переходного периода при использовании старого диалога заказа, не надо для нормальной работы.
		bool IWindowDialogSettings.IsModal => onlyOrderId > 0;
		bool IWindowDialogSettings.EnableMinimizeMaximize => false;
		WindowGravity IWindowDialogSettings.WindowPosition => WindowGravity.Center;
		#endregion
	}

	public class MessagesJournalNode
	{
		#region из базы
		public int Id { get; set; }
		public string UserName { get; set; }
		public DateTime SentTime { get; set; }
		public string Text { get; set; }
		public string Phone { get; set; }
		public DateTime StatusTime { get; set; }
		public string LastStatus { get; set; }
		#endregion
		#region Расчетные
		public string SentTimeText => SentTime.ToString("g");
		public string StatusTimeText => StatusTime.ToString("g");
		public string StatusRusText => StatusResult.GetStatusRus(LastStatus);
		#endregion
	}
}
