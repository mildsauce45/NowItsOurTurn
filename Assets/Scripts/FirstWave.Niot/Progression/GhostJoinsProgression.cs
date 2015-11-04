using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Managers;
using FirstWave.Niot.Managers;

namespace FirstWave.Niot.Progression
{
	public class GhostJoinsProgression : DialogProgression
	{
		protected override void OnPostDialog()
		{
			var ghost = new Player("", 20);

			ghost.Class = "Sorcerer";
			ghost.Level = 1;
			ghost.Exp = 0;

			ghost.Speed = 6;
			ghost.Strength = 2;
			ghost.Will = 8;
			ghost.Endurance = 5;

			ghost.Weapon = WeaponManager.Instance.GetWeapon(3);

			GameStateManager.Instance.GameData.Party[2] = ghost;
		}
	}
}
