using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstWave.Xml
{
	public abstract class CharParser<TInput> : BasicParser<TInput>
	{
		/// <summary>
		/// Implemented as abstract because while we know we always want to produce characters as output
		/// We don't "really" know what type of input we may want to feed this thing. This allows us to build
		/// additional parsers based on this parser without tying us down to a specific input type just yet
		/// </summary>
		public abstract Parser<TInput, char> AnyChar { get; }

		/// <summary>
		/// The basic character parser. Will produce a result (of the given character) if the input is the character
		/// </summary>
		public Parser<TInput, char> Char(char ch)
		{
			return from c in AnyChar
				   where c == ch
				   select c;
		}

		/// <summary>
		/// Another basic character parser. Allows us to produce a result if we match the given predicate. This is generalization
		/// of the above parser.
		/// </summary>
		public Parser<TInput, char> Char(Predicate<char> predicate)
		{
			return from c in AnyChar
				   where predicate(c)
				   select c;
		}

		/// <summary>
		/// Returns a parse that will succeed if the given character is a letter
		/// </summary>
		public Parser<TInput, char> Letter()
		{
			return Char(char.IsLetter);
		}

		public Parser<TInput, char> LetterOrDigit()
		{
			return Char(char.IsLetterOrDigit);
		}

		public Parser<TInput, char> IsWhitespace()
		{
			return Char(char.IsWhiteSpace);
		}

		public Parser<TInput, char> NotWhitespace()
		{
			return Char(c => !char.IsWhiteSpace(c));
		}

		public Parser<TInput, char> In(IEnumerable<char> valid)
		{
			return Char(c => valid.Contains(c));
		}
	}
}
