using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				var type = typeof(T);

				instance = (T)FindObjectOfType(type);

				if (instance == null)
					Debug.LogError(string.Format("An instance of {0} is needed in the scen, but none could be found.", typeof(T)));
			}

			return instance;
		}
	}
}
