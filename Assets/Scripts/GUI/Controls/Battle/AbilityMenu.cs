using System;
using System.Linq;
using FirstWave.Core.GUI;
using FirstWave.Niot.Game;
using UnityEngine;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class AbilityMenu : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		public Vector2 position;
		public Vector2 size;

		public Thickness margin;
		public Thickness padding;

		public VerticalAlignment verticalAlignment;
		public HorizontalAlignment horizontalAlignment;		

		public float inputDelay = 0.125f;
		public float iconSize = 24;

		public event EventHandler Canceled;

		private InputTimer inputTimer;

		private AbilityMenuItem[] abilityArray;		

		public int CurrentIndex
		{
			get
			{
				for (int i = 0; i < abilityArray.Length; i++)
				{
					if (abilityArray[i] != null && abilityArray[i].IsSelected)
						return i;
				}

				return -1;
			}
		}

		public AbilityMenuItem CurrentItem
		{
			get { return abilityArray[CurrentIndex]; }
		}

		public void AddMenuItem(AbilityMenuItem menuItem)
		{
			if (menuItem == null || menuItem.Ability == null)
				return;

			if (abilityArray == null)
				abilityArray = new AbilityMenuItem[Constants.Ranges.STANDARD_ABILITY_SIZE + Constants.Ranges.FINISHER_ABILITY_SIZE];

			int beginningIndex = menuItem.Ability.IsFinisher ? 4 : 0;

			for (int i = beginningIndex; i < 6; i++)
			{
				if (abilityArray[i] == null)
				{
					abilityArray[i] = menuItem;
					break;
				}
			}

			if (!abilityArray.Where(a => a != null).Any(abi => abi.IsSelected))
				menuItem.IsSelected = true;			
		}

		public void Clear()
		{
			if (abilityArray != null)
			{
				for (int i = 0; i < abilityArray.Length; i++)
					abilityArray[i] = null;
			}
		}

		#region Input Methods

		private void MoveUp()
		{
			Func<int, int> func = i =>
			{
				if (i < 2)
					return 4 + i;
				else
					return i - 2;
			};

			Move(CurrentIndex, func);
		}

		private void MoveDown()
		{
			Func<int, int> func = i =>
			{
				if (i > 3)
					return i - 4;
				else
					return i + 2;
			};

			Move(CurrentIndex, func);
		}

		private void MoveLeft()
		{
			// Since we use a 2x3 grid, this is identical
			MoveRight();
		}

		private void MoveRight()
		{
			Func<int, int> func = i =>
			{
				if (i % 2 == 0)
					return i + 1;
				else
					return i - 1;
			};

			Move(CurrentIndex, func);
		}

		private void Move(int beginningIndex, Func<int, int> incrementor)
		{
			int newIndex = incrementor(beginningIndex);

			while (newIndex != beginningIndex && GetItemAt(newIndex) == null)
				newIndex = incrementor(newIndex);

			if (newIndex != beginningIndex)
			{
				GetItemAt(beginningIndex).IsSelected = false;
				GetItemAt(newIndex).IsSelected = true;
			}
		}

		private AbilityMenuItem GetItemAt(int index)
		{
			return abilityArray[index];
		}

		#endregion

		#region Unity Engine

		void Start()
		{
			inputTimer = new InputTimer(inputDelay);
			inputTimer.Start();
		}

		void Update()
		{
			if (inputTimer.IsTimeUp)
			{
				if (InputManager.Instance.KeyReleased("Cancel"))
				{
					if (Canceled != null)
						Canceled(this, EventArgs.Empty);

					inputTimer.Reset();
				}
				else if (InputManager.Instance.KeyReleased("Interact"))
				{
					if (CurrentItem != null && CurrentItem.Ability.CooldownRemaining <= 0)
						CurrentItem.ClickAction();
					else
					{
						/// TODO: Play a nope sound
					}

					inputTimer.Reset();
				}
				else
				{
					var up = InputManager.Instance.VerticalAxisUp();
					var down = InputManager.Instance.VerticalAxisDown();

					var left = InputManager.Instance.HorizontalAxisLeft();
					var right = InputManager.Instance.HorizontalAxisRight();

					if (up)
						MoveUp();
					else if (down)
						MoveDown();
					else if (left)
						MoveLeft();
					else if (right)
						MoveRight();

					if (up || down || left || right)
						inputTimer.Reset();
				}
			}

			inputTimer.Update();
		}

		void OnGUI()
		{
			var controlWidth = GetWidth();

			BorderBox.Draw(new Rect(GetXLocation(), GetYLocation(), controlWidth, size.y), textures);

			var menuItemWidth = (controlWidth / 2) - 10;
			var menuItemHeight = (size.y / 3) - 20;

			var leftX = GetXLocation() + 10;

			var x = leftX;
			var y = GetYLocation() + 10;

			for (int i = 0; i < abilityArray.Length; i++)
			{
				if (abilityArray[i] == null)
					continue;

				var ability = abilityArray[i].Ability;

				if (ability == null)
					continue;

				if (!ability.IsFinisher)
					DrawRegular(x, y, i, ability);
				else
					DrawFinisher(x, y, i, ability);

				if (i % 2 == 0)
					x += menuItemWidth + 10;
				else
				{
					x = leftX;
					y += menuItemHeight + 10;
				}
			}
		}

		private void DrawRegular(float x, float y, int i, Ability ability)
		{
			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			if (ability.Icon && ability.Icon.width > 0)
				UnityEngine.GUI.DrawTexture(new Rect(x, y, iconSize, iconSize), ability.Icon);

			string text = ability.Name;

			if (ability.CooldownRemaining > 0)
				text += "  " + ability.CooldownRemaining;

			var content = new GUIContent(text);
			var contentSize = style.CalcSize(content);

			UnityEngine.GUI.Label(new Rect(x + iconSize + 10, y, contentSize.x, contentSize.y), content, style);

			if (abilityArray[i].IsSelected)
				UnityEngine.GUI.DrawTexture(new Rect(x, y + iconSize + 2, iconSize + 10 + contentSize.x, 2), textures.Pointer);
		}

		private void DrawFinisher(float x, float y, int i, Ability ability)
		{
			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			if (ability.Icon && ability.Icon.width > 0)
				UnityEngine.GUI.DrawTexture(new Rect(x, y, iconSize, iconSize), ability.Icon);

			string text = ability.Name;

			var content = new GUIContent(text);
			var contentSize = style.CalcSize(content);

			UnityEngine.GUI.Label(new Rect(x + iconSize + 10, y, contentSize.x, contentSize.y), content, style);

			var pillX = x + iconSize + 10 + contentSize.x + 5;
			var pillScale = 0.66f;

			foreach (var cost in (ability as Finisher).ElementCost)
			{
				var texture = FieldEffectDisplay.Instance.GetTextureForEffect(cost.ElementType);

				var textureWidth = texture.width * pillScale;
				var textureHeight = texture.height * pillScale;

				for (int j = 0; j < cost.Amount; j++)
				{
					UnityEngine.GUI.DrawTexture(new Rect(pillX, y, textureWidth, textureHeight), texture);

					pillX += textureWidth + 5;
				}
			}

			if (abilityArray[i].IsSelected)
				UnityEngine.GUI.DrawTexture(new Rect(x, y + iconSize + 2, iconSize + 10 + contentSize.x, 2), textures.Pointer);
		}

		#endregion

		#region Layout Methods

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

		#endregion
	}
}
