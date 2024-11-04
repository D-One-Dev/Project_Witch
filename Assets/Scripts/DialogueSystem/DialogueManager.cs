using System.Collections;
using UnityEngine;
using Zenject;

public abstract class DialogueManager
{
    [Inject(Id = "DialogueScreen")]
    protected readonly GameObject _dialogueScreen;

    protected DialogueHolder _currentDialogue = null;
    protected AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    public void Trigger()
    {
        if(_currentDialogue != null)
        {
            if (!_currentDialogue.IsDialogueActive)
            {
                _currentDialogue.StartDialogue();
            }
            else
            {
                _currentDialogue.SkipLine();
            }
        }
    }

    protected void EndDialogue()
    {
        _animationsController.FadeOutScreen(_dialogueScreen);
    }

    public void LeaveDialogue()
    {
        _currentDialogue = null;
        EndDialogue();
    }

    public void SetDialogue(DialogueHolder dialogue)
    {
        _currentDialogue = dialogue;
    }

    protected abstract IEnumerator OnPhraseEnd(float delayTime);
}