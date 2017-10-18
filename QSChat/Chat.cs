﻿using System;
using System.Collections.Generic;
using QSProjectsLib;
using MySql.Data.MySqlClient;
using NLog;
using Gtk;

namespace QSChat
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Chat : Gtk.Bin
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public UserInfo ChatUser;

		public event EventHandler ChatUpdated;

		private int ShowDays = 3;
		private int slowModeCountdown;
		private uint ActiveCount = 0;
		private DateTime? lastReadChat;
		private bool windowActive;
		private DateTime lastMessageTime;
		private TextTagTable textTags;
		private ChatHistory historyWindow;
		public int NewMessageCount;

		private bool isHided = false;
		public bool IsHided
		{
			get
			{
				return isHided;
			}
			set
			{
				if (isHided == value)
					return;
				isHided = value;
				if(isHided)
				{
					this.Hide();
				}
				else 
				{
					this.Show();
					NewMessageCount = 0;
					SetLastRead();
				}
			}
		}

		private bool active = false;
		public bool Active
		{
			get
			{
				return active;
			}
			set
			{
				if(active == false && value == true)
				{
					GLib.Timeout.Add(2000, new GLib.TimeoutHandler(OnUpdateTimer));

					var sql = "SELECT last_read_chat FROM users WHERE id = @id";
					MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)QSMain.ConnectionDB);
					cmd.Parameters.AddWithValue("@id", ChatUser.Id);
					var result = cmd.ExecuteScalar();
					if (result != DBNull.Value)
						lastReadChat = (DateTime)result;

					(Toplevel as Window).FocusInEvent += TopLevel_FocusInEvent;
					(Toplevel as Window).FocusOutEvent += Handle_FocusOutEvent;;
				}
				active = value;
			}
		}

		private bool slowMode
		{
			get
			{
				return (ActiveCount > 60);
			}
		}

		public Chat()
		{
			this.Build();
			textTags = QSChatMain.BuildTagTable();
		}

		private bool OnUpdateTimer()
		{
			if (!Active)
				return false;
			if(slowMode && slowModeCountdown > 0)
			{
				slowModeCountdown--;
				return true;
			}

			//Обновляем чат
			logger.Info("Обновляем чат...");
			if((QSMain.ConnectionDB as MySqlConnection).State != System.Data.ConnectionState.Open)
			{
				logger.Warn("Соедиение с сервером не открыто.");
				return true;
			}
			string sql = "SELECT datetime, text, users.name as user FROM chat_history " +
				"LEFT JOIN users ON users.id = chat_history.user_id " +
				"WHERE datetime > DATE_SUB(CURDATE(), INTERVAL " + ShowDays.ToString() +" DAY) " +
				"ORDER BY datetime";
			MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)QSMain.ConnectionDB);
			TextBuffer tempBuffer = new TextBuffer(textTags);
			TextIter iter = tempBuffer.EndIter;
			string lastSender = null, lastMessage = null;
			var oldNewMessageCount = NewMessageCount;
			NewMessageCount = 0;
			DateTime MaxDate = default(DateTime);
			using (MySqlDataReader rdr = cmd.ExecuteReader())
			{
				while (rdr.Read())
				{
					DateTime mesDate = rdr.GetDateTime("datetime");
					if (mesDate.Date != MaxDate.Date)
					{ 
						tempBuffer.InsertWithTagsByName(ref iter, String.Format("\n{0:D}", mesDate.Date), "date");
					}
					tempBuffer.InsertWithTagsByName(ref iter, string.Format("\n({0:t}) {1}: ", mesDate, rdr.GetString("user")), 
						QSChatMain.GetUserTag(rdr.GetString("user")));
					tempBuffer.Insert(ref iter, rdr.GetString("text"));

					lastSender = rdr.GetString("user");
					lastMessage = rdr.GetString("text");

					if (lastReadChat.HasValue && mesDate > lastReadChat)
						NewMessageCount++;
					if (mesDate > MaxDate)
						MaxDate = mesDate;
				}
			}
			if (ActiveCount > uint.MaxValue - 10)
				ActiveCount = 61;
			ActiveCount++;
			if(MaxDate > lastMessageTime)
			{
				ActiveCount = 0;
				textviewChat.Buffer = tempBuffer;
				//Сдвигаем скрол до конца
				TextIter ti = textviewChat.Buffer.GetIterAtLine(textviewChat.Buffer.LineCount-1);
				TextMark tm = textviewChat.Buffer.CreateMark("eot", ti,false);
				textviewChat.ScrollToMark(tm, 0, false, 0, 0); 
			}
			lastMessageTime = MaxDate;
			if (slowModeCountdown <= 0)
				slowModeCountdown = 8;
			logger.Info("Ок");
			if (ChatUpdated != null)
				ChatUpdated(this, EventArgs.Empty);

			if(NewMessageCount > oldNewMessageCount && (IsHided || !windowActive))
			{
				NewMessage.OpenChat = OnOpenChat;
				NewMessage.ShowMessage(lastSender, QSChatMain.GetUserColor(lastSender), lastMessage);
			}

			return true;
		}

		protected void OnButtonSendClicked(object sender, EventArgs e)
		{
			if (textviewMessege.Buffer.Text == "")
				return;

			logger.Info("Отправка сообщения...");
			if((QSMain.ConnectionDB as MySqlConnection).State != System.Data.ConnectionState.Open)
			{
				logger.Warn("Соедиение с сервером не открыто.");
				return;
			}
			string sql = "INSERT INTO chat_history (user_id, datetime, text) " +
				"VALUES(@user_id, NOW(), @text)";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)QSMain.ConnectionDB);
				cmd.Parameters.AddWithValue("@user_id", ChatUser.Id);
				cmd.Parameters.AddWithValue("@text", textviewMessege.Buffer.Text);
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Не удалось отправить сообщение.");
				return;
			}
			textviewMessege.Buffer.Text = "";
			ActiveCount = 0;
			OnUpdateTimer();
		}

		[GLib.ConnectBefore]
		protected void OnTextviewMessegeKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			if(args.Event.Key == Gdk.Key.Return && !args.Event.State.HasFlag(Gdk.ModifierType.ControlMask))
			{
				buttonSend.Click();
				args.RetVal = true;
			}
		}

		protected void OnButtonHistoryClicked(object sender, EventArgs e)
		{
			if(historyWindow == null)
			{
				historyWindow = new ChatHistory();
				historyWindow.Destroyed += OnHistoryDestroyed;
				historyWindow.Show();
			}
			else
			{
				historyWindow.Present();
			}
		}

		void OnHistoryDestroyed (object sender, EventArgs e)
		{
			historyWindow = null;
			logger.Debug("History Destroyed");
		}

		void OnOpenChat()
		{
			IsHided = false;
			(this.Toplevel as Window).Present();
		}

		void SetLastRead()
		{
			logger.Debug("Last read changed");
			var sql = "UPDATE users SET last_read_chat = NOW() WHERE id = @id;" +
				"SELECT last_read_chat FROM users WHERE id = @id";
			MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)QSMain.ConnectionDB);
			cmd.Parameters.AddWithValue("@id", ChatUser.Id);
			var result = cmd.ExecuteScalar();
			lastReadChat = (DateTime)result;
			OnUpdateTimer();
		}

		void TopLevel_FocusInEvent(object o, FocusInEventArgs args)
		{
			logger.Debug("Active");
			windowActive = true;
			if (!IsHided)
			{
				SetLastRead();
				NewMessage.HideIfActive();
			}
		}

		void Handle_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			windowActive = false;
			logger.Debug("Not Active");
		}
	}
}
