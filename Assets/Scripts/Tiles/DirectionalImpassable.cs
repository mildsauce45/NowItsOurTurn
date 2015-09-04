using UnityEngine;

namespace FirstWave.TileMap
{
	public class DirectionalImpassable : Impassable
	{
		public Directions impassableDirections;

		public override bool CanPass(Transform player)
		{
			if ((impassableDirections & Directions.Up) == Directions.Up && player.position.y > this.transform.position.y)
				return false;

			return true;
		}
	}
}
