using FirstWave.Niot.Interactables;
using FirstWave.Niot.Managers;
using UnityEngine;

namespace Assets.Scripts.Interactables
{
	public class StartFightNPC : NPC
	{
		public string[] enemies;
		public string postFightProgression;

		protected override void PostDialog()
		{
			TransitionManager.Instance.enemiesToFight = enemies;
			TransitionManager.Instance.postFightProgression = postFightProgression;
			TransitionManager.Instance.sceneToLoad = FindObjectOfType<MapManager>().sceneName;
			TransitionManager.Instance.playerPosition = FindObjectOfType<TiledHeroController>().transform.position;

			Application.LoadLevel("TurnBasedBattle");
		}
	}
}
