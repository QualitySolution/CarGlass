using System;
using CarGlass.Domain;
using Gtk;
using QS.DomainModel.UoW;
using QSOrmProject;

namespace CarGlass.Dialogs
{
	public partial class NoteDlg : FakeTDIEntityGtkDialogBase<Note>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public NoteDlg()
		{
			this.Build();   
		}

		public NoteDlg(int id)
        {
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateForRoot<Note>(id);
			Configure();
		}

		public NoteDlg(Note note) : this(note.Id) { }

		public NoteDlg(ushort pointNumber, ushort calendarNumber, DateTime date)
		{
			this.Build();
			UoWGeneric = UnitOfWorkFactory.CreateWithNewRoot<Note>();
			Entity.PointNumber = pointNumber;
			Entity.CalendarNumber = calendarNumber;
			Entity.Date = date;
			Configure();
		}

		void Configure()
        {
			ydatepicker.Date = Entity.Date;
			textview.Buffer.Text = Entity.Message;
			this.Title = $"Заметка на {Entity.Date} число";
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
        {
			Entity.Date = ydatepicker.Date;
			Entity.Message = textview.Buffer.Text;

			Save();
		}

		public override bool Save()
		{
			UoW.Save();
			logger.Info("Save note on " + Entity.Date);
			Respond(ResponseType.Ok);
			return true;
		}
	}
}
