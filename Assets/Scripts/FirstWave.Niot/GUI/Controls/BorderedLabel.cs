using UnityEngine;
using System.Collections;
using FirstWave.Core.GUI;

namespace FirstWave.Niot.GUI.Controls
{
	public class BorderedLabel : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		public string text;

		public VerticalAlignment verticalAlignment;
		public HorizontalAlignment horizontalAlignment;

		public TextAlignment textAlignment;

		public Vector2 size;
		public Vector2 position;

		public Thickness margin;
		public Thickness padding;

		void OnGUI()
		{
			BorderBox.Draw(new Rect(GetXLocation(), GetYLocation(), size.x, size.y), textures);

			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			var textContent = new GUIContent(text);
			var textSize = style.CalcSize(textContent);

			float labelX = 0;
			float labelY = 0;

			if (textAlignment == TextAlignment.Left)
			{
				labelX = GetXLocation() + padding.left + textures.BorderVertical.width;
				labelY = GetYLocation() + padding.top + textures.BorderVertical.height;
				textSize = new Vector2(size.x - padding.left - padding.right, size.y - padding.top - padding.bottom);
			}
			else if (textAlignment == TextAlignment.Center)
			{				
				labelX = GetXLocation() + padding.left + ((size.x / 2) - (textSize.x / 2));
				labelY = GetYLocation() + padding.top + ((size.y / 2) - (textSize.y / 2));				
			}

			UnityEngine.GUI.Label(new Rect(labelX, labelY, textSize.x, textSize.y), textContent, style);
		}

		private float GetXLocation()
		{
			if (horizontalAlignment == HorizontalAlignment.Left)
				return margin.left;
			else
				return Screen.width - margin.right - size.x;
		}

		private float GetYLocation()
		{
			if (verticalAlignment == VerticalAlignment.Top)
				return margin.top;
			else
				return Screen.height - margin.bottom - size.y;
		}
	}
}
