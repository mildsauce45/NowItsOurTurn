using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour
{
	public int Id;
	public string Name;
	public int[] EquippedAbilities;

	// Use this for initialization
	void Awake()
	{
		EquippedAbilities = new int[3];
		EquippedAbilities[0] = 1;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
