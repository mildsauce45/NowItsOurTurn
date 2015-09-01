using UnityEngine;
using System.Collections;
using FirstWave.Niot.Managers;

public class TiledHeroController : MonoBehaviour
{
	public float MoveSpeed = 5f;
	public Vector2 MaxCoordinates;

	private bool IsMoving = false;

	public Transform InteractionCheck;
	public LayerMask InteractionLayer;

	private Vector3 target;
	private MapManager mapManager;
	private InputManager inputManager;
	private Animator anim;

	// Use this for initialization
	void Start()
	{
		target = transform.position;

		mapManager = FindObjectOfType<MapManager>();
		inputManager = FindObjectOfType<InputManager>();

		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (inputManager.DisableCharacterMotor)
			return;

		if (!IsMoving && inputManager.KeyReleased("Interact"))
			HandleInteraction();
		else
			MyMovement();
	}

	private void MyMovement()
	{
		if (IsMoving)
		{
			Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);

			// At a certain point just clamp this down
			if ((newPosition - target).magnitude < 0.025f)
			{
				transform.position = target;
				IsMoving = false;

				HandleSceneTransition();
				HandleBoatTransition();

				if (mapManager.encounterChance > 0 && Random.Range(0, 100) <= mapManager.encounterChance)
				{
					BattleTransitionManager.Instance.playerPosition = target;
					Application.LoadLevel("TurnBasedBattle");
				}
			}
			else
				transform.position = newPosition;

			return;
		}
		else
		{
			var moveX = Input.GetAxisRaw("Horizontal");
			var moveY = Input.GetAxisRaw("Vertical");

			if (moveX == 0 && moveY == 0)
				return;

			var targetX = 0;
			var targetY = 0;

			if (Mathf.Abs(moveX) > 0)
				targetX = (int)moveX;
			else if (Mathf.Abs(moveY) > 0)
				targetY = (int)moveY;

			if (InteractionCheck != null)
				InteractionCheck.transform.localPosition = new Vector3(targetX, targetY, 0);

			target = new Vector3(transform.position.x + targetX, transform.position.y + targetY, 0);

			anim.SetBool("Walking", targetX != 0 && targetY != 0);
			anim.SetFloat("InputX", targetX);
			anim.SetFloat("InputY", targetY);

			// First do some bounds checking
			if (target.x < 0 || target.y < 0 || target.x > MaxCoordinates.x || target.y > MaxCoordinates.y)
				return;
			
			// Check to see if the target tile can be moved onto
			if (mapManager.Impassables.ContainsKey(target))
			{
				Impassable i = mapManager.Impassables[target];
				if (!i.Contingent || !i.CanPass())
					return;
			}

			IsMoving = target != transform.position;
		}
	}

	private void HandleSceneTransition()
	{
		if (mapManager.SceneLoaders.ContainsKey(transform.position))
		{
			SceneLoader sl = mapManager.SceneLoaders[transform.position];
			if (sl != null)
				Application.LoadLevel(sl.Scene);
		}
	}

	private void HandleBoatTransition()
	{
		if (mapManager.Boat != null && mapManager.Boat.transform.position == transform.position)
		{
			this.gameObject.SetActive(false);

			mapManager.Boat.gameObject.AddComponent<TileBoatController>();
			mapManager.Boat.gameObject.GetComponent<TileBoatController>().Hero = this;
		}
	}

	private void HandleInteraction()
	{
		var positionToCheck = gameObject.transform.position + InteractionCheck.transform.localPosition;

		if (mapManager.Interactables.ContainsKey(positionToCheck))
		{
			Interactable i = mapManager.Interactables[positionToCheck];
			if (i.AllowInteraction)
			{
				inputManager.DisableCharacterMotor = i.DisableCharacterMotor;
				i.Interact();
			}
		}
	}
}
