using System;
using FirstWave.Core.GUI.Menus;
using FirstWave.Niot.Game;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class AbilityMenuItem : MenuItem
	{
		public Ability Ability { get; private set; }

		public bool IsSelected { get; set; }

		public AbilityMenuItem(Ability ability, Action clickAction)
			: base(ability.Name, clickAction)
		{
			this.Ability = ability;
		}
	}
}
