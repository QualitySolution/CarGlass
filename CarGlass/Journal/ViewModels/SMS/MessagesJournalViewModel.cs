using System;
using CarGlass.Domain.SMS;
using NHibernate;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Project.Domain;
using QS.Project.Journal;
using QS.Project.Journal.DataLoader;
using QS.Services;

namespace CarGlass.Journal.ViewModels.SMS
{
	public class MessagesJournalViewModel : JournalViewModelBase
	{
		public MessagesJournalViewModel(IUnitOfWorkFactory unitOfWorkFactory, IInteractiveService interactiveService, INavigationManager navigation) : base(unitOfWorkFactory, interactiveService, navigation)
		{
			TabName = TabName = "Отправленные сообщения";

			var dataLoader = new ThreadDataLoader<MessagesJournalNode>(unitOfWorkFactory);
			dataLoader.AddQuery(ItemsQuery);
			DataLoader = dataLoader;

			//CreateNodeActions();

			UpdateOnChanges(typeof(SentMessage));
		}

		protected IQueryOver<SentMessage> ItemsQuery(IUnitOfWork uow)
		{
			MessagesJournalNode resultAlias = null;
			UserBase userAlias = null;
			return uow.Session.QueryOver<SentMessage>()
				.Left.JoinAlias(x => x.User, () => userAlias)
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
		public string SentTimeText => SentTime.ToLongTimeString();
		public string StatusTimeText => StatusTime.ToLongDateString();
		#endregion
	}
}
