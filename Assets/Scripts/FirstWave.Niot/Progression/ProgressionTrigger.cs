using UnityEngine;

namespace FirstWave.Niot.Progression
{
	public abstract class ProgressionTrigger : MonoBehaviour
	{
		public abstract void Trigger(string progression);
	}
}
