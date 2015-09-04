using System.Collections.Generic;
using UnityEngine;

namespace FirstWave.Niot.Managers
{
	public class MapManager : MonoBehaviour
	{

		public IDictionary<Vector3, Impassable> Impassables { get; private set; }
		public IDictionary<Vector3, SceneLoader> SceneLoaders { get; private set; }
		public IDictionary<Vector3, Interactable> Interactables { get; private set; }

		public Boat Boat { get; private set; }

		public string sceneName;
		public float encounterChance;

		void Awake()
		{
			BattleTransitionManager.Instance.sceneToLoad = sceneName;

			if (BattleTransitionManager.Instance.playerPosition.HasValue)
			{
				FindObjectOfType<TiledHeroController>().transform.position = BattleTransitionManager.Instance.playerPosition.Value;
				BattleTransitionManager.Instance.playerPosition = null;
			}
		}

		// Use this for initialization
		void Start()
		{
			Impassables = new Dictionary<Vector3, Impassable>();
			SceneLoaders = new Dictionary<Vector3, SceneLoader>();
			Interactables = new Dictionary<Vector3, Interactable>();

			var impassables = FindObjectsOfType<Impassable>();

			foreach (var i in impassables)
			{
				if (Impassables.ContainsKey(i.gameObject.transform.position))
				{
					Debug.Log("A duplicate tile exists at " + i.gameObject.transform.position + ": " + i.gameObject.name);
					continue;
				}

				Impassables.Add(i.gameObject.transform.position, i);
			}

			var sceneLoaders = FindObjectsOfType<SceneLoader>();

			foreach (var sl in sceneLoaders)
				SceneLoaders.Add(sl.gameObject.transform.position, sl);

			var interactables = FindObjectsOfType<Interactable>();

			foreach (var i in interactables)
				Interactables.Add(i.gameObject.transform.position, i);

			var boat = GameObject.FindObjectOfType<Boat>();
			if (boat != null)
				Boat = boat;
		}
	}
}
