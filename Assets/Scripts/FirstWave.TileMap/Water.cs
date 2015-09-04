using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.TileMap
{
	public class Water : Impassable
	{
		private MapManager mapManager;

		void Start()
		{
			mapManager = GameObject.FindObjectOfType<MapManager>();
		}

		public override bool CanPass(Transform player)
		{
			// In the future we need to not do the whole FindObjectOfType on the tile boat controller but read some flag on a passed in object here
			return mapManager.Boat != null ? (mapManager.Boat.transform.position == gameObject.transform.position || GameObject.FindObjectOfType<TileBoatController>() != null) : false;
		}
	}
}