using FirstWave.Core.GUI;
using FirstWave.Niot.Game;
using UnityEngine;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class PartyInformationDisplay : MonoBehaviour
	{
		public BorderTextures textures;
		public FontProperties fontProperties;

		public Vector2 frameSize;

		private Player[] party;

		void Awake()
		{
		}

		void OnGUI()
		{
			if (party == null)
				party = TurnBasedBattleManager.Instance.Party;

			if (party == null)
				return;

			// Our start x is going to start at the screens middle line and then half as much as we have party member info
			float x = (Screen.width / 2) - ((party.Length * frameSize.x + party.Length * 10f + party.Length * textures.BorderVertical.width * 2) / 2);
			float y = 10;

			var style = GUIManager.GetMessageBoxStyle(fontProperties);

			foreach (var member in party)
			{
				BorderBox.Draw(new Rect(x, y, frameSize.x, frameSize.y), textures);

				var nameStartY = y + textures.BorderHorizontal.height + 10f;

				var nameContent = new GUIContent(member.Name);
				var nameSize = style.CalcSize(nameContent);

				var nameStartX = (x - textures.BorderVertical.width) + ((frameSize.x / 2) - (nameSize.x / 2));

				UnityEngine.GUI.Label(new Rect(nameStartX, nameStartY, nameSize.x, nameSize.y), nameContent, style);

				x += (textures.BorderVertical.width * 2) + frameSize.x + 10f;
				
				var hpContent = new GUIContent(string.Format("H: {0}/{1}", member.CurrentHP, member.MaxHP));
				var hpSize = style.CalcSize(hpContent);

				var hpStartY = nameStartY + nameSize.y + 5f;

				UnityEngine.GUI.Label(new Rect(nameStartX, hpStartY, hpSize.x, hpSize.y), hpContent, style);
			}
		}
	}
}
