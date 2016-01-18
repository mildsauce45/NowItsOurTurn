using System.Linq;
using FirstWave.Core;
using FirstWave.Core.GUI;
using FirstWave.Niot.Game;
using FirstWave.StateMachine.Unity;
using UnityEngine;

namespace FirstWave.Niot.Battle
{
	public class DisplayCombatantsState : State<TurnBasedBattleTriggers?>
	{
		private Enemy[] enemies;

		private MessageBox messageBox;

		private CombinedTimer timer;

		public DisplayCombatantsState(GameObject owner)
			: base(owner)
		{
			this.enemies = TurnBasedBattleManager.Instance.EnemyParty;

			this.messageBox = owner.GetComponent<MessageBox>();
			this.messageBox.enabled = false;
		}

		public override void OnEnter()
		{
			var enemiesText = string.Join(", ", enemies.Select(e => e.Name).ToArray());

			var textDisplayTimer = new TextDisplayTimer(messageBox.characterDelay, "You are approached by " + enemiesText);
			this.messageBox.textSource = textDisplayTimer;

			timer = new CombinedTimer(textDisplayTimer, new Timer(1f));

			// Now that we have set the text, start the main timer
			timer.Start();

			this.messageBox.enabled = true;
		}

		public override void OnExit()
		{
			this.messageBox.enabled = false;
		}

		public override void Update()
		{
			timer.Update();
		}

		public override TurnBasedBattleTriggers? GetTrigger()
		{
			if (timer.IsComplete)
				return TurnBasedBattleTriggers.CombatantsDisplayed;

			return null;
		}
	}
}
