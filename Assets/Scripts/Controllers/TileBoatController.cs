using FirstWave.Niot.Managers;
using FirstWave.TileMap;
using UnityEngine;

public class TileBoatController : MonoBehaviour {

	public float MoveSpeed = 5f;
	public TiledHeroController Hero;

	private bool IsMoving = false;

	private Vector3 target;
	private MapManager mapManager;

	// Use this for initialization
	void Start () {
		target = transform.position;

		mapManager = GameObject.FindObjectOfType<MapManager>();
	}
	
	// Update is called once per frame
	void Update () {
		DoMovement();
	}

	private void DoMovement()
	{
		if (IsMoving)
		{
			Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MoveSpeed);

			// At a certain point just clamp this down
			if ((newPosition - target).magnitude < 0.025f)
			{
				transform.position = target;
				IsMoving = false;

				Hero.transform.position = target;
			}
			else
				transform.position = newPosition;

			return;
		}
		else
		{
			var moveX = Input.GetAxis("Horizontal");
			var moveY = Input.GetAxis("Vertical");

			if (moveX == 0 && moveY == 0)
				return;

			var targetX = 0;
			var targetY = 0;

			if (moveX > 0)
				targetX = 1;
			else if (moveX < 0)
				targetX = -1;
			else if (moveY > 0)
				targetY = 1;
			else if (moveY < 0)
				targetY = -1;

			target = new Vector3(transform.position.x + targetX, transform.position.y + targetY, 0);

			// First do some bounds checking
			if (target.x < 0 || target.y < 0)
			{
				target = Vector3.zero;
				return;
			}

			// Check to see if the target tile can be moved onto
			if (mapManager.Impassables.ContainsKey(target))
			{
				Impassable i = mapManager.Impassables[target];
				if (!i.Contingent || !i.CanPass(transform))
				{
					target = Vector3.zero;
					return;
				}
			}
			else if (!mapManager.Impassables.ContainsKey(target))
			{
				Hero.gameObject.SetActive(true);
				Destroy(this);
			}

			IsMoving = target != transform.position;
		}
	}
}
