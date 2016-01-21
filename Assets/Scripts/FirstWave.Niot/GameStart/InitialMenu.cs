using FirstWave.Niot.Game;
using FirstWave.Niot.Game.Data;
using FirstWave.Niot.Managers;
using FirstWave.Unity.Core.Input;
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
        // This part of the UI is so simple, I'm not going to bother with a full blown state machine
        // Much more than this though, and a FSM starts to look pretty good
        private enum StartMenuState
        {
            InitialMenu,
            ContinueMenu
        }

		private Frame frame;
        private StartMenuState currentState;

        void Awake()
        {
            frame = FindObjectOfType<Frame>();
            if (!frame)
                throw new Exception("Couldn't find UPF Frame");

            currentState = StartMenuState.InitialMenu;
        }

		void Start()
		{
            SetupInitialMenu();
		}

        void Update()
        {
            if (InputManager.Instance.KeyReleased("Cancel") && currentState == StartMenuState.ContinueMenu)
            {
                currentState = StartMenuState.InitialMenu;
                SetupInitialMenu();
            }
        }

		private void StartNewGame()
		{
			GameStateManager.Instance.GameData = SaveDataHelper.StartNewGame();

            //SceneManager.LoadScene("ThroneRoomFloor");

			// For the demo
			SceneManager.LoadScene("DemoOverworld");
		}

		private void LoadExistingGame()
		{
            currentState = StartMenuState.ContinueMenu;

            SetupContinueMenu();
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
        private void SetupInitialMenu()
        {
            frame.Clear();

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

        private void SetupContinueMenu()
        {
            frame.Clear();

            var menu = new Menu();
            menu.SelectKey = "Interact";

            if (!SaveDataHelper.DataLoaded)
                SaveDataHelper.ReadGameData();

            for (int i = 0; i < GameConstants.Ranges.NUM_OF_GAME_SAVES; i++)
            {
                var data = SaveDataHelper.GetGameData(i);
                if (data != null)
                {
                    int closureIndex = i;
                    menu.AddItem(string.Format("{0} - Lvl. {1}", data.Party[0].Name, data.Party[0].Level), () => ContinueGame(closureIndex));
                }
                else
                {
                    menu.AddItem("-", () => { });
                }
            }

            var sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Center;
            sp.VerticalAlignment = VerticalAlignment.Center;

            sp.AddChild(menu);

            frame.AddPanel(sp);
        }
	}
}
