using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour
{

	public abstract bool DisableCharacterMotor { get; }
	public abstract bool AllowInteraction { get; }

	public virtual void Interact()
	{
	}
}
