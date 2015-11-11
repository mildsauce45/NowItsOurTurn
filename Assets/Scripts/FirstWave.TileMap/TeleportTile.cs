using System.Collections;
using UnityEngine;

namespace FirstWave.TileMap
{
	public class TeleportTile : EventTile
	{
		public float speed = 3.5f;
		public Vector3 newLocation;

		public override void OnEnter()
		{
			var hero = FindObjectOfType<TiledHeroController>();
			var camera = FindObjectOfType<CameraFollow>();

			StartCoroutine(DelayedTeleport(hero, camera));			
		}

		private IEnumerator DelayedTeleport(TiledHeroController hero, CameraFollow camera)
		{
			hero.enabled = false;
			camera.enabled = false;

			hero.transform.position = new Vector3(newLocation.x, newLocation.y, 0);
			hero.SetTarget(newLocation);

			while ((newLocation - camera.transform.position).magnitude > 0.025f)
			{
				var newCameraPosition = Vector3.MoveTowards(camera.transform.position, newLocation, speed);

				camera.transform.position = newCameraPosition;

				yield return new WaitForEndOfFrame();
			}

			hero.enabled = true;
			camera.enabled = true;
		}
	}
}
