using System;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Validation;
using QS.ViewModels.Dialog;

namespace CarGlass.ViewModels.SMS
{
	public class SendMessageViewModel : WindowDialogViewModelBase
	{
		public SendMessageViewModel(IUnitOfWorkFactory unitOfWorkFactory, INavigationManager navigation, IValidator validator = null) : base(navigation)
		{
			IsModal = true;
			Title = "Отправка СМС";
			WindowPosition = QS.Dialog.WindowGravity.Center;
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

		public bool SendSensetive => !String.IsNullOrWhiteSpace(MessageText);

		#endregion

		#region Действия

		public void Send()
		{

			Close(false, CloseSource.Save);
		}

		public void Cancel()
		{
			Close(false, CloseSource.Cancel);
		}

		#endregion
	}
}
