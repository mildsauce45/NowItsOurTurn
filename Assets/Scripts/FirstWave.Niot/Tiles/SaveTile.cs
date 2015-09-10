using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstWave.Niot.Game.Data;
using FirstWave.Niot.Managers;
using FirstWave.TileMap;

namespace FirstWave.Niot.Tiles
{
	public class SaveTile : EventTile
	{
		private MapManager manager;

		public override void OnEnter()
		{
			if (manager == null)
				manager = FindObjectOfType<MapManager>();
			
			if (manager != null)
				GameStateManager.Instance.GameData.Save(manager.sceneName, this.transform.position);
		}
	}
}
