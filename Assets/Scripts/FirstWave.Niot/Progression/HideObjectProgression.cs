using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Niot.Progression
{
	public class HideObjectProgression : ProgressionTrigger
	{
		public GameObject toRemove;

		public override void Trigger(string progression)
		{
			if (toRemove)
				Destroy(toRemove.gameObject);

			if (!string.IsNullOrEmpty(progression))
				GameStateManager.Instance.GameData.StoryProgressions.Add(progression);
		}
	}
}
