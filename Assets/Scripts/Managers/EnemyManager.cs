using System.Collections.Generic;
using System.Linq;
using FirstWave.Core.Extensions;
using FirstWave.Xml;
using FirstWave.Xml.Documents;
using UnityEngine;
using System.Xml;

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

			var doc = new XmlDocument();
			doc.LoadXml(enemiesAsset.text);

			var enemiesNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(n => n.Name == "enemy");

			foreach (var enemyNode in enemiesNodes)
			{
				var name = enemyNode.GetAttributeValue("name");

				var maxHP = enemyNode.GetAttributeValue("hp").ToInt();

				var enemy = new Enemy(name, maxHP);

				enemy.Speed = enemyNode.GetAttributeValue("speed").ToInt();
				enemy.Strength = enemyNode.GetAttributeValue("strength").ToInt();
				enemy.Will = enemyNode.GetAttributeValue("will").ToInt();
				enemy.Endurance = enemyNode.GetAttributeValue("endurance").ToInt();

				enemy.Experience = enemyNode.GetAttributeValue("xp").ToInt();
				enemy.Gold = enemyNode.GetAttributeValue("gold").ToInt();
				enemy.BehaviorType = enemyNode.GetAttributeValue("behavior");

				var textureString = enemyNode.GetAttributeValue("texture");
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

		private void ParseAbilities(Enemy enemy, XmlNode enemyNode)
		{
			var abilitiesNode = enemyNode.ChildNodes.OfType<XmlNode>().FirstOrDefault(x => x.Name == "abilities");
			if (abilitiesNode == null)
				return;

			var knownAbilities = new List<Ability>();

			foreach (var abilityNode in abilitiesNode.ChildNodes.OfType<XmlNode>())
			{
				var abilityName = abilityNode.GetAttributeValue("name");

				var ability = AbilityManager.Instance.GetAbility(abilityName);
				if (ability != null)
					knownAbilities.Add(ability);
			}

			enemy.EquippedAbilities = knownAbilities.Where(a => !a.IsFinisher).ToArray();
			enemy.EquippedFinishers = knownAbilities.Where(a => a.IsFinisher).ToArray();
		}
	}
}
