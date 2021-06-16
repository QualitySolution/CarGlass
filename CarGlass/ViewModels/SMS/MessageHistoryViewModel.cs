using System;
using QS.Navigation;
using QS.ViewModels.Dialog;

namespace CarGlass.ViewModels.SMS
{
	public class MessageHistoryViewModel : WindowDialogViewModelBase
	{
		public MessageHistoryViewModel(INavigationManager navigation) : base(navigation)
		{
			IsModal = true;
		}
	}
}
