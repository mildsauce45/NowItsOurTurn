using UnityEngine;
using System.Collections;

public class Impassable : MonoBehaviour {

	public bool Contingent;

	public virtual bool CanPass(Transform player)
	{
		return false;
	}
}
