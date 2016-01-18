using FirstWave.Core.Extensions;
using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using FirstWave.Unity.Core.Input;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class NPC : Interactable
	{
		// Unlike simple interactables, we're going to key off of the stored converstation name, instead of raw text
		public string conversationName;

		public GameObject dialogPrefab;
		
		private Dialog dialogInstance;

		private Patroller patroller;
        private InputManager inputManager;

		void Start()
		{
			patroller = GetComponent<Patroller>();
            inputManager = InputManager.Instance;
		}

		public override bool DisableCharacterMotor
		{
			get { return true; }
		}

		public override bool AllowInteraction
		{
			get { return true; }
		}

		public override void Interact()
		{
			dialogInstance = dialogPrefab.Instantiate().GetComponent<Dialog>();

			dialogInstance.StartConversation(ConversationManager.Instance.GetConversation(conversationName));
			dialogInstance.OnClosed += Dialog_OnClosed;

			if (patroller != null)
				patroller.disableCharacterMotor = true;			
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			inputManager.Flush();

			Destroy(dialogInstance.gameObject);
			dialogInstance = null;

			if (patroller != null)
				patroller.disableCharacterMotor = false;

			//FindObjectOfType<InputManager>().DisableCharacterMotor = false;

			PostDialog();
		}

		protected virtual void PostDialog()
		{
		}
	}
}
