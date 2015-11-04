using FirstWave.Core.GUI;
using UnityEngine;

namespace FirstWave.Core.GUI.Dialogs
{
	public class Dialog : MonoBehaviour
	{
		public string Text;

		#region GUI Elements

		public BorderTextures textures;
		public FontProperties fontProperties;

		#endregion

		public float InputDelay = 0.5f;
		public float CharacterDelay = 0.0125f;

		public float HorizontalOffset = 10f;
		public float VerticalOffset = 10f;
		public float TextPadding = 10f;
		public float Height = 200f;

		public float ChoiceWidth = 150f;
		public float ChoiceHeight = 35f;

		private float inputPassedTime;
		private float characterDisplayPassedTime;
		private Conversation conversation;

		private InputManager inputManager;

		public delegate void CloseAction(string branch);
		public event CloseAction OnClosed;

		void Start()
		{
			inputPassedTime = 0f;
			characterDisplayPassedTime = 0f;

			inputManager = FindObjectOfType<InputManager>();
		}

		// Update is called once per frame
		void Update()
		{
			if (inputPassedTime > InputDelay)
			{
				if (inputManager.KeyReleased("Interact"))
				{
					if (conversation == null || (!conversation.RequiresChoice && conversation.IsComplete))
						SafeRaiseClose();
					else if (conversation != null)
					{
						if (conversation.RequiresChoice)
						{
							conversation.SetChoice();
							if (conversation.IsComplete)
								SafeRaiseClose();
						}

						conversation.AdvanceConversation();

						ResetTimers();
					}
				}
				else if (inputManager.KeyReleased("Cancel"))
					SafeRaiseClose();
				else if (conversation != null)
				{
					if (inputManager.KeyReleased("left"))
						conversation.PrevChoice();
					else if (inputManager.KeyReleased("right"))
						conversation.NextChoice();
				}
			}

			inputPassedTime += Time.deltaTime;
			characterDisplayPassedTime += Time.deltaTime;
		}

		void OnGUI()
		{
			GUIStyle style = GUIManager.GetMessageBoxStyle(fontProperties);

			float dialogWidth = Screen.width - (HorizontalOffset * 2);

			string dialogText = conversation != null ? conversation.CurrentLine : Text;
			
			BorderBox.Draw(new Rect(HorizontalOffset, VerticalOffset, dialogWidth, Height), textures);

			int numCharacters = (int)(characterDisplayPassedTime / CharacterDelay);
			numCharacters = Mathf.Clamp(numCharacters, 0, dialogText.Length);

			UnityEngine.GUI.Label(new Rect(HorizontalOffset + TextPadding, VerticalOffset + TextPadding, dialogWidth - (TextPadding * 2),
					  Height - (TextPadding * 2)),
					  dialogText.Substring(0, numCharacters), style);

			if (conversation != null && conversation.RequiresChoice)
			{
				var choiceItem = conversation.CurrentItem as ChoiceItem;

				float startY = Height + VerticalOffset + 10;
				float startX = HorizontalOffset;

				int currentChoice = 0;
				foreach (var option in choiceItem.Options)
				{
					BorderBox.Draw(new Rect(startX, startY, ChoiceWidth, ChoiceHeight), textures);

					var optionContent = new GUIContent(option.Text);
					var textSize = style.CalcSize(optionContent);

					float optionStartY = startY + ((ChoiceHeight - textSize.y) / 2) + (fontProperties.additionalPadding > 0 ? fontProperties.additionalPadding : 0);

					UnityEngine.GUI.Label(new Rect(startX + ((ChoiceWidth - textSize.x) / 2), optionStartY, textSize.x, textSize.y), optionContent, style);

					if (currentChoice == conversation.CurrentChoice)
						UnityEngine.GUI.DrawTexture(new Rect((startX + (ChoiceWidth - textures.Pointer.width) / 2) - 20, startY + (ChoiceHeight - textures.Pointer.height) / 2, textures.Pointer.width, textures.Pointer.height), textures.Pointer);

					startX += ChoiceWidth + 10;
					currentChoice++;
				}
			}
		}

		private void ResetTimers(bool resetCharacters = true)
		{
			inputPassedTime = 0f;

			if (resetCharacters)
				characterDisplayPassedTime = 0f;
		}

		public void StartConversation(string text)
		{
			Text = text;
		}

		public void StartConversation(Conversation conversation)
		{
			conversation.Restart();

			this.conversation = conversation;
		}

		private void SafeRaiseClose()
		{
			ResetTimers();

			if (OnClosed != null)
				OnClosed(conversation != null ? conversation.CurrentBranch : null);			
		}
	}
}
