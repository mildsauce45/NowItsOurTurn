using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FirstWave.Core;
using FirstWave.Core.Audio;
using FirstWave.Core.GUI;
using FirstWave.Niot.Abilities;
using FirstWave.Niot.Game;
using FirstWave.StateMachine.Unity;
using UnityEngine;

namespace FirstWave.Niot.Battle
{
	public class ResolveActionsState : State<TurnBasedBattleTriggers?>
	{
		public Player[] Party { get; private set; }
		public Enemy[] Enemies { get; private set; }

		public IList<BattleCommand> Commands { get; set; }

		public float delayTimer = 0.75f;

		private MessageBox messageBox;
		private AudioSource audioSource;

		private TextDisplayTimer textTimer;
		private AudioTimer audioTimer;

		private bool proceed = true;

		public ResolveActionsState(GameObject owner)
			: base(owner)
		{
			Party = TurnBasedBattleManager.Instance.Party;
			Enemies = TurnBasedBattleManager.Instance.EnemyParty;

			this.messageBox = owner.GetComponent<MessageBox>();
			this.audioSource = owner.GetComponent<AudioSource>();

			this.audioTimer = new AudioTimer(this.audioSource);
		}

		public override void Update()
		{
			// If there are any commands left and we've finished displaying the last one, or this is the first command go ahead and resolve it
			if (Commands.Any() && proceed)
			{
				// If the next command comes from a dead man, ignore it
				while (Commands.Any() && Commands.First().Actor.IsDead)
					Commands.RemoveAt(0);

				// Now if there are any left grab the first and resolve it
				if (Commands.Any())
				{
					var command = Commands.First();

					proceed = false;

					ResolveAction(command);
				}
			}
			else if (!proceed && textTimer != null)
			{
				textTimer.Update();
			}
		}

		public override TurnBasedBattleTriggers? GetTrigger()
		{
			if (!proceed)
				return null;

			// Player death takes precedence over enemy death
			if (Party.All(p => p.IsDead))
				return TurnBasedBattleTriggers.PlayerKilled;
			else if (Enemies.All(p => p.IsDead))
				return TurnBasedBattleTriggers.EnemyKilled;
			else if (!Commands.Any())
				return TurnBasedBattleTriggers.CombatContinues;

			return null;
		}

		public override void OnEnter()
		{
			this.messageBox.enabled = true;
		}

		public override void OnExit()
		{
			this.messageBox.enabled = false;
		}

		private void ResolveAction(BattleCommand command)
		{
			if (command == null || command.Actor.IsDead)
			{
				ProceedToNextCommand();
				return;
			}

			IEnumerable<ITargetable> targets = null;

			if (command.Actor is Player)
				targets = Enemies;
			else
				targets = Party;

			ITargetable target = null;

			if (command.Target.TargetType == TargetTypes.Single)
			{
				target = targets.FirstOrDefault(t => t == command.Target.TheTarget);

				// If the selected target is now dead, pick the first available target
				if (target.IsDead)
					target = targets.FirstOrDefault(t => !t.IsDead);

				if (target != null)
				{
					var combatant = target as Combatant;

					if (!command.Ability.IsFinisher || UseFinisher(command.Actor, command.Ability))
						UseAbility(command.Actor, command.Ability, combatant);
				}
			}
			else if (command.Target.TargetType == TargetTypes.All)
			{
				if (targets.OfType<Combatant>().All(c => c.IsDead))
				{
					ProceedToNextCommand();
					return;
				}

				if (!command.Ability.IsFinisher || UseFinisher(command.Actor, command.Ability))
				{
					if (command.Ability.ElementType != ElementType.None)
						TurnBasedBattleManager.Instance.AddFieldEffect(command.Ability.ElementType);

					this.messageBox.StartCoroutine(CreateMultiTargetCoroutine(command.Actor, command.Ability, targets));

					if (command.Ability.Cooldown > 0)
						command.Ability.CooldownRemaining = command.Ability.Cooldown + 1;
				}
			}

			foreach (var ability in (command.Actor as Combatant).EquippedAbilities)
			{
				if (ability != null && ability.Cooldown > 0 && ability.CooldownRemaining > 0)
					ability.CooldownRemaining--;
			}
		}

		private bool UseFinisher(Combatant actor, Ability finisher)
		{
			var f = finisher as Finisher;

			if (f.CanUse(TurnBasedBattleManager.Instance.FieldEffect))
			{
				// Remove each element from the field effect array
				foreach (var c in f.ElementCost)
				{
					for (int i = c.Amount; i > 0; i--)
					{
						int index = 0;
						while (TurnBasedBattleManager.Instance.FieldEffect[index] != c.ElementType)
							index++;

						TurnBasedBattleManager.Instance.FieldEffect[index] = ElementType.None;
					}
				}

				// Now we need to collapse the field effect array down;
				TurnBasedBattleManager.Instance.SetFieldEffect(TurnBasedBattleManager.Instance.FieldEffect.Where(e => e != ElementType.None).ToArray());

				return true;
			}
			else
			{
				this.messageBox.StartCoroutine(CreateFailedFinisherCoroutine(actor));

				return false;
			}
		}

		private void UseAbility(Combatant actor, Ability ability, Combatant target)
		{
			if (ability.ElementType != ElementType.None)
				TurnBasedBattleManager.Instance.AddFieldEffect(ability.ElementType);

			// Add in the cooldown if we need to, with a plus one because we're going to deduct one now from the cooldown remaining for all abilities for this person
			if (ability.Cooldown > 0)
				ability.CooldownRemaining = ability.Cooldown + 1;

			messageBox.StartCoroutine(CreateSingleTargetCoroutine(actor, target, ability));
		}

		private void SetNewTextOnTimer(string newText)
		{
			if (textTimer == null)
			{
				textTimer = new TextDisplayTimer(messageBox.characterDelay);
				messageBox.textSource = textTimer;
			}

			textTimer.Stop();
			textTimer.Reset();
			textTimer.FullText = newText;
			textTimer.Start();
		}

		private void ProceedToNextCommand()
		{
			Commands.RemoveAt(0);

			proceed = true;
		}

		#region Coroutines

		private IEnumerator CreateFailedFinisherCoroutine(Combatant actor)
		{
			SetNewTextOnTimer(string.Format("The elemental lords remain silent for {0}", actor.Name));

			while (!textTimer.IsComplete)
				yield return new WaitForSeconds(messageBox.characterDelay);

			yield return new WaitForSeconds(delayTimer);

			ProceedToNextCommand();
		}

		private IEnumerator CreateSingleTargetCoroutine(Combatant actor, Combatant target, Ability ability)
		{
			SetNewTextOnTimer(string.Format("{0} uses {1} on {2}.", actor.Name, ability.Name, target.Name));

			while (!textTimer.IsComplete)
				yield return new WaitForSeconds(messageBox.characterDelay);

			yield return new WaitForSeconds(delayTimer);

			int damage = CombatMathHelper.GetDamageForAbility(actor, ability, target);

			if (damage <= 0)
			{
				SetNewTextOnTimer(string.Format("A miss! {0} does no damage to {1}.", actor.Name, target.Name));

				while (!textTimer.IsComplete)
					yield return new WaitForSeconds(messageBox.characterDelay);

				yield return new WaitForSeconds(delayTimer);
			}
			else
			{
				if (ability.AudioClip != null)
				{
					audioTimer.SetAudioClip(ability.AudioClip);
					audioTimer.Start();

					yield return new WaitForSeconds(ability.AudioClip.length);
				}

				SetNewTextOnTimer(string.Format("{0} takes {1} damage.", target.Name, damage));

				while (!textTimer.IsComplete)
					yield return new WaitForSeconds(messageBox.characterDelay);

				yield return new WaitForSeconds(delayTimer);

				textTimer.Stop();

				target.CurrentHP -= damage;

				if (target.IsDead)
				{
					SetNewTextOnTimer(string.Format("{0} dies!", target.Name));

					while (!textTimer.IsComplete)
						yield return new WaitForSeconds(messageBox.characterDelay);

					yield return new WaitForSeconds(delayTimer);
				}
			}

			ProceedToNextCommand();
		}

		private IEnumerator CreateMultiTargetCoroutine(Combatant actor, Ability ability, IEnumerable<ITargetable> targets)
		{
			// Play a sound effect and/or visual cue that something happened

			var livingEnemies = targets.Where(p => !p.IsDead).OfType<Combatant>();

			if (livingEnemies.Any())
			{
				SetNewTextOnTimer(string.Format("{0} uses {1}.", actor.Name, ability.Name));

				while (!textTimer.IsComplete)
					yield return new WaitForSeconds(messageBox.characterDelay);

				yield return new WaitForSeconds(0.5f);

				foreach (var le in livingEnemies)
				{
					int damage = CombatMathHelper.GetDamageForAbility(actor, ability, le);

					SetNewTextOnTimer(string.Format("{0} takes {1} damage.", le.Name, damage));

					while (!textTimer.IsComplete)
						yield return new WaitForSeconds(messageBox.characterDelay);

					yield return new WaitForSeconds(delayTimer);

					le.CurrentHP -= damage;

					if (le.IsDead)
					{
						SetNewTextOnTimer(string.Format("{0} dies!", le.Name));

						while (!textTimer.IsComplete)
							yield return new WaitForSeconds(messageBox.characterDelay);

						yield return new WaitForSeconds(delayTimer);
					}
				}
			}

			ProceedToNextCommand();
		}

		#endregion
	}
}
