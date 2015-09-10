using FirstWave.Core.GUI.Menus;
using FirstWave.Niot.Game.Data;
using FirstWave.Niot.Managers;
using UnityEngine;

namespace FirstWave.Game.GameStart
{
	public class InitialMenu : MonoBehaviour
	{
		private Menu menu;

		void Start()
		{
			menu = GetComponent<Menu>();

			menu.AddMenuItem(new MenuItem("BEGIN A NEW QUEST", StartNewGame));

			if (SaveDataHelper.SaveDataExists())
			{
				menu.AddMenuItem(new MenuItem("CONTINUE A QUEST", LoadExistingGame));
				menu.AddMenuItem(new MenuItem("DELETE A QUEST", () => { }));
				
				menu.size = new Vector2(menu.size.x, 90);

				// Default to the continue option if we have save data
				menu.SetSelectedIndex(1);
			}
		}

		private void StartNewGame()
		{
			GameStateManager.Instance.GameData = SaveDataHelper.StartNewGame();

			Application.LoadLevel("Overworld");
		}

		private void LoadExistingGame()
		{
			SaveDataHelper.ReadGameData();

			Application.LoadLevel("ContinueGame");
		}
	}
}
