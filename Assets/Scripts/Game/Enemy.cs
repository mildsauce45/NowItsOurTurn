using UnityEngine;

namespace FirstWave.Niot.Game
{
	public class Enemy : Combatant
	{
		public Texture2D Sprite { get; set; }

		// Maybe don't need this, I could just actually use the sprite size
		public EnemySize Size { get; set; }

		public int Experience { get; set; }
		public int Gold { get; set; }

		public override int AttackPower
		{
			get { return this.Strength; }
		}

		public override int MagicPower
		{
			get { return this.Will; }
		}

		public override int Defense
		{
			get { return Endurance; }
		}

		public Enemy(string name, int maxHP)
		{
			Name = name;

			MaxHP = maxHP;
			CurrentHP = maxHP;
		}

		/// <summary>
		/// This will create a separate instance for use in battle
		/// </summary>		
		public Enemy(Enemy toClone)
			: this(toClone.Name, toClone.MaxHP)
		{
			this.Sprite = toClone.Sprite;

			this.Speed = toClone.Speed;
			this.Strength = toClone.Strength;
			this.Experience = toClone.Experience;
			this.Gold = toClone.Gold;
		}
	}
}