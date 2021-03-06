﻿using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using FristWave.Niot.GUI;
using FirstWave.Core.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirstWave.Unity.Core.Input;

public class TiledHeroController : MonoBehaviour
{
	public float MoveSpeed = 5f;

	private bool IsMoving = false;

	public Transform InteractionCheck;
	public LayerMask InteractionLayer;
	public GameObject menuPrefab;

	private Vector3 target;
	private MapManager mapManager;
	private Animator anim;
    private InputManager inputManager;

	void Start()
	{
		target = transform.position;

		mapManager = FindObjectOfType<MapManager>();
        
		anim = GetComponent<Animator>();

        inputManager = InputManager.Instance;
	}

	void Update()
	{
		//if (InputManager.DisableCharacterMotor)
		//	return;

		if (!IsMoving && inputManager.KeyReleased("Interact"))
			HandleInteraction();
		else if (!IsMoving && inputManager.KeyReleased("Menu"))
			MainMenu();
		else
			MyMovement();
	}

	public void SetTarget(Vector3 newTarget)
	{
		target = newTarget;
		IsMoving = false;
	}

	public void SetDirection(Directions direction)
	{
		float x = 0f;
		float y = 0f;

		switch (direction)
		{
			case Directions.Up:
				y = 1f;
				break;
			case Directions.Down:
				y = -1f;
				break;
			case Directions.Left:
				x = -1f;
				break;
			case Directions.Right:
				x = 1f;
				break;
		}

		if (anim != null)
		{
			anim.SetFloat("InputX", x);
			anim.SetFloat("InputY", y);
		}
	}

	private void MyMovement()
	{
		if (IsMoving)
		{
			Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);

			// At a certain point just clamp this down
			if ((newPosition - target).magnitude < 0.0125f)
			{
				transform.position = target;
				IsMoving = false;

				HandleEventTiles();
				HandleBoatTransition();

				if (mapManager.encounterChance > 0 && Random.Range(0, 100) <= mapManager.encounterChance)
				{
					TransitionManager.Instance.playerPosition = target;
					SceneManager.LoadScene("TurnBasedBattle");
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

			// Check to see if the target tile can be moved onto
			if (mapManager.Impassables.ContainsKey(target))
			{
				Impassable i = mapManager.Impassables[target];
				if (!i.Contingent || !i.CanPass(transform))
					return;
			}

			IsMoving = target != transform.position;
		}
	}

	private void HandleEventTiles()
	{
		if (mapManager.EventTiles.ContainsKey(transform.position))
		{
			var et = mapManager.EventTiles[transform.position];
			if (et != null)
				et.OnEnter();
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
				//inputManager.DisableCharacterMotor = i.DisableCharacterMotor;
				i.Interact();
			}
		}
	}

	private void MainMenu()
	{
		var menu = menuPrefab.Instantiate().GetComponent<MainMenu>();		

		menu.initiator = this;
		menu.enabled = true;
	}
}
