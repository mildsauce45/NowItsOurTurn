using UnityEngine;

namespace FirstWave.TileMap
{
	public class Impassable : MonoBehaviour
	{
		public bool Contingent;

		public virtual bool CanPass(Transform player)
		{
			return false;
		}
	}
}