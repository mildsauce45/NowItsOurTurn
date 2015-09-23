using FirstWave.Core.Extensions;
using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class NPC : Interactable
	{
		// Unlike simple interactables, we're going to key off of the stored converstation name, instead of raw text
		public string conversationName;

		public GameObject dialogPrefab;

		// This is here primarily so that the conversation doesn't restart immediately after it's ended
		public float InputDelay = 0.5f;

		private float passedTime;
		private bool startTimer;
		private Dialog dialogInstance;

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
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			InputManager.Instance.Flush();

			GameObject.Destroy(dialogInstance.gameObject);
			dialogInstance = null;

			FindObjectOfType<InputManager>().DisableCharacterMotor = false;
		}
	}
}
