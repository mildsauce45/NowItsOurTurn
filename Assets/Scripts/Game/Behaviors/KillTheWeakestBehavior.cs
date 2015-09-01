using FirstWave.Niot.Abilities;
using FirstWave.Niot.Battle;

namespace FirstWave.Niot.Game.Behaviors
{
	public class KillTheWeakestBehavior : IEnemyBehavior
	{
		private Enemy me;

		public KillTheWeakestBehavior(Enemy owner)
		{
			me = owner;
		}

		public BattleCommand GetNextAction(Player[] party, Enemy[] allies, ElementType[] fieldEffect)
		{
			var ability = BehaviorHelpers.GetHighestPotentialDamageAbility(me, fieldEffect);

			if (ability == null)
				ability = Ability.ATTACK;

			var target = new Target { TheTarget = BehaviorHelpers.GetWeakestTarget(party), TargetType = ability.TargetType };

			return new BattleCommand
			{
				Actor = me,
				Target = target,
				Ability = ability,
				CommandType = ability.Id != -1 ? CommandType.Ability : CommandType.Attack
			};
		}
	}
}
