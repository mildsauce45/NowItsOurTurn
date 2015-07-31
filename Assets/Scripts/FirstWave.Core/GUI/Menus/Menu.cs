using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirstWave.Core.GUI.Menus
{
	public class Menu : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		public Vector2 position;
		public Vector2 size;

		public Thickness margin;
		public Thickness padding;

		public VerticalAlignment verticalAlignment;
		public HorizontalAlignment horizontalAlignment;

		public bool DisableInput { get; set; }

		public float inputDelay = 0.125f;
		public float itemPadding = 10f;

		public event EventHandler Canceled;

		private InputTimer inputTimer;

		private bool previous;
		private bool next;

		private int currentOption = -1;
		private List<MenuItem> menuItems;

		#region Unity Engine

		void Start()
		{
			inputTimer = new InputTimer(inputDelay);
			inputTimer.Start();
		}

		void Update()
		{
			if (DisableInput)
				return;

			if (inputTimer.IsTimeUp)
			{
				if (InputManager.Instance.KeyReleased("Interact"))
				{
					menuItems[currentOption].ClickAction();

					inputTimer.Reset();
				}
				else if (InputManager.Instance.KeyReleased("Cancel"))
				{
					if (Canceled != null)
						Canceled(this, EventArgs.Empty);

					inputTimer.Reset();
				}
				else
				{
					var up = InputManager.Instance.VerticalAxisUp();
					var down = InputManager.Instance.VerticalAxisDown();

					if (up)
						SelectPreviousItem();
					else if (down)
						SelectNextItem();

					if (up || down)
						inputTimer.Reset();
				}
			}

			inputTimer.Update();
		}

		void OnGUI()
		{
			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			BorderBox.Draw(new Rect(GetXLocation(), GetYLocation(), GetWidth(), size.y), textures);

			var renderedMenuItems = this.menuItems.Where(m => m.ShouldRender).ToList();
			
			if (renderedMenuItems != null && renderedMenuItems.Count > 0)
			{
				float pointerX = GetXLocation() + padding.left;
				float itemX = pointerX + textures.Pointer.width + 10f;
				float y = GetYLocation() + padding.top;

				int count = 0;

				foreach (var mi in renderedMenuItems)
				{
					if (currentOption >= renderedMenuItems.Count)
						currentOption = next ? 0 : (previous ? renderedMenuItems.Count - 1 : currentOption);

					var itemContent = new GUIContent(mi.Text);
					var textSize = style.CalcSize(itemContent);

					if (count == currentOption)
						UnityEngine.GUI.DrawTexture(new Rect(pointerX, y + (Math.Abs(textSize.y - textures.Pointer.height) / 2), textures.Pointer.width, textures.Pointer.height), textures.Pointer);

					UnityEngine.GUI.Label(new Rect(itemX, y, size.x - (padding.left + padding.right + textures.Pointer.width), textSize.y), itemContent, style);

					y += (textSize.y + itemPadding);

					count++;
				}
			}
		}

		#endregion

		#region Public Methods

		public void AddMenuItem(MenuItem menuItem)
		{
			if (menuItems == null)
				menuItems = new List<MenuItem>();

			menuItems.Add(menuItem);

			if (currentOption < 0)
				currentOption = 0;
		}

		public void SetSelectedIndex(int newIndex)
		{
			if (newIndex < 0 || newIndex > menuItems.Count)
				return;

			currentOption = newIndex;
		}

		#endregion

		#region Private Methods

		private void SelectPreviousItem()
		{
			if (currentOption == 0)
				currentOption = menuItems.Count - 1;
			else
				currentOption--;

			previous = true;
			next = false;
		}

		private void SelectNextItem()
		{
			currentOption = (currentOption + 1) % menuItems.Count;

			previous = false;
			next = true;
		}

		private float GetXLocation()
		{
			if (horizontalAlignment == HorizontalAlignment.Right)
				return Screen.width - margin.right - size.x;
			else
				return margin.left;
		}

		private float GetYLocation()
		{
			if (verticalAlignment == VerticalAlignment.Top)
				return margin.top;
			else
				return Screen.height - margin.bottom - size.y;
		}

		private float GetWidth()
		{
			if (horizontalAlignment == HorizontalAlignment.Stretch && size.x == 0)
				return Screen.width - GetXLocation() - margin.right;

			return size.x;
		}

		#endregion
	}
}
