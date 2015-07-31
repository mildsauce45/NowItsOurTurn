using UnityEngine;
using System.Collections;

public class Impassable : MonoBehaviour {

	public bool Contingent;

	public virtual bool CanPass(/* Player Model Passed in here */)
	{
		return false;
	}
}
