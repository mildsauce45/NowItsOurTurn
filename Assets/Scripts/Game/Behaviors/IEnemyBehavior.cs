using FirstWave.Niot.Abilities;
using FirstWave.Niot.Battle;

namespace FirstWave.Niot.Game.Behaviors
{
	public interface IEnemyBehavior
	{
		BattleCommand GetNextAction(Player[] party, Enemy[] allies, ElementType[] fieldEffect);
	}
}
