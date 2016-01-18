using UnityEngine;
using System.Collections;
using FirstWave.StateMachine.Unity;
using FirstWave.Niot.Game;
using FirstWave.Core.GUI.Menus;
using System.Linq;
using FirstWave.Niot.GUI.Controls.Battle;

namespace FirstWave.Niot.Battle.PartyInput
{
	public class SelectAbilityState : State<PartyInputTriggers>
	{
		private Player[] party;

		private Menu actionMenu;
		private AbilityMenu abilityMenu;

		private bool isCanceled;

		public Ability SelectedAbility { get; set; }

		public SelectAbilityState(GameObject owner)
			: base(owner)
		{
			party = TurnBasedBattleManager.Instance.Party;

			actionMenu = owner.GetComponentsInChildren<Menu>().FirstOrDefault(m => m.name == "ActionMenu");
			abilityMenu = owner.GetComponentInChildren<AbilityMenu>();

			abilityMenu.Canceled += AbilityCanceled;
		}

		public void SetupAbilities(int partyMember)
		{
			// Remove the old cruft
			abilityMenu.Clear();

			var pm = party[partyMember];

			var allAbilities = pm.EquippedAbilities.Union(pm.EquippedFinishers);

			for (int i = 0; i < allAbilities.Count(); i++)
			{
				var ability = allAbilities.ElementAt(i);

				if (ability != null)
					abilityMenu.AddMenuItem(new AbilityMenuItem(ability, () => SelectedAbility = ability));
			}
		}

		#region State Machine Methods

		public override void Update()
		{
		}

		public override void OnEnter()
		{
			actionMenu.DisableInput = true;
			abilityMenu.enabled = true;
			isCanceled = false;

			SelectedAbility = null;
		}

		public override void OnExit()
		{
			actionMenu.DisableInput = false;
			abilityMenu.enabled = false;
		}

		public override PartyInputTriggers GetTrigger()
		{
			if (isCanceled)
				return PartyInputTriggers.ActionCanceled;
			else if (SelectedAbility != null)
				return PartyInputTriggers.AbilitySelected;

			return PartyInputTriggers.Nothing;
		}

		#endregion

		#region Event Handlers

		private void AbilityCanceled(object sender, System.EventArgs ea)
		{
			isCanceled = true;
		}

		#endregion
	}
}