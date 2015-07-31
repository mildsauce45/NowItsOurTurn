using UnityEngine;
using System.Collections;

public class FireParticleSystem : MonoBehaviour
{
	public ParticleSystem particles;

	// Use this for initialization
	void Start()
	{
		if (particles != null)
			particles.GetComponent<Renderer>().sortingLayerName = "ParticleEffects";
	}
}
