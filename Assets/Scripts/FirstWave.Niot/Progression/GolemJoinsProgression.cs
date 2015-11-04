using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Managers;
using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Niot.Progression
{
	public class GolemJoinsProgression : DialogProgression
	{
		protected override void OnPostDialog()
		{			
			var golem = new Player("Rocky", 25);

			golem.Class = "Elemental";
			golem.Level = 1;
			golem.Exp = 0;

			golem.Speed = 2;
			golem.Strength = 8;
			golem.Will = 3;
			golem.Endurance = 8;

			golem.Weapon = WeaponManager.Instance.GetWeapon(2);

			GameStateManager.Instance.GameData.Party[1] = golem;
		}
	}
}
