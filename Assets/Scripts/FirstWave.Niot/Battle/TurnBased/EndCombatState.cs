﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstWave.Niot.Game;
using FirstWave.Niot.Managers;
using FirstWave.StateMachine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirstWave.Niot.Battle
{
	public class EndCombatState : State<TurnBasedBattleTriggers?>
	{
		private Player[] Party { get; set; }
		private Enemy[] Enemies { get; set; }

		public EndCombatState(GameObject owner)
			: base(owner)
		{
		}

		public override void Update()
		{
			// Display some message regarding the new XP, gold, and perhaps levels the players have now

			if (!string.IsNullOrEmpty(TransitionManager.Instance.postFightProgression))
				GameStateManager.Instance.GameData.StoryProgressions.Add(TransitionManager.Instance.postFightProgression);

			TransitionManager.Instance.musicOverride = null;

            SceneManager.LoadScene(TransitionManager.Instance.sceneToLoad);
		}

		public override TurnBasedBattleTriggers? GetTrigger()
		{
			return null;
		}
	}
}
