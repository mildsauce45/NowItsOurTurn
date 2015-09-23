using UnityEngine;

namespace FirstWave.Core.Extensions
{
	public static class GameObjectExtensions
	{
		public static T Instantiate<T>(this T obj) where T : Object
		{
			return (T)GameObject.Instantiate(obj);
		}
	}
}
