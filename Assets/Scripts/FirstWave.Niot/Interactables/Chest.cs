using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using FirstWave.Core.Extensions;
using UnityEngine;
using FirstWave.Unity.Core.Input;

namespace FirstWave.Niot.Interactables
{
	public class Chest : Interactable
	{
		public string chestKey;

		public int itemId;
		public int gold;

		public Sprite closedSprite;
		public Sprite openedSprite;

		public GameObject dialogPrefab;

		private bool opened;
		private SpriteRenderer rendererComponent;
        private InputManager inputManager;
		private Dialog dialogInstance;

		public override bool AllowInteraction
		{
			get { return !opened; }
		}

		public override bool DisableCharacterMotor
		{
			get { return !opened; }
		}

		void Start()
		{
			rendererComponent = GetComponent<SpriteRenderer>();
            inputManager = InputManager.Instance;

			opened = GameStateManager.Instance.GameData.Collectibles.Contains(chestKey);

			UpdateRendererComponent();
		}

		public override void Interact()
		{
			GameStateManager.Instance.GameData.Collectibles.Add(chestKey);
			opened = true;

			UpdateRendererComponent();

			dialogInstance = dialogPrefab.Instantiate().GetComponent<Dialog>();

			var convo = string.Empty;
			if (gold > 0)
			{
				convo = string.Format("You found {0} gold!", gold);
				GameStateManager.Instance.GameData.Gold += gold;
			}
			else
			{
				convo = string.Format("You found a {0}!", "item");
			}

			dialogInstance.StartConversation(convo);
			dialogInstance.OnClosed += Dialog_OnClosed;
		}

		private void UpdateRendererComponent()
		{
			rendererComponent.sprite = opened ? openedSprite : closedSprite;
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			inputManager.Flush();

			Destroy(dialogInstance.gameObject);
			dialogInstance = null;
		}
	}
}
