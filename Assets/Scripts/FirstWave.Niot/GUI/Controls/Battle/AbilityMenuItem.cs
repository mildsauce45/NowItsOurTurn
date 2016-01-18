using System;
using FirstWave.Niot.Game;
using FirstWave.Unity.Gui.Controls;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class AbilityMenuItem : MenuItem
	{
		public Ability Ability { get; private set; }

		//public bool IsSelected { get; set; }

		public AbilityMenuItem(Ability ability, Action clickAction)
			: base(ability.Name, clickAction)
		{
			this.Ability = ability;
		}
	}
}
