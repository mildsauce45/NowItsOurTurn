using UnityEngine;
using System.Collections;

public class FlashingSprite : MonoBehaviour {

	public float minimum = 0f;
	public float maximum = 1f;
	public float duration = 1.5f;

	private float from;
	private float to;
	private float startTime;

	private bool fadeIn;

	public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		fadeIn = true;

		SetFadeValues();
	}
	
	// Update is called once per frame
	void Update () {
		float t = (Time.time - startTime) / duration;

		var result = Mathf.SmoothStep(from, to, t);

		sprite.color = new Color(1f, 1f, 1f, result);		

		if (result >= 1f || result <= 0f)
		{
			fadeIn = !fadeIn;
			SetFadeValues();
		}
	}

	private void SetFadeValues()
	{
		startTime = Time.time;

		if (fadeIn)
		{
			from = minimum;
			to = maximum;
		}
		else
		{
			from = maximum;
			to = minimum;
		}
	}
}
