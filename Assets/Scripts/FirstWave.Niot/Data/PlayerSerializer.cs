using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FirstWave.Niot.Game;
using FirstWave.Core.Extensions;
using FirstWave.Niot.Game.Managers;
using UnityEngine;

namespace FirstWave.Niot.Data
{
	public class PlayerSerializer
	{
		private const int VERSION = 1;

		private Player player;

		private StreamWriter writer;
		private StreamReader reader;

		public PlayerSerializer(Player player, StreamWriter writer)
		{
			this.player = player;
			this.writer = writer;	
		}

		public PlayerSerializer(StreamReader reader)
		{
			this.reader = reader;
		}

		public void Write()
		{
			if (player == null || writer == null)
				throw new Exception("Serializer not set up for writing");

			writer.WriteLine(VERSION);

			writer.WriteLine(player.Name);
			writer.WriteLine(player.MaxHP);

			writer.WriteLine(player.Level);
			writer.WriteLine(player.Exp);
			writer.WriteLine(player.Class);

			writer.WriteLine(player.Speed);
			writer.WriteLine(player.Strength);
			writer.WriteLine(player.Will);
			writer.WriteLine(player.Endurance);

			int abilityCount = player.AllAbilities.Count(a => a != null);

			writer.WriteLine(abilityCount);
			for (int i = 0; i < abilityCount; i++)
				writer.WriteLine(player.AllAbilities[i].Id);

			int equippedAbilityCount = player.EquippedAbilities.Count(a => a != null);

			writer.WriteLine(equippedAbilityCount);
			for (int i = 0; i < equippedAbilityCount; i++)
				writer.WriteLine(player.EquippedAbilities[i].Id);

			int equippedFinisherCount = player.EquippedAbilities.Count(a => a != null);

			writer.WriteLine(equippedFinisherCount);
			for (int i = 0; i < equippedFinisherCount; i++)
				writer.WriteLine(player.EquippedFinishers[i].Id);

			writer.WriteLine(player.Weapon.Id);
		}

		public Player Read()
		{
			var p = new Player();

			int version = reader.ReadLine().ToInt();

			switch (version)
			{
				case 1:
					p.Name = reader.ReadLine();
					p.MaxHP = reader.ReadLine().ToInt();
					p.CurrentHP = p.MaxHP;
					p.Level = reader.ReadLine().ToInt();
					p.Exp = reader.ReadLine().ToInt();
					p.Class = reader.ReadLine();
					p.Speed = reader.ReadLine().ToInt();
					p.Strength = reader.ReadLine().ToInt();
					p.Will = reader.ReadLine().ToInt();
					p.Endurance = reader.ReadLine().ToInt();

					int abilityCount = reader.ReadLine().ToInt();

					for (int i = 0; i < abilityCount; i++)
					{
						int abilityId = reader.ReadLine().ToInt();
						p.AllAbilities[i] = AbilityManager.Instance.GetAbility(abilityId);
					}

					int equippedAbilityCount = reader.ReadLine().ToInt();

					for (int i = 0; i < equippedAbilityCount; i++)
					{
						int abilityId = reader.ReadLine().ToInt();
						p.EquippedAbilities[i] = AbilityManager.Instance.GetAbility(abilityId);
					}

					int equippedFinisherCount = reader.ReadLine().ToInt();

					for (int i = 0; i < equippedFinisherCount; i++)
					{
						int abilityId = reader.ReadLine().ToInt();
						p.EquippedFinishers[i] = AbilityManager.Instance.GetAbility(abilityId);
					}

					int weaponId = reader.ReadLine().ToInt();					

					p.Weapon = WeaponManager.Instance.GetWeapon(weaponId);
					
					break;
				default:
					break;
			}

			return p;
		}
	}
}
