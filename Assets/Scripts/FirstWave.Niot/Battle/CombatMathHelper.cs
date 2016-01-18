using System.Collections.Generic;
using System.Linq;
using FirstWave.Core;
using FirstWave.Niot.Abilities;
using FirstWave.Niot.Game;
using UnityEngine;

namespace FirstWave.Niot.Battle
{
	public static class CombatMathHelper
	{
		public static IList<BattleCommand> OrderCommandsBySpeed(IEnumerable<BattleCommand> commands)
		{
			var speeds = new List<Tuple<Combatant, int>>();

			foreach (var c in commands)
				speeds.Add(Tuple.Create(c.Actor, GetEffectiveAttributeValue(c.Actor.Speed, 0.3f)));

			speeds = speeds.OrderByDescending(s => s.Item2).ToList();

			var orderedCommands = new List<BattleCommand>();

			foreach (var s in speeds)
				orderedCommands.Add(commands.FirstOrDefault(c => c.Actor == s.Item1));

			return orderedCommands;
		}

		public static int GetDamageForAbility(Combatant actor, Ability ability, Combatant target)
		{
			int modifier = ability.AbilityType == AbilityType.Physical ? actor.AttackPower : actor.MagicPower;

			modifier = GetEffectiveAttributeValue(modifier, 0.2f);

			// For now magic damage is not reducible, this will change in the very near future
			int damageReduction = ability.AbilityType == AbilityType.Physical ? target.Defense : 0;

			int damage = Random.Range(ability.DamageRange.Item1, ability.DamageRange.Item2) + modifier - damageReduction;

			return damage;
		}

		private static int GetEffectiveAttributeValue(int attributeValue, float variance)
		{
			return (int)Random.Range(attributeValue * (1f - variance), attributeValue * (1f + variance));
		}
	}
}
