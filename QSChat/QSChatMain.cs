using System;
using System.Collections.Generic;
using Gtk;

namespace QSChat
{
	internal static class QSChatMain
	{
		private static Dictionary<string, string> usersTags = new Dictionary<string, string>();

		private static Dictionary<string, string> tagColors = new Dictionary<string, string>{
			{"user1","#FF00FF"},
			{"user2","#9400D3"},
			{"user3","#191970"},
			{"user4", "#7F0000"},
			{"user5", "#FF8C00"},
			{"user6", "#FFA500"},
			{"user7", "#32CD32"},
			{"user8", "#3CB371"},
			{"user9", "#007F00"},
			{"user10", "#FFFF00"}
		};

		public static TextTagTable BuildTagTable()
		{
			TextTagTable textTags = new TextTagTable();
			var tag = new TextTag("date");
			tag.Justification = Justification.Center;
			tag.Weight = Pango.Weight.Bold;
			textTags.Add(tag);

			foreach(var tagPair in tagColors)
			{
				tag = new TextTag(tagPair.Key);
				tag.Foreground = tagPair.Value;
				textTags.Add(tag);
			}
			return textTags;
		}

		public static string GetUserTag(string userName)
		{
			if (usersTags.ContainsKey(userName))
				return usersTags[userName];
			else
			{
				string tagName = String.Format("user{0}", usersTags.Count % 10 + 1);
				usersTags.Add(userName, tagName);
				return tagName;
			}
		}

		public static string GetUserColor(string userName)
		{
			return tagColors[GetUserTag(userName)];
		}
	}
}

