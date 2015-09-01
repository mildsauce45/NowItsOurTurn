using UnityEngine;

namespace FirstWave.Niot.Managers
{
	public class BattleTransitionManager : Singleton<BattleTransitionManager>
	{
		public Vector2? playerPosition;
		public string sceneToLoad;

		void Start()
		{
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
	}
}
