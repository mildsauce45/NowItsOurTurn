using FirstWave.Core.GUI.Menus;
using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Data;
using FirstWave.Niot.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirstWave.Niot.GameStart
{
	public class SavedGameSelection : MonoBehaviour
	{
		private Menu menu;

		void Start()
		{
			menu = GetComponent<Menu>();

			if (!SaveDataHelper.DataLoaded)
				SaveDataHelper.ReadGameData();

			for (int i = 0; i < Constants.Ranges.NUM_OF_GAME_SAVES; i++)
			{
				var data = SaveDataHelper.GetGameData(i);
				if (data != null)
				{
					int closureIndex = i;
					menu.AddMenuItem(new MenuItem(string.Format("{0} - Lvl. {1}", data.Party[0].Name, data.Party[0].Level), () => ContinueGame(closureIndex)));
				}
				else
				{
					menu.AddMenuItem(new MenuItem("-", () => { }));
				}
			}
		}

		private void ContinueGame(int index)
		{
			var data = SaveDataHelper.GetGameData(index);

			if (data == null)
				return;

			// Mark this game as the one we're continuing
			SaveDataHelper.ContinueExistingGame(index);

			// Set the data into the persistent data manager
			GameStateManager.Instance.GameData = data;

			// Restore the location in the transition manager
			TransitionManager.Instance.playerPosition = data.Location;

			// Load the scene we're supposed to
			SceneManager.LoadScene(data.Scene);
		}
	}
}
