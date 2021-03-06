﻿using FirstWave.Niot.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirstWave.TileMap
{
    public class SceneLoader : EventTile
	{
		public string Scene;
		public Vector2 StartCoordinates;
		public Directions InitialFacingDirection;

		public override void OnEnter()
		{
			TransitionManager.Instance.playerPosition = StartCoordinates;
			TransitionManager.Instance.direction = InitialFacingDirection;

			SceneManager.LoadScene(Scene);
		}
	}
}