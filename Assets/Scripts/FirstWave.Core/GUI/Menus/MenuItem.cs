using System;

namespace FirstWave.Core.GUI.Menus
{
	public class MenuItem
	{
		public string Text { get; private set; }
		public Action ClickAction { get; private set; }

		public bool ShouldRender { get; set; }

		public MenuItem(string text, Action clickAction)
		{
			this.Text = text;
			this.ClickAction = clickAction;

			ShouldRender = true;
		}
	}
}
