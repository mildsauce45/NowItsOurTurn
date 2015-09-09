using System.Collections.Generic;
using System.Linq;
using FirstWave.Core.Utilities;

namespace FirstWave.Xml.Documents
{
	public class Node
	{
		public readonly string Name;
		public readonly string Value;
		public readonly IEnumerable<Attribute> Attributes;
		public readonly IEnumerable<Node> Children;

		public Maybe<string> this[string key]
		{
			get
			{
				if (string.IsNullOrEmpty(key))
					return Maybe<string>.Nothing;

				var attr = (Attributes != null && Attributes.Any()) ? Attributes.FirstOrDefault(a => a.Name == key) : null;

				if (attr == null)
					return Maybe<string>.Nothing;

				return new Maybe<string>(attr.Value);
			}
		}

		public Node(string name, IEnumerable<Attribute> attributes, string value = null, IEnumerable<Node> children = null)
		{
			Name = name;
			Attributes = attributes;
			Value = value;
			Children = children;
		}
	}
}
