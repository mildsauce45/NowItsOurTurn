using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class ConversationManager : MonoBehaviour
{

	private IDictionary<string, Conversation> allConversations;

	// Use this for initialization
	void Start()
	{
		allConversations = new Dictionary<string, Conversation>();
	}

	public Conversation GetConversation(string conversationName)
	{
		if (allConversations != null && allConversations.ContainsKey(conversationName))
			return allConversations[conversationName];

		return null;
	}

	private void InitializeConversations()
	{
		var conversationAsset = Resources.Load("Conversations") as TextAsset;

		var doc = new XmlDocument();
		doc.LoadXml(conversationAsset.text);

		var conversationNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "conversation");

		foreach (var conversationXml in conversationNodes)
		{
			var lines = new List<ConversationItem>();

			// TODO: Support speakers in conversations
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
