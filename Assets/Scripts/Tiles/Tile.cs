using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public bool IsPassable { get; set; }

	void Start()
	{
		IsPassable = true;
	}
}
