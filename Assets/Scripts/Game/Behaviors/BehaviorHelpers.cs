using System.Linq;
using FirstWave.Niot.Abilities;
using UnityEngine;

namespace FirstWave.Niot.Game.Behaviors
{
	internal static class BehaviorHelpers
	{
		internal static Combatant GetWeakestTarget(Combatant[] combatants)
		{
			var livingTargets = combatants.Where(p => !p.IsDead);

			return livingTargets.OrderBy(p => p.CurrentHP).FirstOrDefault();
		}

		internal static Combatant GetRandomTarget(Combatant[] combatants)
		{
			var livingTargets = combatants.Where(p => !p.IsDead);

			return livingTargets.ElementAt(Random.Range(0, livingTargets.Count()));
		}

		internal static Ability GetHighestPotentialDamageAbility(Enemy actor, ElementType[] fieldEffect)
		{
			// First let's look for the highest potential damage in any of the monsters equipped finishers
			if (actor.EquippedFinishers != null && actor.EquippedFinishers.Any())
			{
				var maxDamageFinisher = actor.EquippedFinishers.Where(f => f.CanUse(fieldEffect)).OrderByDescending(f => f.DamageRange.Item2).FirstOrDefault();
				if (maxDamageFinisher != null)
					return maxDamageFinisher;
			}

			// If not look for the biggest potential damage in the monsters abilities that are off cooldown
			if (actor.EquippedAbilities != null && actor.EquippedAbilities.Any())
			{
				var maxDamageAbility = actor.EquippedAbilities.Where(f => f.CooldownRemaining <= 0).OrderByDescending(f => f.DamageRange.Item2).FirstOrDefault();
				if (maxDamageAbility != null)
					return maxDamageAbility;
			}

			return null;
		}
	}
}
