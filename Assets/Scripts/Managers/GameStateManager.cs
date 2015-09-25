using FirstWave.Core.Utilities;
using FirstWave.Niot.Game.Data;

namespace FirstWave.Niot.Managers
{
	public class GameStateManager : SafeSingleton<GameStateManager>
	{
		private const bool DEBUG = true;

		protected override string managerName
		{
			get { return "Game State Manager"; }
		}

		private GameData gameData;

		public GameData GameData
		{
			get
			{
				if (gameData == null && DEBUG)
				{					
					SaveDataHelper.ReadGameData();
					gameData = SaveDataHelper.ContinueExistingGame(0);
				}

				return gameData;
			}
			set { gameData = value; }
		}
	}
}
