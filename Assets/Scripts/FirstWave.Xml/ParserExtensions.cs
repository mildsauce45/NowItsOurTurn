using System;

namespace FirstWave.Xml
{
	public static class ParserExtensions
	{
		/// <summary>
		/// Applies the first parser. If the Result object is null, then it applies the second and returns its result instead
		/// </summary>		
		public static Parser<TInput, TValue> Or<TInput, TValue>(this Parser<TInput, TValue> firstParser, Parser<TInput, TValue> secondParser)
		{
			return input => firstParser(input) ?? secondParser(input);
		}

		/// <summary>
		/// Applies the first parser and then applies its Result object to the second parser, returning its Result object
		/// </summary>		
		public static Parser<TInput, TValue> And<TInput, TValue>(this Parser<TInput, TValue> firstParser, Parser<TInput, TValue> secondParser)
		{
			return input => secondParser(firstParser(input).Rest);
		}

		/// <summary>
		/// Applies the first parser, if the result comes back non-null (i.e. it succeeded) return null, other wise return success with the default
		/// value and the entirety of the input. NOTE: this method is potentially broken for value types as default(TValue) may return something valid
		/// </summary>		
		public static Parser<TInput, TValue> Not<TInput, TValue>(this Parser<TInput, TValue> parser)
		{
			return input =>
			{
				var result = parser(input);

				if (result.Value != null)
					return null;

				return new Result<TInput, TValue>(default(TValue), input);
			};
		}

		#region LINQ Operators

		/// <summary>
		/// Applies the parser, then checks to see if the result matches the given predicate. Implementing Where this way allows
		/// us to build additional parsers using the LINQ Where method in query syntax
		/// </summary>		
		public static Parser<TInput, TValue> Where<TInput, TValue>(this Parser<TInput, TValue> parser, Func<TValue, bool> predicate)
		{
			return input =>
			{
				var result = parser(input);

				if (result == null || !predicate(result.Value))
					return null;

				return result;
			};
		}

		/// <summary>
		/// Applies the parser, then constructs a new Result object by performing the projection function. Implementing Select this way allows
		/// us to build additional parsers using the LINQ Select method in query syntax
		/// </summary>		
		public static Parser<TInput, TProjection> Select<TInput, TValue, TProjection>(this Parser<TInput, TValue> parser, Func<TValue, TProjection> selector)
		{
			return input =>
			{
				var result = parser(input);

				if (result == null)
					return null;

				return new Result<TInput, TProjection>(selector(result.Value), result.Rest);
			};
		}

		/// <summary>
		/// Applies the parser, then maps the results of that to an intermediate projection given by the selector. The results of the intermediate projection are then mapped out into a new result object by applying the
		/// the projector. This function is equivalent to the Monadic operator Bind.
		/// </summary>		
		public static Parser<TInput, TProjection> SelectMany<TInput, TValue, TTemp, TProjection>(this Parser<TInput, TValue> parser, Func<TValue, Parser<TInput, TTemp>> selector, Func<TValue, TTemp, TProjection> projector)
		{
			return input =>
			{
				var result = parser(input);

				if (result == null)
					return null;

				var initialValue = result.Value;

				var intermediateResult = selector(initialValue)(result.Rest);

				if (intermediateResult == null)
					return null;

				return new Result<TInput, TProjection>(projector(initialValue, intermediateResult.Value), intermediateResult.Rest);
			};
		}

		#endregion
	}
}
