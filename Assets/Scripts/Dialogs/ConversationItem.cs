using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public abstract class ConversationItem {

	private static int CURRENT_ORDINAL = 0;

	public string Text { get; protected set; }
	public string Branch { get; protected set; }

	public int Ordinal { get; private set; }

	public ConversationItem() {
		Ordinal = CURRENT_ORDINAL++;
	}

	protected void SetBranch(XmlNode node) {
		var branchNode = node.Attributes.GetNamedItem("branch");
		if (branchNode != null)
			Branch = branchNode.Value.Trim();
	}
}

public class LineItem : ConversationItem {

	public static ConversationItem FromXml(XmlNode node) {

		var newItem = new LineItem();
		newItem.Text = node.InnerText.Trim();

		newItem.SetBranch(node);

		return newItem;
	}
}

public class Option {
	public string Text { get; private set; }
	public string ConfirmBranch { get; private set; }

	public Option(string text, string branch) {
		this.Text = text;
		this.ConfirmBranch = branch;
	}
}
public class ChoiceItem : ConversationItem {
	public Option[] Options { get; private set; }

	public static ConversationItem FromXml(XmlNode node) {

		var newItem = new ChoiceItem();
		newItem.Text = node.FirstChild.Value.Trim();

		var options = new List<Option>();

		var optionNodes = node.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "option");

		foreach (XmlNode on in optionNodes) {
			var text = on.InnerText.Trim();
			var branch = on.Attributes.GetNamedItem("confirm-branch").Value.Trim();

			options.Add(new Option(text, branch));
		}

		newItem.SetBranch(node);

		newItem.Options = options.ToArray();

		return newItem;
	}
}
