using System.Collections.Generic;
using FirstWave.TileMap;
using System.Linq;
using UnityEngine;

namespace FirstWave.Niot.Managers
{
	public class MapManager : MonoBehaviour
	{
		public IDictionary<Vector3, Impassable> Impassables { get; private set; }
		public IDictionary<Vector3, Interactable> Interactables { get; private set; }
		public IDictionary<Vector3, EventTile> EventTiles { get; private set; }

		public Boat Boat { get; private set; }

		public string sceneName;		
		public float encounterChance;

		public string exitToScene;
		public Vector2 exitToCoordinates;

		void Awake()
		{
			TransitionManager.Instance.sceneToLoad = sceneName;

			if (TransitionManager.Instance.playerPosition.HasValue)
			{
				var thc = FindObjectOfType<TiledHeroController>();

				thc.transform.position = TransitionManager.Instance.playerPosition.Value;
				thc.SetDirection(TransitionManager.Instance.direction);

				TransitionManager.Instance.playerPosition = null;
			}
		}

		// Use this for initialization
		void Start()
		{
			Impassables = new Dictionary<Vector3, Impassable>();			
			Interactables = new Dictionary<Vector3, Interactable>();
			EventTiles = new Dictionary<Vector3, EventTile>();

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

			var interactables = FindObjectsOfType<Interactable>();

			foreach (var i in interactables)
				Interactables.Add(i.gameObject.transform.position, i);

			var eventTiles = FindObjectsOfType<EventTile>();

			foreach (var et in eventTiles)
				EventTiles.Add(et.gameObject.transform.position, et);

			var boat = FindObjectOfType<Boat>();
			if (boat != null)
				Boat = boat;
		}

		/// <summary>
		/// Used to update the location of NPCs walking around the map
		/// </summary>
		/// <param name="i"></param>
		public void UpdateLocation(Impassable i)
		{
			foreach (var location in Impassables.Keys.ToList())
			{
				if (Impassables[location] == i)
				{
					Impassables.Remove(location);
					Impassables.Add(i.gameObject.transform.position, i);

					// If this NPC is also interactable, we need to update that location as well
					if (i.GetComponent<Interactable>())
					{
						Interactables.Remove(location);
						Interactables.Add(i.gameObject.transform.position, i.GetComponent<Interactable>());
					}
					break;
				}
			}
		}
	}
}
