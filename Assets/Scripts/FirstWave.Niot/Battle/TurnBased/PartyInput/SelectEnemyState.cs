using System.Linq;
using FirstWave.Core.GUI.Menus;
using FirstWave.StateMachine.Unity;
using UnityEngine;
using FirstWave.Niot.Game;

namespace FirstWave.Niot.Battle.PartyInput
{
	public class SelectEnemyState : State<PartyInputTriggers>
	{
		private bool canceled;
		
		#region Menus & Things

		private Menu menu;
		private MenuItem[] menuItems;

		private Menu actionMenu;

		#endregion

		private Enemy[] enemies;

		public int SelectedIndex = -1;

		public SelectEnemyState(GameObject owner)
			: base(owner)
		{
			this.enemies = TurnBasedBattleManager.Instance.EnemyParty;

			this.menu = owner.GetComponentsInChildren<Menu>().FirstOrDefault(m => m.gameObject.name == "EnemyMenu");
			this.actionMenu = this.Owner.GetComponentsInChildren<Menu>().FirstOrDefault(m => m.gameObject.name == "ActionMenu");

			if (menu != null)
			{
				this.menu.Canceled += Canceled;
				this.menuItems = new MenuItem[enemies.Length];

				for (int i = 0; i < enemies.Length; i++)
				{
					int closureIndex = i;

					var mi = new MenuItem(enemies[i].Name, () => EnemySelected(closureIndex));

					menuItems[i] = mi;

					this.menu.AddMenuItem(mi);
				}
			}
		}

		public override void Update()
		{
		}

		public override PartyInputTriggers GetTrigger()
		{
			if (canceled)
				return PartyInputTriggers.ActionCanceled;
			else if (SelectedIndex >= 0)
				return PartyInputTriggers.EnemySelected;

			return PartyInputTriggers.Nothing;
		}

		public override void OnEnter()
		{
			canceled = false;
			
			SelectedIndex = -1;

			this.menu.enabled = true;

			// Freeze the action menu, but still let it draw itself
			actionMenu.DisableInput = true;

			// Now update the menu items rendering capabilities
			for (int i = 0; i < menuItems.Length; i++)
				menuItems[i].ShouldRender = !enemies[i].IsDead;
		}

		public override void OnExit()
		{
			this.menu.enabled = false;
		}

		private void EnemySelected(int index)
		{			
			SelectedIndex = index;

			this.menu.SetSelectedIndex(0);
		}

		private void Canceled(object sender, System.EventArgs e)
		{
			canceled = true;
		}
	}
}
