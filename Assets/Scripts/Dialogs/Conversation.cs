using System.Linq;

public class Conversation
{
    public ConversationItem[] spokenLines;
    public int CurrentChoice { get; private set; }

    private int currentLineNumber = 0;
    private string currentBranch = null;

    public bool IsComplete
    {
        get { return spokenLines == null || currentLineNumber >= spokenLines.Length - 1 || GetNextItem() == null; }
    }

    public string CurrentLine
    {
        get { return spokenLines != null && currentLineNumber < spokenLines.Length ? spokenLines[currentLineNumber].Text : string.Empty; }
    }

    public ConversationItem CurrentItem
    {
        get { return spokenLines != null && currentLineNumber < spokenLines.Length ? spokenLines[currentLineNumber] : null; }
    }

    public bool RequiresChoice
    {
        get { return spokenLines != null && currentLineNumber < spokenLines.Length && spokenLines[currentLineNumber] is ChoiceItem; }
    }

    public Conversation(ConversationItem[] lines)
    {
        spokenLines = lines;
    }

    public void AdvanceConversation()
    {

        var nextLine = GetNextItem();
        if (nextLine != null)
        {
            currentLineNumber = nextLine.Ordinal;
            CurrentChoice = 0;
        }
    }

    public void NextChoice()
    {
        if (CurrentItem is ChoiceItem)
        {
            var choice = CurrentItem as ChoiceItem;

            CurrentChoice = (CurrentChoice + 1) % choice.Options.Length;
        }
    }

    public void PrevChoice()
    {
        if (CurrentItem is ChoiceItem)
        {
            var choice = CurrentItem as ChoiceItem;

            CurrentChoice = CurrentChoice == 0 ? choice.Options.Length - 1 : CurrentChoice - 1;
        }
    }

    public void SetChoice()
    {
        if (RequiresChoice)
        {
            currentBranch = (CurrentItem as ChoiceItem).Options[CurrentChoice].ConfirmBranch;
            CurrentChoice = 0;
        }
    }

    public void Restart()
    {
        currentLineNumber = 0;
        currentBranch = null;
        CurrentChoice = 0;
    }

    private ConversationItem GetNextItem()
    {
        return spokenLines.OrderBy(ci => ci.Ordinal).FirstOrDefault(ci => ci.Branch == currentBranch && ci.Ordinal > currentLineNumber);
    }
}