using FirstWave.Core.GUI;
using FirstWave.Niot.Battle.PartyInput;
using FirstWave.Niot.Game;
using FirstWave.Niot.GUI.Controls;
using FirstWave.StateMachine;
using FirstWave.StateMachine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirstWave.Niot.Battle
{
    public class GetPartyInputState : State<TurnBasedBattleTriggers?>
	{
		public Player[] Party { get; private set; }
		public Enemy[] Enemies { get; private set; }

		public IList<BattleCommand> Commands { get; private set; }

		private BorderedLabel currentPlayerIndicator;
		private PartyInputControlFlow controlFlow;

		private StateMachine<State<PartyInputTriggers>, PartyInputTriggers> stateMachine;

		private SelectActionState selectActionState;
		private SelectAbilityState selectAbilityState;
		private SelectEnemyState selectEnemyState;

		public GetPartyInputState(GameObject owner)
			: base(owner)
		{
			this.Party = TurnBasedBattleManager.Instance.Party;
			this.Enemies = TurnBasedBattleManager.Instance.EnemyParty;

			Commands = new List<BattleCommand>();

			selectActionState = new SelectActionState(owner);
			selectAbilityState = new SelectAbilityState(owner);
			selectEnemyState = new SelectEnemyState(owner);

			stateMachine = new StateMachine<State<PartyInputTriggers>, PartyInputTriggers>(selectActionState);

			stateMachine.Configure(selectActionState).
				OnEntry(OnActionSelectStateEnter).
				OnExit(OnActionSelectStateExit).
				PermitReentry(PartyInputTriggers.ActionCanceled).
				Permit(PartyInputTriggers.AttackSelected, selectEnemyState).
				Permit(PartyInputTriggers.AbilitySelected, selectAbilityState);

			stateMachine.Configure(selectAbilityState).
				OnEntry(OnSelectAbilityEnter).
				OnExit(OnSelectAbilityExit).
				Permit(PartyInputTriggers.ActionCanceled, selectActionState).
				PermitIf(PartyInputTriggers.AbilitySelected, selectEnemyState, IsAbilitySingleTarget).
				PermitIf(PartyInputTriggers.AbilitySelected, selectActionState, IsAbilityMultiTarget);

			stateMachine.Configure(selectEnemyState).
				OnEntry(a => selectEnemyState.OnEnter()).
				OnExit(HandleEnemySelected).
				PermitIf(PartyInputTriggers.ActionCanceled, selectActionState, () => controlFlow == PartyInputControlFlow.Attack).
				PermitIf(PartyInputTriggers.ActionCanceled, selectAbilityState, () => controlFlow == PartyInputControlFlow.Ability).
				Permit(PartyInputTriggers.EnemySelected, selectActionState);

			this.currentPlayerIndicator = owner.GetComponentInChildren<BorderedLabel>();
		}

		public override void Update()
		{
			var playerIndex = Commands.Count;

			if (playerIndex < Party.Length)
				currentPlayerIndicator.text = Party[playerIndex].Name;

			stateMachine.State.Update();

			var trigger = stateMachine.State.GetTrigger();
			if (trigger != PartyInputTriggers.Nothing)
			{
				Debug.Log("State sub-statemachine: " + trigger);

				stateMachine.Fire(trigger);
			}
		}

		public override TurnBasedBattleTriggers? GetTrigger()
		{
			if (Commands.Count == Party.Length)
				return TurnBasedBattleTriggers.InputAccepted;

			return null;
		}

		public override void OnEnter()
		{
			Commands.Clear();

			currentPlayerIndicator.enabled = true;

			stateMachine.State.OnEnter();
		}

		public override void OnExit()
		{
			currentPlayerIndicator.enabled = false;

			//var menu = this.Owner.GetComponentsInChildren<Menu>().FirstOrDefault(m => m.gameObject.name == "ActionMenu");
			//if (menu != null)
			//	menu.enabled = false;

			var messageBox = this.Owner.GetComponentInChildren<MessageBox>();
			if (messageBox != null)
				messageBox.enabled = false;
		}

		#region Select Action Transitions

		private void OnActionSelectStateEnter(Transition<State<PartyInputTriggers>, PartyInputTriggers> transition)
		{
			selectActionState.OnEnter();

			controlFlow = PartyInputControlFlow.StillSelecting;
		}

		private void OnActionSelectStateExit(Transition<State<PartyInputTriggers>, PartyInputTriggers> transition)
		{
			if (transition.Trigger == PartyInputTriggers.ActionCanceled && Commands.Count > 0)
				RemoveCommand();
			else
			{
				if (transition.Trigger == PartyInputTriggers.AttackSelected)
					controlFlow = PartyInputControlFlow.Attack;
				else if (transition.Trigger == PartyInputTriggers.AbilitySelected)
					controlFlow = PartyInputControlFlow.Ability;				

				transition.Source.OnExit();
			}
		}

		#endregion

		#region Select Ability Transitions

		private void OnSelectAbilityEnter(Transition<State<PartyInputTriggers>, PartyInputTriggers> transition)
		{
			// Perform UI related setup
			selectAbilityState.OnEnter();

			// Now tell the state which party member we're setting up for
			selectAbilityState.SetupAbilities(Commands.Count);
		}

		private void OnSelectAbilityExit(Transition<State<PartyInputTriggers>, PartyInputTriggers> transition)
		{
			selectAbilityState.OnExit();

			if (selectAbilityState.SelectedAbility != null && selectAbilityState.SelectedAbility.TargetType == TargetTypes.All)
			{
				int partyMember = Commands.Count;

				var command = new BattleCommand
				{
					Actor = this.Party[partyMember],
					Ability = selectAbilityState.SelectedAbility,
					Target = new Target { TargetType = TargetTypes.All }
				};

				Commands.Add(command);
			}
		}

		private bool IsAbilitySingleTarget()
		{
			return selectAbilityState.SelectedAbility != null && selectAbilityState.SelectedAbility.TargetType == TargetTypes.Single;
		}

		private bool IsAbilityMultiTarget()
		{
			return selectAbilityState.SelectedAbility != null && selectAbilityState.SelectedAbility.TargetType == TargetTypes.All;
		}

		#endregion

		#region Select Enemy Transitions

		private void HandleEnemySelected(Transition<State<PartyInputTriggers>, PartyInputTriggers> transition)
		{
			selectEnemyState.OnExit();

			if (transition.Trigger == PartyInputTriggers.ActionCanceled)
				return;

			if (selectActionState.IsAttack && selectEnemyState.SelectedIndex >= 0)
			{
				int partyMember = Commands.Count;				

				var command = new BattleCommand
				{
					Actor = this.Party[partyMember],
					Ability = Ability.ATTACK,
					Target = new Target { TargetType = TargetTypes.Single, TheTarget = Enemies[selectEnemyState.SelectedIndex] }
				};

				Commands.Add(command);
			}
			else if (selectActionState.IsAbility && selectAbilityState.SelectedAbility != null && selectEnemyState.SelectedIndex >= 0)
			{
				int partyMember = Commands.Count;

				var command = new BattleCommand
				{
					Actor = this.Party[partyMember],
					Ability = selectAbilityState.SelectedAbility,
					Target = new Target { TargetType = selectAbilityState.SelectedAbility.TargetType, TheTarget = Enemies[selectEnemyState.SelectedIndex] }
				};

				Commands.Add(command);
			}
		}

		#endregion

		private void RemoveCommand()
		{
			if (!Commands.Any())
				return;

			var command = Commands.Last();

			if (command != null)
				Commands.Remove(command);
		}
	}
}
