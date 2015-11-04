using UnityEngine;

namespace FirstWave.Niot.Interactables
{
	public class CastleDoorInteractable : Interactable
	{
		private TiledHeroController hero;
		private Animator anim;

		void Start()
		{
			hero = FindObjectOfType<TiledHeroController>();
			anim = GetComponentInParent<Animator>();
		}

		public override bool AllowInteraction
		{
			get { return true; }
		}

		public override bool DisableCharacterMotor
		{
			// Gonna handle this on my own
			get { return false; }
		}

		public override void Interact()
		{
			hero.enabled = false;

			anim.SetTrigger("Opening");
		}

		public void DoorOpened()
		{
			hero.enabled = true;
		}
	}
}
