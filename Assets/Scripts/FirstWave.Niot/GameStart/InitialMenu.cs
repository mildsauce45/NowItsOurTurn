using FirstWave.Niot.Game.Data;
using FirstWave.Niot.Managers;
using FirstWave.Unity.Gui;
using FirstWave.Unity.Gui.Controls;
using FirstWave.Unity.Gui.Enums;
using FirstWave.Unity.Gui.Panels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirstWave.Game.GameStart
{
    public class InitialMenu : MonoBehaviour
	{
		private Menu menu;

		void Start()
		{
            var frame = FindObjectOfType<Frame>();
            if (!frame)
                throw new Exception("Couldn't find UPF Frame");

            var menu = new Menu();
            menu.SelectKey = "Interact";

            menu.AddItem("BEGIN A NEW QUEST", StartNewGame);

            if (SaveDataHelper.SaveDataExists())
            {
                menu.AddItem("CONTINUE A QUEST", LoadExistingGame);
                menu.AddItem("DELETE A QUEST", () => { });
            }

            var sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            sp.AddChild(menu);

            frame.AddPanel(sp);
		}

		private void StartNewGame()
		{
			GameStateManager.Instance.GameData = SaveDataHelper.StartNewGame();

			//Application.LoadLevel("ThroneRoomFloor");

			// For the demo
			SceneManager.LoadScene("DemoOverworld");
		}

		private void LoadExistingGame()
		{
			SaveDataHelper.ReadGameData();

			SceneManager.LoadScene("ContinueGame");
		}
	}
}
