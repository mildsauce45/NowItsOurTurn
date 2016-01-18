using FirstWave.Niot.Abilities;
using FirstWave.Niot.Game;

namespace FirstWave.Niot.Battle
{
	public class BattleCommand
	{
		public Combatant Actor { get; set; }
		public CommandType CommandType { get; set; }
		public Ability Ability { get; set; }
		public Target Target { get; set; }
	}
}
