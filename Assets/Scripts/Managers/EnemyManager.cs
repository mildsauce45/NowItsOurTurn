using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FirstWave.Core.Extensions;
using UnityEngine;

namespace FirstWave.Niot.Game.Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        private IDictionary<string, Enemy> allEnemies;

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

            var enemiesNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "enemy");

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

                var textureString = enemyNode.GetAttributeValue("texture");
                if (!string.IsNullOrEmpty(textureString))
                {
                    var texture = Resources.Load("Images/Enemies/" + textureString) as Texture2D;
                    if (texture != null)
                        enemy.Sprite = texture;
                }

                allEnemies.Add(enemy.Name, enemy);
            }
        }
    }
}
