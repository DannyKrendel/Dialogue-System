using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public event Action Started = () => { };
        public event Action Ended = () => { };
        public event Action<DialogueData> Continued = _ => { };
        public event Action<int> ChoiceMade = _ => { };

        public bool IsInDialogue { get; private set; }

        private IDialogueHandler _dialogueHandler;

        #region Singleton stuff

        private static DialogueManager _instance;

        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<DialogueManager>();
                return _instance;
            }
        }

        #endregion

        public void StartDialogue(IDialogueHandler dialogueHandler)
        {
            if (IsInDialogue) return;

            _dialogueHandler = dialogueHandler;

            if (!_dialogueHandler.CanContinue)
            {
                Debug.LogError("Dialog can't be continued");
                return;
            }

            IsInDialogue = true;

            Started();

            ContinueInternal();
        }

        public void Continue()
        {
            if (!IsInDialogue) return;

            if (_dialogueHandler.CanContinue)
            {
                ContinueInternal();
            }
            else if (!_dialogueHandler.CanContinue && !_dialogueHandler.HasChoices)
            {
                End();
            }
        }

        public void ChooseAndContinue(int index)
        {
            _dialogueHandler.Choose(index);
            if (_dialogueHandler.CanContinue)
            {
                ContinueInternal();
            }

            ChoiceMade(index);
        }

        public void End()
        {
            if (!IsInDialogue) return;

            _dialogueHandler.ResetState();
            IsInDialogue = false;

            Ended();
        }

        private void ContinueInternal()
        {
            string currentText = _dialogueHandler.Continue();
            string[] choices = null;

            if (_dialogueHandler.HasChoices) choices = _dialogueHandler.GetCurrentChoices().ToArray();

            Continued(new DialogueData
            {
                Text = currentText,
                CharacterName = _dialogueHandler.CurrentCharacterName,
                Choices = choices
            });
        }
    }
}