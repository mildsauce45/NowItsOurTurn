using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FirstWave.Niot.Game.Behaviors;
using UnityEngine;

namespace FirstWave.Niot.Game
{
	public class Enemy : Combatant
	{
		private static IList<Type> ENEMY_BEHAVIORS { get; set; }

		public Texture2D Sprite { get; set; }

		// Maybe don't need this, I could just actually use the sprite size
		public EnemySize Size { get; set; }

		public int Experience { get; set; }
		public int Gold { get; set; }

		public string BehaviorType { get; set; }

		public IEnemyBehavior Behavior { get; set; }

		public override int AttackPower
		{
			get { return this.Strength; }
		}

		public override int MagicPower
		{
			get { return this.Will; }
		}

		public override int Defense
		{
			get { return Endurance; }
		}

		static Enemy()
		{
			var behaviorType = typeof(IEnemyBehavior);

			ENEMY_BEHAVIORS = Assembly.GetExecutingAssembly().GetTypes().Where(t => behaviorType.IsAssignableFrom(t) && t != behaviorType).ToList();
		}

		public Enemy(string name, int maxHP)
		{
			Name = name;

			MaxHP = maxHP;
			CurrentHP = maxHP;
		}

		/// <summary>
		/// This will create a separate instance for use in battle
		/// </summary>		
		public Enemy(Enemy toClone)
			: this(toClone.Name, toClone.MaxHP)
		{
			this.Sprite = toClone.Sprite;

			this.Speed = toClone.Speed;
			this.Strength = toClone.Strength;
			this.Experience = toClone.Experience;
			this.Gold = toClone.Gold;
			this.BehaviorType = toClone.BehaviorType;

			CreateBehavior();

			if (toClone.EquippedAbilities != null)
				this.EquippedAbilities = toClone.EquippedAbilities.Select(a => a.Clone()).ToArray();

			if (toClone.EquippedFinishers != null)
				this.EquippedFinishers = toClone.EquippedFinishers.Select(a => a.Clone()).ToArray();
		}

		private void CreateBehavior()
		{
			if (!string.IsNullOrEmpty(BehaviorType))
			{
				var type = ENEMY_BEHAVIORS.FirstOrDefault(t => t.Name == (BehaviorType + "Behavior"));
				if (type != null)
					this.Behavior = Activator.CreateInstance(type, new[] { this }) as IEnemyBehavior;
			}
		}
	}
}