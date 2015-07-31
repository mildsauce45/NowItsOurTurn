using UnityEngine;

namespace FirstWave.Core
{
	public class TextDisplayTimer : Timer, ITextSource
	{		
		public string FullText { get; set; }

		public string Text
		{
			get { return FullText.Substring(0, GetNumCharactersToDisplay()); }
		}

		public bool IsFullTextDisplayed
		{
			get { return GetNumCharactersToDisplay() == FullText.Length; }
		}

		public override bool IsComplete
		{
			get { return IsFullTextDisplayed; }
		}

		private float characterDisplayPassedTime;

		public TextDisplayTimer(float characterDelay)
			: this(characterDelay, null)
		{
		}

		public TextDisplayTimer(float characterDelay, string text)
			: base(characterDelay)
		{
			FullText = text;			

			characterDisplayPassedTime = 0;
		}		

		public override void Update()
		{
			characterDisplayPassedTime += Time.deltaTime;			

			if (!Enabled)
				return;

			base.Update();
		}

		public override void Reset()
		{
			base.Reset();

			characterDisplayPassedTime = 0f;
		}

		private int GetNumCharactersToDisplay()
		{
			int numCharacters = (int)(characterDisplayPassedTime / TimeSpan);

			return Mathf.Clamp(numCharacters, 0, FullText.Length);
		}

		private void WriteText()
		{
			Debug.Log(Text.Substring(0, GetNumCharactersToDisplay()));			
		}
	}
}
