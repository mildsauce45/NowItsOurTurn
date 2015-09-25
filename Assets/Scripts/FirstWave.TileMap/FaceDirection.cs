using FirstWave.TileMap;
using UnityEngine;

namespace Assets.Scripts.FirstWave.TileMap
{
	public class FaceDirection : MonoBehaviour
	{
		public Directions direction;

		private Animator anim;

		void Start()
		{
			anim = GetComponent<Animator>();
		}

		void FixedUpdate()
		{
			float inputX = 0;
			float inputY = 0;

			switch (direction)
			{
				case Directions.Up:
					inputY = 1;
					break;
				case Directions.Down:
					inputY = -1;
					break;
				case Directions.Left:
					inputX = -1;
					break;
				case Directions.Right:
					inputX = 1;
					break;
			}

			anim.SetFloat("InputX", inputX);
			anim.SetFloat("InputY", inputY);
		}
	}
}
