using System.Linq;
using FirstWave.Niot.Abilities;
using FirstWave.Niot.Battle;
using UnityEngine;

namespace FirstWave.Niot.Game.Behaviors
{
	/// <summary>
	/// The All Out Behavior picks a random living target and hit's the target with the highest damage potential ability it has off of cooldown
	/// If no such ability exists, it will just attack the target
	/// </summary>
	public class AllOutBehavior : IEnemyBehavior
	{
		private Enemy me;

		public AllOutBehavior(Enemy owner)
		{
			me = owner;
		}

		public BattleCommand GetNextAction(Player[] party, Enemy[] allies, ElementType[] fieldEffect)
		{
			var ability = BehaviorHelpers.GetHighestPotentialDamageAbility(me, fieldEffect);

			if (ability == null)
				ability = Ability.ATTACK;

			var target = new Target { TheTarget = BehaviorHelpers.GetRandomTarget(party), TargetType = ability.TargetType };

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
