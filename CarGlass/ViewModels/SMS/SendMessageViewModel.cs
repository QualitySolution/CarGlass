using System;
using CarGlass.Models.SMS;
using QS.Dialog;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Validation;
using QS.ViewModels.Dialog;

namespace CarGlass.ViewModels.SMS
{
	public class SendMessageViewModel : WindowDialogViewModelBase
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private readonly IInteractiveMessage interactive;
		private readonly ProstorSmsService prostorSmsService;
		private readonly OrderMessagesModel orderMessages;

		public SendMessageViewModel(
			IUnitOfWorkFactory unitOfWorkFactory, 
			INavigationManager navigation,
			IInteractiveMessage interactive,
			ProstorSmsService prostorSmsService,
			OrderMessagesModel orderMessages,
			IValidator validator = null) : base(navigation)
		{
			this.interactive = interactive ?? throw new ArgumentNullException(nameof(interactive));
			this.prostorSmsService = prostorSmsService ?? throw new ArgumentNullException(nameof(prostorSmsService));
			this.orderMessages = orderMessages ?? throw new ArgumentNullException(nameof(orderMessages));
			IsModal = true;
			Title = "Отправка СМС";
			WindowPosition = QS.Dialog.WindowGravity.Center;
			MessageText = orderMessages.DefaultMessage;
		}

		#region Свойства View

		private string messageText;

		[PropertyChangedAlso(nameof(SendSensetive))]
		public virtual string MessageText
		{
			get => messageText;
			set => SetField(ref messageText, value);
		}

		#endregion

		#region Sensetive

		public bool SendSensetive => !String.IsNullOrWhiteSpace(MessageText) && !String.IsNullOrEmpty(orderMessages.CustomerPhone);

		#endregion

		#region Действия

		public void Send()
		{
			var result = prostorSmsService.SendMessage(orderMessages.CustomerPhone, MessageText);
			logger.Debug($"Send Result={result.Status}; Message Id={result.MessageId}");
			if(result.HasError)
			{
				interactive.ShowMessage(ImportanceLevel.Error, result.RusStatus);
				return;
			}
			Close(false, CloseSource.Save);
		}

		public void Cancel()
		{
			Close(false, CloseSource.Cancel);
		}

		#endregion
	}
}
