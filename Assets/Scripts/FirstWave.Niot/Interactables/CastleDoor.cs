using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class CastleDoor : MonoBehaviour
	{
		public Sprite closedSprite;
		public Sprite openSprite;

		private CastleDoorInteractable[] doorParts;
		private SpriteRenderer spriteRenderer;
		private MapManager mapManager;

		void Start()
		{
			doorParts = GetComponentsInChildren<CastleDoorInteractable>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			mapManager = FindObjectOfType<MapManager>();
		}

		public void DoorOpened()
		{
			spriteRenderer.sprite = openSprite;

			foreach (var dp in doorParts)
			{
				dp.DoorOpened();
				mapManager.Remove(dp.GetComponent<Impassable>());
			}
			
			GetComponent<Animator>().enabled = false;
		}
	}
}
