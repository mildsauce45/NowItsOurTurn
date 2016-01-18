using System.Linq;
using FirstWave.Core.GUI;
using FirstWave.StateMachine.Unity;
using UnityEngine;
using FirstWave.Unity.Gui.Controls;

namespace FirstWave.Niot.Battle.PartyInput
{
	public class SelectActionState : State<PartyInputTriggers>
	{
		public bool IsAttack { get; private set; }
		public bool IsAbility { get; private set; }

		private Menu menu;
		private MessageBox placeholder;
		private bool canceled;

		public SelectActionState(GameObject owner)
			: base(owner)
		{
			//this.menu = owner.GetComponentsInChildren<Menu>().FirstOrDefault(m => m.gameObject.name == "ActionMenu");

			if (this.menu != null)
			{
				//this.menu.Canceled += Canceled;

				//this.menu.AddMenuItem(new MenuItem("Attack", AttackSelected));
				//this.menu.AddMenuItem(new MenuItem("Ability", AbilitySelected));
				//this.menu.AddMenuItem(new MenuItem("Defend", () => { }));
				//this.menu.AddMenuItem(new MenuItem("Flee", () => { }));
			}

			this.placeholder = owner.GetComponentInChildren<MessageBox>();
		}

		public override void Update()
		{
		}

		public override PartyInputTriggers GetTrigger()
		{
			if (canceled)
				return PartyInputTriggers.ActionCanceled;
			else if (IsAttack)
				return PartyInputTriggers.AttackSelected;
			else if (IsAbility)
				return PartyInputTriggers.AbilitySelected;

			return PartyInputTriggers.Nothing;
		}

		public override void OnEnter()
		{
			canceled = false;
			IsAttack = false;
			IsAbility = false;

			//menu.enabled = true;
			//menu.DisableInput = false;

			placeholder.enabled = true;
		}

		public override void OnExit()
		{
			placeholder.enabled = false;
		}

		private void AttackSelected()
		{
			IsAttack = true;
			IsAbility = false;
		}

		private void AbilitySelected()
		{
			IsAttack = false;
			IsAbility = true;
		}

		private void Canceled(object sender, System.EventArgs e)
		{
			canceled = true;
			IsAttack = false;
			IsAbility = false;
		}
	}
}
