using FirstWave.Unity.Gui.Enums;
using UnityEngine;

namespace FirstWave.Core.GUI
{
    public class MessageBox : MonoBehaviour
	{
		#region Designable Properties

		public string text;

		public BorderTextures textures;
		public FontProperties fontProperties;

		public Thickness margin;
		public Thickness padding;

		public float height;
		public float characterDelay = 0.0125f;

		public VerticalAlignment verticalAlignment;

		#endregion

		#region C# Properties

		public ITextSource textSource { get; set; }

		#endregion

		#region Unity Engine Methods

		void Awake()
		{
		}

		void Start()
		{
		}

		void OnGUI()
		{
			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			float msgBoxWidth = Screen.width - (margin.left + margin.right);

			BorderBox.Draw(new Rect(margin.left, GetYLocation(), msgBoxWidth, height), textures);

			var toDisplay = string.IsNullOrEmpty(text) && textSource != null ? textSource.Text : text;

			UnityEngine.GUI.Label(new Rect(margin.left + padding.left, GetYLocation() + padding.top, msgBoxWidth - (padding.left + padding.right), height - (padding.top + padding.bottom)),
								  toDisplay,
								  style);
		}

		#endregion

		private float GetYLocation()
		{
			if (verticalAlignment == VerticalAlignment.Top)
				return margin.top;
			else
				return Screen.height - margin.bottom - height;
		}
	}
}
