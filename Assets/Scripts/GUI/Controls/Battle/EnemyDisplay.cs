using UnityEngine;
using System.Collections;
using System.Linq;
using FirstWave.Niot.Game;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class EnemyDisplay : MonoBehaviour
	{
		public float enemyPadding = 10f;

		public Texture2D debugLineTexture;
		public bool debug = true;

		private Enemy[] enemyParty;

		void OnGUI()
		{
			if (enemyParty == null)
				enemyParty = TurnBasedBattleManager.Instance.EnemyParty;

			if (enemyParty == null)
				return;

			// My plan is to align each monster vertically along it's bottom edge (like they're standing on the ground)
			// For now, I'll start that at the middle of the screen and adjust down by half of the tallest enemy.
			// I can adjust this overall plan if needs be
			var bottomY = (Screen.height / 2) + (enemyParty.Max(e => e.Sprite.height) / 2);

			if (debug)
			{
				//Debug.DrawLine(new Vector3(0, Screen.height / 2), new Vector3(Screen.width, Screen.height / 2));
				UnityEngine.GUI.DrawTexture(new Rect(0, Screen.height / 2, Screen.width, 1), debugLineTexture);
				UnityEngine.GUI.DrawTexture(new Rect(Screen.width / 2, 0, 1, Screen.height), debugLineTexture);
			}

			// Similar to the Y coordinate, I'm going to start in the middle and build evenly out. Of course this time I need
			// to start in the middle minus half of the overall width of the enemy textures plus some padding between each guy
			var startX = (Screen.width / 2) - ((enemyParty.Sum(e => e.Sprite.width) + (enemyPadding * (enemyParty.Length - 1))) / 2);

			foreach (var enemy in enemyParty)
			{
				var enemyY = bottomY - enemy.Sprite.height;

				// Only draw the enemy if he's not dead, but reserve the space for him, even if he is
				if (!enemy.IsDead)
					UnityEngine.GUI.DrawTexture(new Rect(startX, enemyY, enemy.Sprite.width, enemy.Sprite.height), enemy.Sprite);

				startX += enemy.Sprite.width + enemyPadding;
			}
		}
	}
}