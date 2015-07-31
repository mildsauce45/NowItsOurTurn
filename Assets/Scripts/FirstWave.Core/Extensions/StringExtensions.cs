using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstWave.Core.Extensions
{
	public static class StringExtensions
	{
		public static int ToInt(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return 0;

			int result = 0;
			int.TryParse(s, out result);

			return result;
		}
	}
}
