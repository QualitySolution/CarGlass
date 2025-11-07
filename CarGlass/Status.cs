using System;
using System.Collections.Generic;
using System.Linq;
using CarGlass.Domain;
using NLog;
using QS.DomainModel.UoW;

namespace CarGlass
{
	public partial class Status : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		Domain.Status status = new Domain.Status();
		public bool NewItem;

		public Status()
		{
			this.Build();
			foreach( OrderTypeClass orderTypeClass in UoW.Query<OrderTypeClass>().List())
				checklistTypes.AddCheckButton(
					orderTypeClass.Id.ToString(), orderTypeClass.Name);

		}

		public void Fill(int id)
		{
			NewItem = false;

			logger.Info("Запрос статуса №{0}...", id);
			status = UoW.Query<Domain.Status>().List().First(x => x.Id == id);

            labelId.Text = status.Id.ToString();
            entryName.Text = status.Name;
			checkColor.Active = !String.IsNullOrEmpty(status.Color);

            if(!String.IsNullOrEmpty(status.Color))
            {
                Gdk.Color TempColor = new Gdk.Color();
				Gdk.Color.Parse(status.Color, ref TempColor);
                colorbuttonMarker.Color = TempColor;
            }

            //Читаем лист типов заказов
            string[] types = status.Usedtypes.Split(new char[] { ',' });
            foreach(string ordertype in types)
            {
                if(checklistTypes.CheckButtons.ContainsKey(ordertype))
                    checklistTypes.CheckButtons[ordertype].Active = true;
            }

            logger.Info("Ok");
				this.Title = entryName.Text;

			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			buttonOk.Sensitive = Nameok;
		}

		protected void OnCheckColorClicked(object sender, EventArgs e)
		{
			colorbuttonMarker.Sensitive = checkColor.Active;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			status.Name = entryName.Text;
            if(checkColor.Active)
            {
                Gdk.Color c = colorbuttonMarker.Color;
                string ColorStr = String.Format("#{0:x4}{1:x4}{2:x4}", c.Red, c.Green, c.Blue);
                logger.Debug(ColorStr);
                status.Color = ColorStr;

            }
            status.Usedtypes = "";
            foreach(KeyValuePair<string, Gtk.CheckButton> Pair in checklistTypes.CheckButtons)
            {
                if(Pair.Value.Active)
                    status.Usedtypes += Pair.Key + ",";
            }
            UoW.Save(status);
            UoW.Commit();
            Respond(Gtk.ResponseType.Ok);

        }

        protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

	}
}