using System;

namespace FirstWave.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T? EnumFromString<T>(this string s) where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ApplicationException(string.Format("Given type {0} is not an enum", type));

            var obj = Enum.Parse(type, s);
            if (obj != null)
                return (T)obj;

            return null;
        }
    }
}
