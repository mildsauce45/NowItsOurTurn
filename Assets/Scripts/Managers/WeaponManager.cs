using System.Collections.Generic;
using System.Linq;
using FirstWave.Core.Extensions;
using FirstWave.Xml;
using FirstWave.Xml.Documents;
using UnityEngine;

namespace FirstWave.Niot.Game.Managers
{
	public class WeaponManager
	{
		private static WeaponManager me;
		public static WeaponManager Instance
		{
			get
			{
				if (me == null)
					me = new WeaponManager();

				return me;
			}
		}

		private IDictionary<int, Weapon> allWeapons;

		private WeaponManager()
		{
		}

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

			var doc = new XmlParser().Doc(weaponsAsset.text);

			var weaponNodes = doc.Value.Root.Children.Where(w => w.Name == "weapon");

			foreach (var weaponNode in weaponNodes)
			{
				var weapon = CreateWeapon(weaponNode);

				if (weapon != null)
					allWeapons.Add(weapon.Id, weapon);
			}
		}

		private Weapon CreateWeapon(Node weaponNode)
		{
			var weapon = new Weapon();

			weapon.Id = weaponNode["id"].Value.ToInt();
			weapon.Name = weaponNode["name"].Value;
			weapon.Power = weaponNode["power"].Value.ToInt();
			weapon.MagicPower = weaponNode["magicPower"].Value.ToInt();
			weapon.Value = weaponNode["value"].Value.ToInt();

			return weapon;
		}
	}
}
