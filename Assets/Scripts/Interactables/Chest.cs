using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using FirstWave.Core.Extensions;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class Chest : Interactable
	{
		public string chestKey;

		public int itemId;

		public Sprite closedSprite;
		public Sprite openedSprite;

		public GameObject dialogPrefab;

		private bool opened;
		private SpriteRenderer rendererComponent;
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

			opened = GameStateManager.Instance.GameData.Collectibles.Contains(chestKey);

			UpdateRendererComponent();
		}

		public override void Interact()
		{
			GameStateManager.Instance.GameData.Collectibles.Add(chestKey);
			opened = true;

			UpdateRendererComponent();

			dialogInstance = dialogPrefab.Instantiate().GetComponent<Dialog>();

			dialogInstance.StartConversation("You found an item!");
			dialogInstance.OnClosed += Dialog_OnClosed;			
		}

		private void UpdateRendererComponent()
		{
			rendererComponent.sprite = opened ? openedSprite : closedSprite;
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			InputManager.Instance.Flush();

			Destroy(dialogInstance.gameObject);
			dialogInstance = null;

			FindObjectOfType<InputManager>().DisableCharacterMotor = false;
		}
	}
}
