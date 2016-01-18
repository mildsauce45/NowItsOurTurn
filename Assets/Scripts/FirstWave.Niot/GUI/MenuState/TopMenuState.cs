using FirstWave.Core.GUI;
using FirstWave.Core.GUI.Menus;
using FirstWave.Niot.GUI.Controls;
using FirstWave.Niot.GUI.MenuState;
using FirstWave.Niot.Managers;
using FirstWave.StateMachine.Unity;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.GUI.MenuState
{
	public class TopMenuState : State<MainMenuTrigger>
	{
		private const string MENU_LABEL_STATUS = "Status";
		private const string MENU_LABEL_EQUIP = "Equip";
		private const string MENU_LABEL_ABILITIES = "Abilities";
		private const string MENU_LABEL_ITEMS = "Items";

		private bool canceled;
		private MainMenuTrigger? trigger;

		private Menu menu;
		private BorderedLabel menuItemDescription;
		private BorderedLabel partyGold;

		public TopMenuState(GameObject owner, BorderTextures textures, FontProperties fontProperties)
			: base(owner)
		{
			menu = owner.GetComponent<Menu>();
			menuItemDescription = owner.GetComponent<BorderedLabel>();
			partyGold = owner.GetComponentsInChildren<BorderedLabel>().Where(bl => bl.gameObject.name == "PartyGoldHolder").FirstOrDefault();

			if (menu != null)
			{
				menu.textures = textures;
				menu.fontProperties = fontProperties;

				menu.Canceled += Canceled;
				menu.SelectionChanged += SelectionChanged;

				menu.AddMenuItem(new MenuItem(MENU_LABEL_STATUS, () => trigger = MainMenuTrigger.StatusSelected));
				menu.AddMenuItem(new MenuItem(MENU_LABEL_EQUIP, () => { }));
				menu.AddMenuItem(new MenuItem(MENU_LABEL_ABILITIES, () => { }));
				menu.AddMenuItem(new MenuItem(MENU_LABEL_ITEMS, () => { }));
			}			

			if (menuItemDescription != null)
			{
				menuItemDescription.textures = textures;
				menuItemDescription.fontProperties = fontProperties;

				menuItemDescription.margin.left = menu.margin.left;
				menuItemDescription.margin.top = menu.margin.top + menu.size.y + 20;
			}

			if (partyGold != null)
			{
				partyGold.textures = textures;
				partyGold.fontProperties = fontProperties;

				partyGold.margin.top = menu.margin.top;
				partyGold.margin.right = menu.margin.left;

				partyGold.text = string.Format("Gold: {0}G", GameStateManager.Instance.GameData.Gold);
			}
		}

		private void SelectionChanged(MenuItem newSelection)
		{
			if (menuItemDescription != null)
				menuItemDescription.text = GetMenuItemDescription(newSelection.Text);
		}

		public override MainMenuTrigger GetTrigger()
		{
			if (canceled)
				return MainMenuTrigger.MenuExited;

			return trigger ?? MainMenuTrigger.Nothing;
		}

		public override void Update()
		{
		}

		public override void OnEnter()
		{
			menu.enabled = true;
			menu.DisableInput = false;

			menuItemDescription.enabled = true;
			partyGold.enabled = true;

			trigger = null;
		}

		public override void OnExit()
		{
			menu.DisableInput = true;
			menuItemDescription.enabled = false;
			partyGold.enabled = false;
		}

		private void Canceled(object sender, System.EventArgs e)
		{
			canceled = true;
		}

		private string GetMenuItemDescription(string menuItemText)
		{
			Debug.Log(menuItemText);

			switch (menuItemText)
			{
				case MENU_LABEL_STATUS:
					return "View information about each party member and the party in general.";
				case MENU_LABEL_EQUIP:
					return "Equip your party with weapons and armor you have procured.";
				case MENU_LABEL_ABILITIES:
					return "Set the abilities and finishers you'll use in combat.";
				case MENU_LABEL_ITEMS:
					return "View key items that you've found on your journey.";
				default:
					return "Foobar";
			}
		}
	}
}
