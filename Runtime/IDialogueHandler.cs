using System.Collections.Generic;

namespace DialogueSystem
{
    public interface IDialogueHandler
    {
        bool CanContinue { get; }
        string CurrentCharacterName { get; }
        string CurrentText { get; }
        bool HasChoices { get; }
        int CurrentChoicesCount { get; }

        IReadOnlyCollection<string> GetCurrentChoices();
        string Continue();
        void Choose(int choiceIndex);
        void ResetState();
    }
}