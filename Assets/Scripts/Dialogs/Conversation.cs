using System.Linq;

namespace FirstWave.Core.GUI.Dialogs
{
	public class Conversation
	{
		public ConversationItem[] spokenLines;
		public int CurrentChoice { get; private set; }
		public string CurrentBranch { get; private set; }

		private int currentLineNumber = 0;

		public bool IsComplete
		{
			get { return spokenLines == null || GetNextItem() == null; }
		}

		public string CurrentLine
		{
			get { return CurrentItem != null ? CurrentItem.Text : string.Empty; }
		}

		public ConversationItem CurrentItem
		{
			get { return spokenLines != null ? spokenLines.FirstOrDefault(ci => ci.Ordinal == currentLineNumber) : null; }
		}

		public bool RequiresChoice
		{
			get { return CurrentItem != null && CurrentItem is ChoiceItem; }
		}

		public Conversation(ConversationItem[] lines)
		{
			spokenLines = lines;

			currentLineNumber = lines.OrderBy(ci => ci.Ordinal).FirstOrDefault().Ordinal;
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
				CurrentBranch = (CurrentItem as ChoiceItem).Options[CurrentChoice].ConfirmBranch;
				CurrentChoice = 0;
			}
		}

		public void Restart()
		{
			currentLineNumber = spokenLines.OrderBy(ci => ci.Ordinal).FirstOrDefault().Ordinal;
			CurrentBranch = null;
			CurrentChoice = 0;
		}

		private ConversationItem GetNextItem()
		{
			return spokenLines.OrderBy(ci => ci.Ordinal).FirstOrDefault(ci => ci.Branch == CurrentBranch && ci.Ordinal > CurrentItem.Ordinal);
		}
	}
}