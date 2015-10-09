using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Niot.Progression
{
	public enum TriggerType
	{
		OnStart,
		OnEnter
	}

	public class StoryTrigger : MonoBehaviour
	{
		public string progression;

		public string[] requiredProgression;

		public TriggerType triggerType;

		public GameObject triggerHandler;

		void Start()
		{
			if (triggerType == TriggerType.OnStart && PlayerHasRequiredProgression())
				HandleTrigger();
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (triggerType == TriggerType.OnEnter && PlayerHasRequiredProgression())
				HandleTrigger();			
		}

		private bool PlayerHasRequiredProgression()
		{
			if (!string.IsNullOrEmpty(progression) && GameStateManager.Instance.GameData.StoryProgressions.Contains(progression))
				return false;

			if (requiredProgression == null || requiredProgression.Length == 0)
				return true;

			foreach (var key in requiredProgression)
			{
				if (!GameStateManager.Instance.GameData.StoryProgressions.Contains(key))
					return false;
			}

			return true;
		}

		private void HandleTrigger()
		{
			if (!string.IsNullOrEmpty(progression))
				GameStateManager.Instance.GameData.StoryProgressions.Add(progression);

			if (triggerHandler)
				triggerHandler.GetComponent<ProgressionTrigger>().Trigger();
		}
	}
}
