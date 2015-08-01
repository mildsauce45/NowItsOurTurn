using System.Linq;
using FirstWave.Core;
using FirstWave.Niot.Abilities;
using FirstWave.Niot.Battle;
using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Managers;
using FirstWave.StateMachine;
using FirstWave.StateMachine.Unity;
using UnityEngine;

public class TurnBasedBattleManager : Singleton<TurnBasedBattleManager>
{
	public GameObject DisplayCombatantsUI;
	public GameObject GetPlayerInputUI;
	public GameObject ResolveActionUI;	

	public Player[] Party { get; private set; }
	public Enemy[] EnemyParty { get; private set; }

	public ElementType[] FieldEffect { get; private set; }

	private StateMachine<State<TurnBasedBattleTriggers?>, TurnBasedBattleTriggers> stateMachine;

	// Use this for initialization
	void Start()
	{
		var combatants = this.InitializeBattle();

		Party = combatants.Item1;
		EnemyParty = combatants.Item2;

		var initialState = new DisplayCombatantsState(DisplayCombatantsUI);
		var getInputState = new GetPartyInputState(GetPlayerInputUI);
		var getEnemyInputState = new GetEnemyInputState(combatants.Item1, combatants.Item2, this.gameObject);
		var resolveState = new ResolveActionsState(ResolveActionUI);

		stateMachine = new StateMachine<State<TurnBasedBattleTriggers?>, TurnBasedBattleTriggers>(initialState);

		stateMachine.Configure(initialState).
			OnEntry(a => initialState.OnEnter()).
			OnExit(a => initialState.OnExit()).
			Permit(TurnBasedBattleTriggers.CombatantsDisplayed, getInputState);

		stateMachine.Configure(getInputState).
			OnEntry(a => getInputState.OnEnter()).
			OnExit(a => getInputState.OnExit()).
			Permit(TurnBasedBattleTriggers.InputAccepted, getEnemyInputState).
			Permit(TurnBasedBattleTriggers.InputCanceled, initialState);

		stateMachine.Configure(getEnemyInputState).Permit(TurnBasedBattleTriggers.EnemyActionDecided, resolveState);

		stateMachine.Configure(resolveState).
			OnEntry(a => { resolveState.Commands = CombatMathHelper.OrderCommandsBySpeed(getInputState.Commands.Concat(getEnemyInputState.Commands)); resolveState.OnEnter(); }).
			OnExit(a => resolveState.OnExit()).
			Permit(TurnBasedBattleTriggers.CombatContinues, getInputState).
			Permit(TurnBasedBattleTriggers.EnemyKilled, null).
			Permit(TurnBasedBattleTriggers.PlayerKilled, null);

		// Now call OnEnter in the initial state
		initialState.OnEnter();
	}

	void Update()
	{
		if (stateMachine.State == null)
			return;

		stateMachine.State.Update();

		var trigger = stateMachine.State.GetTrigger();
		if (trigger.HasValue)
		{
			Debug.Log(trigger);
			stateMachine.Fire(trigger.Value);
		}
	}

	public void AddFieldEffect(ElementType et)
	{
		int firstEmptyIndex = -1;

		for (int i = 0; i < FieldEffect.Length; i++)
		{
			if (FieldEffect[i] == ElementType.None)
			{
				firstEmptyIndex = i;
				break;
			}
		}

		// If there was an empty slot put the element there
		if (firstEmptyIndex >= 0)
			FieldEffect[firstEmptyIndex] = et;
		else
		{
			// Otherwise we need to move everything up one and add this guy to the end;
			for (int i = 0; i < FieldEffect.Length - 1; i++)
				FieldEffect[i] = FieldEffect[i + 1];

			FieldEffect[FieldEffect.Length - 1] = et;
		}
	}

	public void SetFieldEffect(ElementType[] newArr)
	{
		if (newArr.Length < FieldEffect.Length)
		{
			// First copy the elements over
			System.Array.Copy(newArr, FieldEffect, newArr.Length);

			// Now fill in empty elements in the trailing positions
			for (int i = newArr.Length; i < FieldEffect.Length; i++)
				FieldEffect[i] = ElementType.None;
		}
		else if (newArr.Length == FieldEffect.Length)
			FieldEffect = newArr;
	}

	private Tuple<Player[], Enemy[]> InitializeBattle()
	{
		var players = new Player[3];
		players[0] = new Player("The Sauce", 999);
		players[1] = new Player("Callsign Charlie", 999);
		players[2] = new Player("Rasch", 999);

		players[0].EquippedAbilities[0] = AbilityManager.Instance.GetAbility(2);
		players[0].EquippedAbilities[1] = AbilityManager.Instance.GetAbility(8);
		players[0].EquippedFinishers[0] = AbilityManager.Instance.GetAbility(9);
		players[0].Speed = 6;
		players[0].Strength = 5;
		players[0].Will = 6;
		players[0].Endurance = 4;
		players[0].Weapon = WeaponManager.Instance.GetWeapon(1);

		players[1].EquippedAbilities[0] = AbilityManager.Instance.GetAbility(3);
		players[1].Speed = 10;
		players[1].Strength = 3;
		players[1].Will = 8;
		players[1].Endurance = 4;
		players[1].Weapon = WeaponManager.Instance.GetWeapon(2);

		players[2].EquippedAbilities[0] = AbilityManager.Instance.GetAbility(7);
		players[2].EquippedAbilities[1] = AbilityManager.Instance.GetAbility(6);
		players[2].EquippedAbilities[2] = AbilityManager.Instance.GetAbility(2);
		players[2].EquippedAbilities[3] = AbilityManager.Instance.GetAbility(4);
		players[2].EquippedFinishers[0] = AbilityManager.Instance.GetAbility(9);
		players[2].EquippedFinishers[1] = AbilityManager.Instance.GetAbility(10);
		players[2].Speed = 2;
		players[2].Strength = 10;
		players[2].Will = 3;
		players[2].Endurance = 8;
		players[2].Weapon = WeaponManager.Instance.GetWeapon(3);

		var enemies = new Enemy[3];

		for (int i = 0; i < enemies.Length; i++)
			enemies[i] = EnemyManager.Instance.GetEnemy(Random.Range(0, 3));

		FieldEffect = new ElementType[Constants.Ranges.FIELD_EFFECT_SIZE];

		for (int i = 0; i < FieldEffect.Length; i++)
			FieldEffect[i] = ElementType.None;

		return Tuple.Create(players, enemies);
	}
}
