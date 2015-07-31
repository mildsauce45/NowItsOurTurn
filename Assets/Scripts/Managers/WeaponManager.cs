using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FirstWave.Core.Extensions;
using FirstWave.Niot.Game;
using UnityEngine;

namespace FirstWave.Niot.Game.Managers
{
	public class WeaponManager : Singleton<WeaponManager>
	{
		private IDictionary<int, Weapon> allWeapons;

		public Weapon GetWeapon(int weaponId)
		{
			if (allWeapons == null || allWeapons.Count == 0)
				InitializeWeapons();

			if (allWeapons.ContainsKey(weaponId))
				return allWeapons[weaponId].Clone();

			return null;
		}

		private void InitializeWeapons()
		{
			allWeapons = new Dictionary<int, Weapon>();

			var weaponsAsset = Resources.Load("Weapons") as TextAsset;

			var doc = new XmlDocument();
			doc.LoadXml(weaponsAsset.text);

			var weaponNodes = doc.FirstChild.ChildNodes.OfType<XmlNode>().Where(x => x.Name == "weapon");

			foreach (var weaponNode in weaponNodes)
			{
				var weapon = CreateWeapon(weaponNode);

				if (weapon != null)
					allWeapons.Add(weapon.Id, weapon);
			}
		}

		private Weapon CreateWeapon(XmlNode weaponNode)
		{
			var weapon = new Weapon();

			weapon.Id = weaponNode.GetAttributeValue("id").ToInt();
			weapon.Name = weaponNode.GetAttributeValue("name");
			weapon.Power = weaponNode.GetAttributeValue("power").ToInt();
			weapon.MagicPower = weaponNode.GetAttributeValue("magicPower").ToInt();
			weapon.Value = weaponNode.GetAttributeValue("value").ToInt();

			return weapon;
		}
	}
}
