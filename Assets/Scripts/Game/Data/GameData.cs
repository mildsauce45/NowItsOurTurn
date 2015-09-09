using FirstWave.Niot.Game.Managers;
using UnityEngine;

namespace FirstWave.Niot.Game.Data
{
	public class GameData
	{
		public Player[] Party { get; private set; }

		public int Gold { get; set; }

		public string Scene { get; set; }
		public Vector2 Location { get; set; }

		public GameData()
		{
			Party = new Player[3];

			var leader = new Player("Demon Lord", 20);
			leader.Class = "Demon Lord";
			leader.Speed = 6;
			leader.Strength = 5;
			leader.Will = 6;
			leader.Endurance = 4;

			leader.Weapon = WeaponManager.Instance.GetWeapon(1);

			Party[0] = leader;
		}
	}
}
