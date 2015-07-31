using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f; // How smoothly the camera catches up
	public float ySmooth = 8f;
	public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

	public Transform player;

	bool CheckXMargin() {
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}

	bool CheckYMargin() {
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}

	void Update () {
		TrackPlayer();
	}

	void TrackPlayer() {
		float targetX = player.position.x;
		float targetY = player.position.y;

		//if (CheckXMargin())
		//	targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		//if (CheckYMargin())
		//	targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		//transform.position = new Vector3(targetX, targetY, transform.position.z);
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
