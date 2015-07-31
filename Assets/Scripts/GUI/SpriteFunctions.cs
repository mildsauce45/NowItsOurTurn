using UnityEngine;
using System.Collections;

public class SpriteFunctions {

	/// <summary>
	/// Resizes the sprite to screen.
	/// </summary>
	/// <param name="sprite">Sprite.</param>
	/// <param name="camera">Camera.</param>
	/// <param name="fitToScreenWidth">Fit to screen width. 1 is the default</param>
	/// <param name="fitToScreenHeight">Fit to screen height. 1 is the default</param>
	public static void ResizeSpriteToScreen(GameObject sprite, Camera camera, int fitToScreenWidth, int fitToScreenHeight)
	{
		var sr = sprite.GetComponent<SpriteRenderer>();

		if (sr == null || fitToScreenWidth < 0 || fitToScreenHeight < 0)
			return;

		sprite.transform.localScale = new Vector3(1, 1, 1);

		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		float worldScreenHeight = camera.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		float newXTransform = 1f;
		float newYTransform = 1f;

		if (fitToScreenWidth > 0)
			newXTransform = worldScreenWidth / width / fitToScreenWidth;

		if (fitToScreenHeight > 0)
			newYTransform = worldScreenHeight / height / fitToScreenHeight;

		sprite.transform.localScale = new Vector3(newXTransform, newYTransform, 1);
	}
}
