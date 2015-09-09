using System.Linq;

namespace FirstWave.Xml
{
	public abstract class BasicParser<TInput>
	{
		/// <summary>
		/// This parser will always succeed without consuming any of the input
		/// </summary>
		public Parser<TInput, TValue> Success<TValue>(TValue value)
		{
			return input => new Result<TInput, TValue>(value, input);
		}

		/// <summary>
		/// Applies a parser repeatedly. This parser need not succeed.
		/// </summary>		
		public Parser<TInput, TValue[]> Repeat<TValue>(Parser<TInput, TValue> parser)
		{
			return RepeatMustSucceed(parser).Or(Success(new TValue[0]));
		}

		/// <summary>
		/// Applies a parser repeatedly. We select out the resulting successes
		/// </summary>		
		public Parser<TInput, TValue[]> RepeatMustSucceed<TValue>(Parser<TInput, TValue> parser)
		{
			return from p in parser
				   from ps in Repeat(parser)
				   select (new[] { p }).Concat(ps).ToArray();
		}

		/// <summary>
		/// Applies a parser that will always succeed, however if it supplied parser does not succeed, no input is consumed
		/// </summary>		
		public Parser<TInput, TValue> ZeroOrMore<TInput, TValue>(Parser<TInput, TValue> parser)
		{
			return input =>
			{
				var result = parser(input);

				return result ?? new Result<TInput, TValue>(default(TValue), input);
			};
		}
	}
}
