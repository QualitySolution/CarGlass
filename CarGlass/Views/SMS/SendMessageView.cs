using System;
using CarGlass.ViewModels.SMS;
using QS.Views.Dialog;

namespace CarGlass.Views.SMS
{
	public partial class SendMessageView : DialogViewBase<SendMessageViewModel>
	{
		public SendMessageView(SendMessageViewModel viewModel) : base(viewModel)
		{
			this.Build();

			textMessage.Binding.AddBinding(ViewModel, v => v.MessageText, w => w.Buffer.Text).InitializeFromSource();
			buttonSend.Binding.AddBinding(ViewModel, v => v.SendSensetive, w => w.Sensitive).InitializeFromSource();
		}

		protected void OnButtonSendClicked(object sender, EventArgs e)
		{
			ViewModel.Send();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			ViewModel.Cancel();
		}
	}
}
