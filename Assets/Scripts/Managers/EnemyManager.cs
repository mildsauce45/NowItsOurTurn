using System.Collections.Generic;
using System.Linq;
using FirstWave.Core.Extensions;
using FirstWave.Xml;
using FirstWave.Xml.Documents;
using UnityEngine;

namespace FirstWave.Niot.Game.Managers
{
	public class EnemyManager
	{
		private static EnemyManager me;
		public static EnemyManager Instance
		{
			get
			{
				if (me == null)
					me = new EnemyManager();

				return me;
			}
		}

		private IDictionary<string, Enemy> allEnemies;

		private EnemyManager()
		{
		}

		public Enemy GetEnemy(string name)
		{
			if (allEnemies == null || allEnemies.Count == 0)
				InitializeEnemies();

			if (allEnemies.ContainsKey(name))
				// Clone the stock implementation and return it
				return new Enemy(allEnemies[name]);

			return null;
		}

		public Enemy GetEnemy(int id)
		{
			if (allEnemies == null || allEnemies.Count == 0)
				InitializeEnemies();

			// Clone the stock implementation and return it
			return new Enemy(allEnemies.Values.ElementAt(id));
		}

		private void InitializeEnemies()
		{
			allEnemies = new Dictionary<string, Enemy>();

			var enemiesAsset = Resources.Load("Enemies") as TextAsset;

			var parser = new XmlParser();
			var doc = parser.Doc(enemiesAsset.text);

			var enemiesNodes = doc.Value.Root.Children.Where(n => n.Name == "enemy");

			foreach (var enemyNode in enemiesNodes)
			{
				var name = enemyNode["name"].Value;

				var maxHP = enemyNode["hp"].Value.ToInt();

				var enemy = new Enemy(name, maxHP);

				enemy.Speed = enemyNode["speed"].Value.ToInt();
				enemy.Strength = enemyNode["strength"].Value.ToInt();
				enemy.Will = enemyNode["will"].Value.ToInt();
				enemy.Endurance = enemyNode["endurance"].Value.ToInt();

				enemy.Experience = enemyNode["xp"].Value.ToInt();
				enemy.Gold = enemyNode["gold"].Value.ToInt();
				enemy.BehaviorType = enemyNode["behavior"].Value;

				var textureString = enemyNode["texture"].Value;
				if (!string.IsNullOrEmpty(textureString))
				{
					var texture = Resources.Load("Images/Enemies/" + textureString) as Texture2D;
					if (texture != null)
						enemy.Sprite = texture;
				}

				ParseAbilities(enemy, enemyNode);

				allEnemies.Add(enemy.Name, enemy);
			}
		}

		private void ParseAbilities(Enemy enemy, Node enemyNode)
		{
			var abilitiesNode = enemyNode.Children.FirstOrDefault(x => x.Name == "abilities");
			if (abilitiesNode == null)
				return;

			var knownAbilities = new List<Ability>();

			foreach (var abilityNode in abilitiesNode.Children)
			{
				var abilityName = abilityNode["name"].Value;

				var ability = AbilityManager.Instance.GetAbility(abilityName);
				if (ability != null)
					knownAbilities.Add(ability);
			}

			enemy.EquippedAbilities = knownAbilities.Where(a => !a.IsFinisher).ToArray();
			enemy.EquippedFinishers = knownAbilities.Where(a => a.IsFinisher).ToArray();
		}
	}
}
