using FirstWave.Core;
using FirstWave.Niot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FirstWave.TileMap
{
	public class Patroller : MonoBehaviour
	{
		public float moveSpeed = 3.5f;
		public float moveDelay = 10f;
		public bool disableCharacterMotor = false;

		private Vector3 target;
		private bool isMoving;

		private MapManager mapManager;
		private Animator anim;
		private Timer timer;

		void Start()
		{
			target = transform.position;

			mapManager = FindObjectOfType<MapManager>();

			anim = GetComponent<Animator>();			
		}

		void Update()
		{			
			if (disableCharacterMotor)
				return;

			HandleMovement();

			if (timer == null)
			{
				timer = new Timer(moveDelay, PickNewTarget);
				timer.Start();
            }

			timer.Update();			
		}

		private void HandleMovement()
		{
			if (isMoving)
			{
				Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

				// At a certain point just clamp down to the target
				if ((newPosition - target).magnitude < 0.025f)
				{
					transform.position = target;
					isMoving = false;

					mapManager.UpdateLocation(GetComponent<Impassable>());
				}
				else
				{
					transform.position = newPosition;
				}
			}
		}

		private void PickNewTarget()
		{
			int i = UnityEngine.Random.Range(0, 5);

			Directions? direction = null;

			switch (i)
			{
				case 1:
					direction = Directions.Up;
					break;
				case 2:
					direction = Directions.Down;
					break;
				case 3:
					direction = Directions.Left;
					break;
				case 4:
					direction = Directions.Right;
					break;
			}

			if (direction.HasValue)
			{
				var targetX = 0;
				var targetY = 0;

				if (direction == Directions.Up)
					targetY = 1;
				else if (direction == Directions.Down)
					targetY = -1;
				else if (direction == Directions.Left)
					targetX = -1;
				else if (direction == Directions.Right)
					targetX = 1;

				target = new Vector3(transform.position.x + targetX, transform.position.y + targetY, 0);

				anim.SetBool("Walking", targetX != 0 && targetY != 0);
				anim.SetFloat("InputX", targetX);
				anim.SetFloat("InputY", targetY);

				if (mapManager.Impassables.ContainsKey(target))
					return;

				isMoving = target != transform.position;
			}
		}
	}
}
