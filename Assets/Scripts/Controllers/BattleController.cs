using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour
{
	private InputManager inputManger;
	private PlayerCharacter playerCharacter;

	private bool acceptingInput;

	// Use this for initialization
	void Start()
	{
		inputManger = FindObjectOfType<InputManager>();		
		playerCharacter = GetComponent<PlayerCharacter>();

		acceptingInput = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (!acceptingInput)
			return;

		bool attacked = CheckAbilities();

		if (!attacked)
		{
			if (inputManger.KeyReleased("Defend"))
			{
				Debug.Log("Defended");
			}
		}
	}

	private bool CheckAbilities()
	{
		if (inputManger.KeyReleased("Ability1") && playerCharacter.EquippedAbilities[0] != 0)
		{			
		}

		if (inputManger.KeyReleased("Ability2") && playerCharacter.EquippedAbilities[1] != 0)
		{			
		}

		if (inputManger.KeyReleased("Ability3") && playerCharacter.EquippedAbilities[2] != 0)
		{			
		}

		return false;
	}
}
