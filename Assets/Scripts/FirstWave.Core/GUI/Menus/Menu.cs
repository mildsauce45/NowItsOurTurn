﻿using System;
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

		#region Events & Delegates

		public event EventHandler Canceled;

		public delegate void SelectionChangedHandler(MenuItem newSelection);
		public event SelectionChangedHandler SelectionChanged;

		#endregion

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
			if (menuItems == null || !menuItems.Any())
				return;

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
			{
				currentOption = 0;
				SafeRaiseSelectionChanged();
			}
		}

		public void SetSelectedIndex(int newIndex)
		{
			if (newIndex < 0 || newIndex > menuItems.Count)
				return;

			currentOption = newIndex;
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

			previous = true;
			next = false;

			SafeRaiseSelectionChanged();
		}

		private void SelectNextItem()
		{
			currentOption = (currentOption + 1) % menuItems.Count;

			previous = false;
			next = true;

			SafeRaiseSelectionChanged();
		}

		private float GetXLocation()
		{
			if (horizontalAlignment == HorizontalAlignment.Right)
				return Screen.width - margin.right - size.x;
			else if (horizontalAlignment == HorizontalAlignment.Left)
				return margin.left;
			else
				return (Screen.width / 2) - (size.x) / 2;
		}

		private float GetYLocation()
		{
			if (verticalAlignment == VerticalAlignment.Top)
				return margin.top;
			else if (verticalAlignment == VerticalAlignment.Bottom)
				return Screen.height - margin.bottom - size.y;
			else
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

		private void SafeRaiseSelectionChanged()
		{
			if (SelectionChanged != null && currentOption > -1 && currentOption < menuItems.Count)
				SelectionChanged(menuItems[currentOption]);
		}

		#endregion
	}
}
