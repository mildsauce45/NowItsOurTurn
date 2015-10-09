using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FirstWave.Xml.Documents;

public abstract class ConversationItem
{
	private static int CURRENT_ORDINAL = 0;

	public string Text { get; protected set; }
	public string Branch { get; protected set; }

	public int Ordinal { get; private set; }

	public ConversationItem()
	{
		Ordinal = CURRENT_ORDINAL++;
	}

	protected void SetBranch(XmlNode node)
	{
		var branchNode = node.Attributes.GetNamedItem("branch");
		if (branchNode != null)
			Branch = branchNode.Value.Trim();
	}
}

public class LineItem : ConversationItem
{
	public LineItem()
	{
	}

	public LineItem(string text)
	{
		Text = text;
	}

	public static ConversationItem FromXml(XmlNode node)
	{
		var newItem = new LineItem();
		newItem.Text = node.InnerText.Trim();

		newItem.SetBranch(node);

		return newItem;
	}
}

public class Option
{
	public string Text { get; private set; }
	public string ConfirmBranch { get; private set; }

	public Option(string text, string branch)
	{
		this.Text = text;
		this.ConfirmBranch = branch;
	}
}

public class ChoiceItem : ConversationItem
{
	public Option[] Options { get; private set; }

	public static ConversationItem FromXml(XmlNode node)
	{

		var newItem = new ChoiceItem();
		newItem.Text = node.Value.Trim();

		var options = new List<Option>();

		var optionNodes = node.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "option");

		foreach (var on in optionNodes)
		{
			var text = on.Value.Trim();
			var branch = on["confirm-branch"].Value;

			options.Add(new Option(text, branch));
		}

		newItem.SetBranch(node);

		newItem.Options = options.ToArray();

		return newItem;
	}
}
