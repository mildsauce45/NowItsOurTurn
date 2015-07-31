using System.Collections.Generic;
using System.Linq;
using FirstWave.Niot.Game;
using FirstWave.StateMachine.Unity;
using UnityEngine;

namespace FirstWave.Niot.Battle
{
	public class GetEnemyInputState : State<TurnBasedBattleTriggers?>
	{
		public Player[] Party { get; private set; }
		public Enemy[] Enemies { get; private set; }

		public IList<BattleCommand> Commands { get; private set; }
		
		public GetEnemyInputState(Player[] party, Enemy[] enemies, GameObject owner)
			: base(owner)
		{
			this.Party = party;
			this.Enemies = enemies;

			Commands = new List<BattleCommand>();
		}		

		public override void Update()
		{
			// I only ever plan to be in the Update state one time so I can just clear the old list of commands right now
			Commands.Clear();

			for (int i = 0; i < Enemies.Length; i++)
			{
				if (!Enemies[i].IsDead)
					CreateCommand(Enemies[i]);
			}
		}

		public override TurnBasedBattleTriggers? GetTrigger()
		{
			if (Commands.Count == Enemies.Count(e => !e.IsDead))
				return TurnBasedBattleTriggers.EnemyActionDecided;

			return null;
		}

		private void CreateCommand(Enemy actor)
		{
			// Basic behavior: randomly attack a party member
			var target = Party[Random.Range(0, Party.Length)];

			var command = new BattleCommand 
			{
				Actor = actor, 
				Ability = new Ability { Name = "Sword Swipe" }, 
				Target = new Target { TargetType = TargetTypes.Single, TheTarget = target } 
			};

			Commands.Add(command);
		}
	}
}