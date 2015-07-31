using FirstWave.Core.GUI;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public string Text;
    
    public Font Font;

    #region GUI Elements

	public BorderTextures textures;

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
    //private ConversationManager conversationManager;

    private InputManager inputManager;

    public delegate void CloseAction();
    public static event CloseAction OnClosed;

    void Start()
    {
        inputPassedTime = 0f;
        characterDisplayPassedTime = 0f;

        //conversationManager = FindObjectOfType<ConversationManager>();
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
                {
                    ResetTimers(true);

                    if (OnClosed != null)
                        OnClosed();

                    if (conversation != null)
                        conversation.Restart();
                }
                else if (conversation != null)
                {
                    ResetTimers(true);

                    if (conversation.RequiresChoice)
                        conversation.SetChoice();

                    conversation.AdvanceConversation();
                }
            }
            else if ((conversation != null && conversation.RequiresChoice) && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                ResetTimers(false);

                float input = Input.GetAxis("Horizontal");
                if (input < 0)
                    conversation.PrevChoice();
                else
                    conversation.NextChoice();
            }
        }

            inputPassedTime += Time.deltaTime;
            characterDisplayPassedTime += Time.deltaTime;
    }

    void OnGUI()
    {
        GUIStyle style = GUIManager.GetMessageBoxStyle(null);

        float dialogWidth = Screen.width - (HorizontalOffset * 2);

        string dialogText = conversation != null ? conversation.CurrentLine : Text;

        BorderBox.Draw(new Rect(HorizontalOffset, VerticalOffset, dialogWidth, Height), textures);

        int numCharacters = (int)(characterDisplayPassedTime / CharacterDelay);
        numCharacters = Mathf.Clamp(numCharacters, 0, dialogText.Length);

        GUI.Label(new Rect(HorizontalOffset + TextPadding, VerticalOffset + TextPadding, dialogWidth - (TextPadding * 2),
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

                var textSize = style.CalcSize(new GUIContent(option.Text));
                GUI.Label(new Rect(startX + ((ChoiceWidth - textSize.x) / 2), startY + ((ChoiceHeight - textSize.y) / 2), textSize.x, textSize.y), option.Text, style);

                if (currentChoice == conversation.CurrentChoice)
                    GUI.DrawTexture(new Rect((startX + (ChoiceWidth - textures.Pointer.width) / 2) - 20, startY + (ChoiceHeight - textures.Pointer.height) / 2, textures.Pointer.width, textures.Pointer.height), textures.Pointer);

                startX += ChoiceWidth + 10;
                currentChoice++;
            }
        }
    }

    private void ResetTimers(bool resetCharacters)
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
        this.conversation = conversation;
    }
}
