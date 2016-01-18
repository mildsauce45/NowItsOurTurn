using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FirstWave.Core.GUI.Dialogs;
using UnityEngine;

namespace FirstWave.Niot.Managers
{
	public class ConversationManager
	{
		#region Singleton Implementation

		private static ConversationManager me;
		public static ConversationManager Instance
		{
			get
			{
				if (me == null)
					me = new ConversationManager();
				return me;
			}
		}

		private ConversationManager()
		{
		}

		#endregion

		private IDictionary<string, Conversation> allConversations;

		public Conversation GetConversation(string name)
		{
			if (allConversations == null)
				InitializeConversations();

			if (allConversations.ContainsKey(name))
				return allConversations[name];

			return null;
		}

		private void InitializeConversations()
		{
			allConversations = new Dictionary<string, Conversation>();

			var conversationAsset = Resources.Load("Conversations") as TextAsset;			

			var doc = new XmlDocument();
			doc.LoadXml(conversationAsset.text);

			var conversationNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(n => n.Name == "conversation");

			foreach (var conversationXml in conversationNodes)
			{
				var lines = new List<ConversationItem>();

				foreach (XmlNode line in conversationXml.ChildNodes)
				{
					if (line.Name == "line")
						lines.Add(LineItem.FromXml(line));
					else if (line.Name == "choice")
						lines.Add(ChoiceItem.FromXml(line));
				}

				var conversation = new Conversation(lines.ToArray());

				allConversations.Add(conversationXml.Attributes.GetNamedItem("name").Value, conversation);
			}
		}
	}
}
