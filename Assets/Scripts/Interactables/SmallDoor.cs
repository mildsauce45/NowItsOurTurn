using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class SmallDoor : Interactable
	{
		public Sprite openSprite;
		public Sprite closedSprite;

		private bool isOpen;
		private MapManager mapManager;
		private SpriteRenderer spriteRenderer;

		public override bool AllowInteraction
		{
			get { return !isOpen; }
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
			}
		}
	}
}
