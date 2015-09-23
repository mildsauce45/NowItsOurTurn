using System.Linq;
using FirstWave.Xml.Documents;

namespace FirstWave.Xml
{
	public class XmlParser : CharParser<string>
	{
		#region Input Type Any Char Implementation

		/// <summary>
		/// In order to parse any char from a string input we first check to see if we have input, and if so our result object
		/// has a value of the first character and contains the rest of the string
		/// </summary>
		public override Parser<string, char> AnyChar
		{
			get { return input => input != null && input.Length > 0 ? new Result<string, char>(input[0], input.Substring(1)) : null; }
		}

		#endregion

		#region Helper Parsers

		private readonly Parser<string, char[]> Whitespace;
		private readonly Parser<string, char> Lt;
		private readonly Parser<string, char> Gt;
		private readonly Parser<string, char> Eq;
		private readonly Parser<string, char> Slash;
		private readonly Parser<string, string> Id;
		private readonly Parser<string, string> Value;
		private readonly Parser<string, Node> ShortNode;
		private readonly Parser<string, Node> ValueNode;
		private readonly Parser<string, Node> FullNode;
		private readonly Parser<string, char> Punctuation;

		private readonly Parser<string, char[]> XmlDeclaration;

		#endregion

		#region Public Parsers (public mostly for testing purposes)

		public readonly Parser<string, string> Tag;
		public readonly Parser<string, Attribute> Attr;
		public readonly Parser<string, Node> Node;
		public readonly Parser<string, Document> Doc;

		#endregion

		public XmlParser()
		{
			Whitespace = Repeat(IsWhitespace());

			// These are simple convenience parsers for special characters to shorten our lengthier parsers
			Lt = Char('<');
			Gt = Char('>');
			Eq = Char('=');
			Slash = Char('/');

			// Slightly more complex parser but still dealing with individual characters
			Punctuation = Char('.').Or(Char('?')).Or(Char('!')).Or(Char('{')).Or(Char('}')).Or(Char('"'));

			// A parser to check for the xml declaration usually found just before the root node of an xml document
			XmlDeclaration = from lws in Whitespace
							 from lt in Lt
							 from q1 in Char('?')
							 from cs in Repeat(AnyChar.Where(c => c != '?'))
							 from q2 in Char('?')
							 from gt in Gt
							 select cs;

			// This is the parser for the xml node name. In this simple version we allow any name that contains only letters
			// Obviously this is incomplete and doesn't handle xml namespacing at all
			Id = from lws in Whitespace
				 from c in Letter()
				 from cs in Repeat(Letter())
				 select new string(new[] { c }.Concat(cs).ToArray());

			// The value of a node contains letters, digits, or whitespace. again this is incomplete as it should really allow for any
			// character that is between the > of the node declaration and the </ of the close of the node
			Value = from c in LetterOrDigit()
					from cs in Repeat(LetterOrDigit().Or(IsWhitespace()).Or(Punctuation))
					select new string(new[] { c }.Concat(cs).ToArray());

			// Parse out the beginning portion of a node declaration by combining the < parser with the Id parser. We'll only select out the
			// name of the node though. Throwing out the <
			Tag = from lws in Whitespace
				  from lt in Lt
				  from id in Id
				  select id;

			// An attribute is in the form of: name="value". On the surface this looks completely correct, but if you look at it more closely,
			// it uses the Id parser from above meaning that the value of the attribute cannot contain anything other that letters or whitespace
			// in a true parser this just doesnt cut it
			Attr = from lws in Whitespace
				   from id in Id
				   from eq in Eq
				   from oq in Char('"')
				   from value in Repeat(LetterOrDigit().Or(IsWhitespace()).Or(Char('.')).Or(Char(',')))
				   from cq in Char('"')
				   select new Attribute(id, new string(value));

			// Here I define a parser to detect a self closing node. i.e. <foo/>
			// I will also capture any attributes along the way.
			ShortNode = from lws in Whitespace
						from t in Tag
						from attrs in ZeroOrMore(Repeat(Attr))
						from s in Slash
						from gt in Gt
						from tws in Whitespace
						select new Node(t, attrs);

			// Here is a parser that dectects nodes that contains a value. i.e. <foo>value</foo>
			// The node may contain zero or more attributes.
			ValueNode = from lws in Whitespace
						from t in Tag
						from attrs in ZeroOrMore(Repeat(Attr))
						from lws2 in Whitespace
						from gt in Gt
						from v in Value
						from lt in Lt
						from s in Slash
						from id in Id
						where id == t
						from gt2 in Gt
						from tws in Whitespace
						select new Node(t, attrs, v);

			// Finally here is a parser that can detects nodes that contains zero or more child nodes.
			FullNode = from lws in Whitespace
					   from t in Tag
					   from attrs in ZeroOrMore(Repeat(Attr))
					   from foo in Whitespace
					   from gt in Gt
					   from lws2 in Whitespace
					   from ns in ZeroOrMore(Repeat(Node))
					   from tws in Whitespace
					   from lt in Lt
					   from s in Slash
					   from id in Id
					   where id == t
					   from gt2 in Gt
					   from tws2 in Whitespace
					   select new Node(t, attrs, null, ns);

			// With the 3 previous parsers defined, we can now say that a valid node is either self closing,
			// contains a value, or other child nodes
			Node = ShortNode.Or(ValueNode).Or(FullNode);

			// An parser for the entire xml document (note im not checking for the xml declaration yet)
			Doc = from lws in Whitespace
				  from doc in Node
				  select new Document(doc);
		}
	}
}
