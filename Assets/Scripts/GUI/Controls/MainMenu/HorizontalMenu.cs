using FirstWave.Core.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FirstWave.Core.GUI.Menus;

namespace Assets.Scripts.GUI.Controls.MainMenu
{
	public class HorizontalMenu : MonoBehaviour
	{
		#region Inspector Properties

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
		public float itemPadding = 20f;

		#endregion

		public event EventHandler Canceled;
		public event Menu.SelectionChangedHandler SelectionChanged;

		public int SelectedIndex
		{
			get { return currentOption; }
		}

		private InputTimer inputTimer;

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
					var left = InputManager.Instance.HorizontalAxisLeft();
					var right = InputManager.Instance.HorizontalAxisRight();

					if (left)
						SelectPreviousItem();
					else if (right)
						SelectNextItem();

					if (left || right)
						inputTimer.Reset();
				}
			}

			inputTimer.Update();
		}

		void OnGUI()
		{
			if (menuItems == null || !menuItems.Any())
				return;

			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			var xLoc = GetXLocation();
			var yLoc = GetYLocation();
			var width = GetWidth();

			BorderBox.Draw(new Rect(xLoc, yLoc, width, size.y), textures);
			
			float itemY = yLoc + padding.top;

			float sizeOfNames = GetTotalMenuItemSize(style);
			float itemX = xLoc + (width / 2) - (sizeOfNames / 2) - ((itemPadding * (menuItems.Count - 1)) / 2);

			int count = 0;

			foreach (var mi in menuItems)
			{
				var itemContent = new GUIContent(mi.Text);
				var textSize = style.CalcSize(itemContent);

				UnityEngine.GUI.Label(new Rect(itemX, itemY, textSize.x, textSize.y), itemContent, style);

				if (count == currentOption)
					UnityEngine.GUI.DrawTexture(new Rect(itemX - textures.Pointer.width - 10f, itemY, textures.Pointer.width, textures.Pointer.height), textures.Pointer);

				itemX += (textSize.x + itemPadding);
				count++;
			}
		}

		private float GetXLocation()
		{
			if (horizontalAlignment != HorizontalAlignment.Right)
				return margin.left;

			return Screen.width - margin.right - size.x;
		}

		private float GetYLocation()
		{
			if (verticalAlignment == VerticalAlignment.Top)
				return margin.top;
			else if (verticalAlignment == VerticalAlignment.Bottom)
				return Screen.height - margin.bottom - size.y;

			return (Screen.height / 2) - (size.y / 2);
		}

		private float GetWidth()
		{
			if (horizontalAlignment == HorizontalAlignment.Stretch && size.x == 0)
				return Screen.width - GetXLocation() - margin.right;
			else if (horizontalAlignment == HorizontalAlignment.Left && size.x == 0)
				return Screen.width - margin.left - margin.right;

			return size.x;
		}

		private float GetTotalMenuItemSize(GUIStyle style)
		{
			float size = 0;

			foreach (var mi in menuItems)
				size += style.CalcSize(new GUIContent(mi.Text)).x;
			
			return size;
		}

		#endregion

		#region Public API

		public void AddMenuItem(MenuItem menuItem)
		{
			if (menuItems == null)
				menuItems = new List<MenuItem>();

			menuItems.Add(menuItem);

			if (currentOption < 0)
			{
				currentOption = 0;
				SafeRaiseSelectionChanged();
			}
		}

		public int GetIndex(MenuItem menuItem)
		{
			return menuItems.IndexOf(menuItem);
		}

		#endregion

		#region Private Methods

		private void SelectPreviousItem()
		{
			if (currentOption == 0)
				currentOption = menuItems.Count - 1;
			else
				currentOption--;

			SafeRaiseSelectionChanged();
		}

		private void SelectNextItem()
		{
			currentOption = (currentOption + 1) % menuItems.Count;

			SafeRaiseSelectionChanged();
		}

		private void SafeRaiseSelectionChanged()
		{
			if (SelectionChanged != null && currentOption > -1 && currentOption < menuItems.Count)
				SelectionChanged(menuItems[currentOption]);
		}

		#endregion
	}
}
