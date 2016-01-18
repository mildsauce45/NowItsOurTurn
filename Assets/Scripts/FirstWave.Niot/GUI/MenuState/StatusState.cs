using Assets.Scripts.GUI.Controls.MainMenu;
using FirstWave.Core.GUI;
using FirstWave.Core.GUI.Menus;
using FirstWave.Niot.GUI.MenuState;
using FirstWave.Niot.Managers;
using FirstWave.StateMachine.Unity;
using UnityEngine;

namespace Assets.Scripts.GUI.MenuState
{
	public class StatusState : State<MainMenuTrigger>
	{
		private MainMenuTrigger? trigger;

		private HorizontalMenu menu;
		private PartyStatusPanel panel;		

		public StatusState(GameObject owner, BorderTextures textures, FontProperties fontProperties)
			: base(owner)
		{
			menu = owner.GetComponent<HorizontalMenu>();
			panel = owner.GetComponent<PartyStatusPanel>();
			
			if (menu != null)
			{
				menu.textures = textures;
				menu.fontProperties = fontProperties;

				menu.Canceled += Canceled;
				menu.SelectionChanged += SelectionChanged;

				foreach (var pm in GameStateManager.Instance.GameData.Party)
				{
					var partyMember = pm;
					if (partyMember == null)
						continue;

					// These never need a click action as the work is done by the selection changed handler
					menu.AddMenuItem(new MenuItem(partyMember.Name, () => { }));
				}
			}

			if (panel != null)
			{
				panel.textures = textures;
				panel.fontProperties = fontProperties;

				panel.PartyMember = GameStateManager.Instance.GameData.Party[0];
			}
		}
				
		public override MainMenuTrigger GetTrigger()
		{
			return trigger ?? MainMenuTrigger.Nothing;
		}

		public override void Update()
		{			
		}

		public override void OnEnter()
		{
			menu.enabled = true;
			panel.enabled = true;
			trigger = null;
		}

		public override void OnExit()
		{
			menu.enabled = false;
			panel.enabled = false;
		}

		#region Event Handlers

		private void Canceled(object sender, System.EventArgs e)
		{
			trigger = MainMenuTrigger.ActionCanceled;
		}

		private void SelectionChanged(MenuItem newMenu)
		{
			if (menu == null || panel == null)
				return;

			int selectedIndex = menu.GetIndex(newMenu);
			
			if (selectedIndex > -1)
				panel.PartyMember = GameStateManager.Instance.GameData.Party[selectedIndex];
		}	

		#endregion
	}
}
