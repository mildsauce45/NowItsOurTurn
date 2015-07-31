using System.Xml;

namespace FirstWave.Core.Extensions
{
    public static class XmlNodeExtensions
    {
        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            var attrNode = node.Attributes.GetNamedItem(attributeName);
            if (attrNode == null)
                return null;

            return attrNode.Value;
        }

		public static T GetEnumAttributeValue<T>(this XmlNode node, string attributeName, T defaultValue) where T : struct
		{
			var value = GetAttributeValue(node, attributeName);

			if (!string.IsNullOrEmpty(value))
			{
				var @enum = value.EnumFromString<T>();

				return @enum ?? defaultValue;
			}
			else
				return defaultValue;
		}
    }
}

