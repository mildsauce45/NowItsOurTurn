using FirstWave.Core.Extensions;
using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FirstWave.Unity.Core.Input;

namespace Assets.Scripts.Interactables
{
	public class WitchDoctor : Interactable
	{
		public GameObject dialogPrefab;

		public float inputDelay = 0.5f;

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
			dialogInstance.StartConversation(CreateConversation());
			dialogInstance.OnClosed += Dialog_OnClosed;
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			InputManager.Instance.Flush();

			Destroy(dialogInstance.gameObject);
			dialogInstance = null;

			//FindObjectOfType<InputManager>().DisableCharacterMotor = false;
		}

		private Conversation CreateConversation()
		{
			var convo = new List<ConversationItem>();

			convo.Add(new LineItem("Hello! Allow me to gaze into the void and discern the amount of experience you need to grow stronger and crush the humans."));

			var party = GameStateManager.Instance.GameData.Party.Where(p => p != null).ToArray();

			for (int i = 0; i < party.Length; i++)
				convo.Add(new LineItem(string.Format("{0}, you need {1} experience to achieve the next level and continue your path to greatness.", party[i].Name, 100 - party[i].Exp)));

			convo.Add(new LineItem("Return any time comrades and may the elemental spirits and Lord Charon guide your path."));
			
			return new Conversation(convo.ToArray());
		}
	}
}
