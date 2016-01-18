using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class SmallDoor : Interactable
	{
		public Sprite openSprite;
		public Sprite closedSprite;
		public string requiredKey;

		private bool isOpen;
		private MapManager mapManager;
		private SpriteRenderer spriteRenderer;

		public override bool AllowInteraction
		{
			get { return !isOpen && (string.IsNullOrEmpty(requiredKey) || PlayerHasKey(requiredKey)); }
		}

		public override bool DisableCharacterMotor
		{
			get { return false; }
		}

		void Start()
		{
			mapManager = FindObjectOfType<MapManager>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public override void Interact()
		{
			if (!isOpen)
			{
				spriteRenderer.sprite = openSprite;
				mapManager.Remove(GetComponent<Impassable>());

				isOpen = true;
			}
		}

		private bool PlayerHasKey(string key)
		{
			return GameStateManager.Instance.GameData.Collectibles.Contains(key);
		}
	}
}
