using FirstWave.Core.Extensions;
using FirstWave.Niot.Data;
using FirstWave.Niot.Game.Managers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FirstWave.Niot.Game.Data
{
	public class GameData
	{
		/// <summary>
		/// Once versions of the game start making it out to friends, this will become important, because then they'd be able to move their
		/// save games forward without incident (fingers crossed)
		/// </summary>
		private const int VERSION = 1;

		public Player[] Party { get; private set; }

		public int Gold { get; set; }

		public HashSet<string> Collectibles { get; set; }
		public HashSet<string> StoryProgressions { get; set; }

		public string Scene { get; set; }
		public Vector2 Location { get; set; }

		public GameData()
		{
			Party = new Player[3];

			var leader = new Player("Gigas", 20);

			leader.Class = "Demon Lord";
			leader.Level = 1;
			leader.Exp = 0;

			leader.Speed = 6;
			leader.Strength = 5;
			leader.Will = 6;
			leader.Endurance = 4;

			leader.Weapon = WeaponManager.Instance.GetWeapon(1);

			Party[0] = leader;

			Collectibles = new HashSet<string>();
			StoryProgressions = new HashSet<string>();
		}

		public void Save(string sceneName, Vector2 currentLocation)
		{
			Scene = sceneName;
			Location = currentLocation;

			using (var fileStream = SaveDataHelper.GetGameDataStream(FileAccess.Write))
			{
				using (var writer = new StreamWriter(fileStream))
				{
					writer.WriteLine(VERSION);
					writer.WriteLine(Scene);

					writer.WriteLine(Location.x);
					writer.WriteLine(Location.y);

					writer.WriteLine(Gold);

					writer.WriteLine(Collectibles.Count);
					if (Collectibles.Count > 0)
					{
						foreach (var key in Collectibles)
							writer.WriteLine(key);
					}

					writer.WriteLine(StoryProgressions.Count);
					if (StoryProgressions.Count > 0)
					{
						foreach (var key in StoryProgressions)
							writer.WriteLine(key);
					}

					int partyMembers = Party.Count(p => p != null);
					writer.WriteLine(partyMembers);

					for (int i = 0; i < partyMembers; i++)
						new PlayerSerializer(Party[i], writer).Write();
				}

				fileStream.Close();
			}
		}

		public static GameData Load()
		{
			var gameData = new GameData();

			using (var fileStream = SaveDataHelper.GetGameDataStream(FileAccess.Read))
			{
				using (var reader = new StreamReader(fileStream))
				{
					int version = reader.ReadLine().ToInt();

					switch (version)
					{
						case 1:
							gameData.Scene = reader.ReadLine();

							var x = reader.ReadLine().ToFloat();
							var y = reader.ReadLine().ToFloat();
							gameData.Location = new Vector2(x, y);

							gameData.Gold = reader.ReadLine().ToInt();

							gameData.Collectibles = new HashSet<string>();
							int numCollectibles = reader.ReadLine().ToInt();
							if (numCollectibles > 0)
							{
								for (int i = 0; i < numCollectibles; i++)
									gameData.Collectibles.Add(reader.ReadLine());
							}

							gameData.StoryProgressions = new HashSet<string>();
							int numProgressions = reader.ReadLine().ToInt();
							if (numProgressions > 0)
							{
								for (int i = 0; i < numProgressions; i++)
									gameData.StoryProgressions.Add(reader.ReadLine());
							}

							int partyMemberCount = reader.ReadLine().ToInt();
							for (int i = 0; i < partyMemberCount; i++)
							{
								var partyMember = new PlayerSerializer(reader).Read();
								gameData.Party[i] = partyMember;
							}

							break;
						default:
							break;
					}
				}

				fileStream.Close();
			}

			return gameData;
		}
	}
}
