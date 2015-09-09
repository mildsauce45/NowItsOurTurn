using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstWave.Xml
{
	public class Result<TInput, TValue>
	{
		public readonly TValue Value;
		public readonly TInput Rest;

		public Result(TValue value, TInput rest)
		{
			Value = value;
			Rest = rest;
		}
	}

	/// <summary>
	/// The parser is declared as a delegate because it can then support construction and assignment of new delegates
	/// using C#'s lambda notation for creation of anonymous delegates. This isn't possible with classes (as far as i know)
	/// </summary>	
	public delegate Result<TInput, TValue> Parser<TInput, TValue>(TInput input);
}
