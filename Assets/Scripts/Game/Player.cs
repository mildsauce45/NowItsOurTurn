namespace FirstWave.Niot.Game
{
	public class Player : Combatant
	{
		public string Class { get; set; }

		public Weapon Weapon { get; set; }

		public Armor Head { get; set; }
		public Armor Chestpiece { get; set; }

		public Ability[] AllAbilities { get; set; }

		public override int AttackPower
		{
			get { return Strength + (Weapon != null ? Weapon.Power : 0); }
		}

		public override int MagicPower
		{
			get { return Will + (Weapon != null ? Weapon.MagicPower : 0);}
		}

		public override int Defense
		{
			get
			{
				var result = Endurance;

				if (Head != null)
					result += Head.Power;

				if (Chestpiece != null)
					result += Chestpiece.Power;

				return result;
			}
		}

		public Player(string name, int maxHP)
		{
			this.Name = name;

			MaxHP = maxHP;
			CurrentHP = maxHP;

			AllAbilities = new Ability[20];

			EquippedAbilities = new Ability[Constants.Ranges.STANDARD_ABILITY_SIZE];
			EquippedFinishers = new Ability[Constants.Ranges.FINISHER_ABILITY_SIZE];
		}
	}
}