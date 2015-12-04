using FirstWave.Niot.Abilities;
using FirstWave.Niot.Battle;
using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Behaviors;
using System.Linq;

namespace Assets.Scripts.FirstWave.Niot.Behaviors
{
	public class RimuldarGeneralBehavior : IEnemyBehavior
	{
		private Enemy me;

		private Ability pressTheAdvantage;
		private Ability solarBeam;
		private Ability cleave;
		private Ability sliceAndDice;

		public RimuldarGeneralBehavior(Enemy owner)
		{
			me = owner;

			pressTheAdvantage = me.EquippedFinishers.FirstOrDefault(a => a.Name == "Press the Advantage");
			solarBeam = me.EquippedAbilities.FirstOrDefault(a => a.Name == "Solar Beam");
			cleave = me.EquippedAbilities.FirstOrDefault(a => a.Name == "Cleave");
			sliceAndDice = me.EquippedAbilities.FirstOrDefault(a => a.Name == "Slice & Dice");

			// The general can use the solar beam ability every other turn
			solarBeam.Cooldown = 1;
		}

		public BattleCommand GetNextAction(Player[] party, Enemy[] allies, ElementType[] fieldEffect)
		{
			if (pressTheAdvantage.CanUse(fieldEffect))
			{
				var target = new Target { TheTarget = BehaviorHelpers.GetStrongestTarget(party), TargetType = TargetTypes.Single };

				return CreateBattleCommand(target, pressTheAdvantage, CommandType.Ability);
			}

			if ((me.CurrentHP / (decimal)me.MaxHP) <= .5m && solarBeam.CanUse(fieldEffect))
			{
				var target = new Target { TheTarget = BehaviorHelpers.GetWeakestTarget(party), TargetType = TargetTypes.Single };

				return CreateBattleCommand(target, solarBeam, CommandType.Ability);
			}

			if (party.All(p => !p.IsDead) && cleave.CanUse(fieldEffect))
			{
				var target = new Target { TargetType = TargetTypes.All };

				return CreateBattleCommand(target, cleave, CommandType.Ability);
			}

			if (sliceAndDice.CanUse(fieldEffect))
			{
				var target = new Target { TheTarget = BehaviorHelpers.GetRandomTarget(party), TargetType = TargetTypes.Single };

				return CreateBattleCommand(target, sliceAndDice, CommandType.Ability);
			}

			return CreateBattleCommand(new Target { TheTarget = BehaviorHelpers.GetRandomTarget(party), TargetType = TargetTypes.Single }, 
									   Ability.ATTACK, 
									   CommandType.Attack);
        }

		private BattleCommand CreateBattleCommand(Target target, Ability ability, CommandType type)
		{
			return new BattleCommand
			{
				Actor = me,
				Target = target,
				Ability = ability,
				CommandType = type
			};
		}
	}
}
