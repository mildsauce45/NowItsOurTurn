
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

		public static float ToFloat(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return 0f;

			float result = 0;
			float.TryParse(s, out result);

			return result;
		}
	}
}
