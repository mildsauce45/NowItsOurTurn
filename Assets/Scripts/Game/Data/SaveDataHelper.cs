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

		private static GameData[] SavedGameData = new GameData[3];
		private static int SaveGameIndex = -1;		

		public static bool SaveDataExists()
		{
			if (!DoesDataFileExist())
				CreateDataFile();

			// Read GameData Headers (you know, name and level)

			return SavedGameData.Any(sgd => sgd != null);
		}

		public static GameData StartNewGame()
		{
			SetGameDataIndex();

			var newGameData = new GameData();

			SavedGameData[SaveGameIndex] = newGameData;

			return newGameData;
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

		private static void CreateDataFile()
		{
			try
			{
				var stream = File.Create(SAVE_DATA_LOCATION);				
			}
			catch (Exception)
			{
				Debug.LogError("Could not create save file at " + SAVE_DATA_LOCATION);
			}
		}

		#endregion
	}
}
