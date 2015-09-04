using FirstWave.TileMap;
using UnityEngine;

namespace FirstWave.Niot.Managers
{
	public class TransitionManager : MonoBehaviour
	{
		private static TransitionManager _instance;		

		public static TransitionManager Instance
		{
			get
			{
				if (_instance == null)
				{
					var existing = FindObjectOfType<TransitionManager>();
					if (existing != null)
						_instance = existing;
					else
					{
						var managerObject = new GameObject();
						managerObject.name = "Transition Manager";

						_instance = managerObject.AddComponent<TransitionManager>();

						DontDestroyOnLoad(managerObject);
					}
				}

				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		public Vector2? playerPosition;
		public Directions direction;
		public string sceneToLoad;

		void Awake()
		{
			if (Instance == null)
			{
				DontDestroyOnLoad(gameObject);
				Instance = this;
			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}
