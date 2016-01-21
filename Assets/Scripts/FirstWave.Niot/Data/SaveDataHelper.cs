using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FirstWave.Niot.Game.Data
{
	public static class SaveDataHelper
	{
		#region Constants

		private static readonly string SAVE_DATA_LOCATION = Application.persistentDataPath + "/saveData.dat";

		private static readonly string SAVE_FILE_1 = Application.persistentDataPath + "/saveGameOne.sav";
		private static readonly string SAVE_FILE_2 = Application.persistentDataPath + "/saveGameTwo.sav";
		private static readonly string SAVE_FILE_3 = Application.persistentDataPath + "/saveGameThree.sav";

		#endregion

		public static bool DataLoaded
		{
			get { return SavedGameData.Any(gd => gd != null); }
		}

		private static GameData[] SavedGameData = new GameData[GameConstants.Ranges.NUM_OF_GAME_SAVES];
		private static int SaveGameIndex = -1;		

		public static bool SaveDataExists()
		{
			return File.Exists(SAVE_FILE_1) || File.Exists(SAVE_FILE_2) || File.Exists(SAVE_FILE_3);
		}		

		public static void ReadGameData()
		{
			if (File.Exists(SAVE_FILE_1))
			{
				SaveGameIndex = 0;

				SavedGameData[SaveGameIndex] = GameData.Load();
			}

			if (File.Exists(SAVE_FILE_2))
			{
				SaveGameIndex = 1;

				SavedGameData[SaveGameIndex] = GameData.Load();
			}

			if (File.Exists(SAVE_FILE_3))
			{
				SaveGameIndex = 2;

				SavedGameData[SaveGameIndex] = GameData.Load();
			}

			SaveGameIndex = -1;
		}

		public static GameData StartNewGame()
		{
			SetGameDataIndex();

			var newGameData = new GameData();

			SavedGameData[SaveGameIndex] = newGameData;

			return newGameData;
		}

		public static GameData GetGameData(int index)
		{
			return SavedGameData[index];
		}

		public static GameData ContinueExistingGame(int index)
		{
			if (index > SavedGameData.Length)
			{
				Debug.LogError("Index exceeds number of game saves");
				index = 0;
			}

			SaveGameIndex = index;

			return SavedGameData[SaveGameIndex];
		}

		private static void SetGameDataIndex()
		{
			for (int i = 0; i < SavedGameData.Length; i++)
			{
				if (SavedGameData[i] == null)
				{
					SaveGameIndex = i;
					break;
				}
			}
		}

		#region I/O Methods

		public static Stream GetGameDataStream(FileAccess access)
		{
			if (SaveGameIndex < 0)
			{
				Debug.LogError("We shouldn't have been able to save the game yet alone load it (ignore if this is development)");
				SaveGameIndex = 0;
			}

			var location = SAVE_FILE_1;
			if (SaveGameIndex == 1)
				location = SAVE_FILE_2;
			else if (SaveGameIndex == 2)
				location = SAVE_FILE_3;

			if (access == FileAccess.Write)
				return File.OpenWrite(location);
			else if (access == FileAccess.Read)
				return File.OpenRead(location);

			throw new ArgumentException("access mode was neither read nor write");
		}

		private static bool DoesDataFileExist()
		{
			try
			{
				File.OpenRead(SAVE_DATA_LOCATION);

				return true;
			}
			catch (FileNotFoundException)
			{
			}

			return false;
		}

		#endregion
	}
}
