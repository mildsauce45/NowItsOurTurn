using FirstWave.Core.Extensions;
using FirstWave.Core.GUI.Dialogs;
using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Niot.Progression
{
	public class DialogProgression : ProgressionTrigger
	{
		public string conversationName;
		public GameObject dialogPrefab;
		public TiledHeroController hero;

		private Dialog dialogInstance;

		public override void Trigger()
		{
			if (hero)
				hero.enabled = false;

			dialogInstance = dialogPrefab.Instantiate().GetComponent<Dialog>();
			dialogInstance.StartConversation(ConversationManager.Instance.GetConversation(conversationName));
			dialogInstance.OnClosed += Dialog_OnClosed;
		}

		private void Dialog_OnClosed(string branch)
		{
			dialogInstance.OnClosed -= Dialog_OnClosed;

			InputManager.Instance.Flush();

			Destroy(dialogInstance.gameObject);
			dialogInstance = null;

			if (hero)
				hero.enabled = true;
		}
	}
}
