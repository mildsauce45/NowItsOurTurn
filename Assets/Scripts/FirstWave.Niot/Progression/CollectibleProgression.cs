using FirstWave.Niot.Managers;
using FirstWave.Niot.Progression;

namespace Assets.Scripts.FirstWave.Niot.Progression
{
	public class CollectibleProgression : DialogProgression
	{
		public string collectibleKey;

		protected override void OnPostDialog()
		{
			if (!GameStateManager.Instance.GameData.Collectibles.Contains(collectibleKey))
				GameStateManager.Instance.GameData.Collectibles.Add(collectibleKey);
		}
	}
}
