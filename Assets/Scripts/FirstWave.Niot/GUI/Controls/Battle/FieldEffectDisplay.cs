using System.Linq;
using FirstWave.Niot.Abilities;
using UnityEngine;

namespace FirstWave.Niot.GUI.Controls.Battle
{
	public class FieldEffectDisplay : Singleton<FieldEffectDisplay>
	{
		public Texture2D martialPill;
		public Texture2D firePill;
		public Texture2D waterPill;
		public Texture2D lightPill;
		public Texture2D darkPill;

		public float pillPadding = 5f;

		// Used to locate this display
		private PartyInformationDisplay partyDisplay;

		private float textureWidth;
		private float textureHeight;

		void Awake()
		{
			textureWidth = martialPill.width;
			textureHeight = martialPill.height;

			partyDisplay = this.gameObject.GetComponent<PartyInformationDisplay>();
		}

		void OnGUI()
		{
			var fieldEffects = TurnBasedBattleManager.Instance.FieldEffect;

			if (fieldEffects == null || partyDisplay == null)
				return;			

			// Now let's get all the non-None elements
			var nonEmptyFieldEffects = fieldEffects.Where(et => et != ElementType.None);
			int count = nonEmptyFieldEffects.Count();

			if (count == 0)
				return;

			var y = partyDisplay.frameSize.y + 15f; // 15f takes into account the top margin and some margin for this guy
			var x = (Screen.width / 2) - (count * textureWidth + ((count - 1) * pillPadding)) / 2;

			foreach (var fe in nonEmptyFieldEffects)
			{
				UnityEngine.GUI.DrawTexture(new Rect(x, y, textureWidth, textureHeight), GetTextureForEffect(fe));

				x += (textureWidth + pillPadding);
			}
		}

		public Texture2D GetTextureForEffect(ElementType et)
		{
			Texture2D res = null;

			switch (et)
			{
				case ElementType.Martial:
					res = martialPill;
					break;
				case ElementType.Fire:
					res = firePill;
					break;
				case ElementType.Water:
					res = waterPill;
					break;
				case ElementType.Light:
					res = lightPill;
					break;
				case ElementType.Dark:
					res = darkPill;
					break;
			}

			return res;
		}
	}
}