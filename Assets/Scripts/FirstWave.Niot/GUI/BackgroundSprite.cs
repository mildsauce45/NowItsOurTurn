using UnityEngine;
using System.Collections;

public class BackgroundSprite : MonoBehaviour {

	public GameObject go;
	public Camera theCamera;

	// Use this for initialization
	void Start () {
		if (go == null)
			go = gameObject;

		if (theCamera == null)
			theCamera = Camera.allCameras[0];

		SpriteFunctions.ResizeSpriteToScreen(go, theCamera, 1, 1);
	}
	
}
