using UnityEngine;
using System.Collections;

namespace FirstWave.Core.GUI
{
	public class GUIManager
	{
		public static GUIStyle GetMessageBoxStyle(FontProperties fontProperties)
		{
			var style = new GUIStyle();

			if (fontProperties == null)
				style.normal.textColor = Color.white;
			else
			{
				style.normal.textColor = fontProperties.fontColor;
				style.font = fontProperties.font;
				style.fontSize = fontProperties.fontSize;
			}

			return style;
		}
	}
}
