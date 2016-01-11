using FirstWave.Niot.Interactables;
using FirstWave.Niot.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Interactables
{
	public class StartFightNPC : NPC
	{
		public string[] enemies;
		public string postFightProgression;		
		public AudioClip musicOverride;

		protected override void PostDialog()
		{
			TransitionManager.Instance.enemiesToFight = enemies;
			TransitionManager.Instance.postFightProgression = postFightProgression;
			TransitionManager.Instance.sceneToLoad = FindObjectOfType<MapManager>().sceneName;
			TransitionManager.Instance.playerPosition = FindObjectOfType<TiledHeroController>().transform.position;
			TransitionManager.Instance.musicOverride = musicOverride;

            SceneManager.LoadScene("TurnBasedBattle");
		}
	}
}
