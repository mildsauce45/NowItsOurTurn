
namespace FirstWave.Niot.Game
{
	public abstract class Combatant : ITargetable
	{
		public string Name { get; set; }

		public int MaxHP { get; set; }
		public int CurrentHP { get; set; }

		public int Speed { get; set; }
		public int Strength { get; set; }
		public int Will { get; set; }
		public int Endurance { get; set; }

		public abstract int AttackPower { get; }
		public abstract int MagicPower { get; }
		public abstract int Defense { get; }

		public bool IsDead
		{
			get { return CurrentHP <= 0; }
		}
	}
}
