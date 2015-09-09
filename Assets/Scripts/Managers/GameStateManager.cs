using FirstWave.Core.Utilities;
using FirstWave.Niot.Game.Data;

namespace FirstWave.Niot.Managers
{
	public class GameStateManager : SafeSingleton<GameStateManager>
	{
		protected override string managerName
		{
			get { return "Game State Manager"; }
		}

		public GameData GameData { get; set; }
	}
}
