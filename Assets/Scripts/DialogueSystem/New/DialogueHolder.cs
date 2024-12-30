using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DialogueHolder : MonoBehaviour
{
    public bool IsDialogueActive { get; private set; }
    [Inject(Id = "DialogueText")]
    private readonly TMP_Text dialogueText;
    [Inject(Id = "DialogueScreen")]
    protected readonly GameObject _dialogueScreen;
    [SerializeField] private DialogueLine[] dialogues;
    [SerializeField] private OnDialogueEndEvent onDialogueEndAction = new OnDialogueEndEvent();

    private DialogueLine _currentLine;
    private AnimationsController _animationsController;
    private TextAnimator _textAnimator;
    [Serializable]
    private class OnDialogueEndEvent : UnityEvent { }

    [Inject]
    public void Construct(AnimationsController animationsController, TextAnimator textAnimator)
    {
        _animationsController = animationsController;
        _textAnimator = textAnimator;
    }
    private IEnumerator DialogueSequence()
    {
        foreach (DialogueLine line in dialogues)
        {
            _currentLine = line;
            _currentLine.StartLine(this, dialogueText);
            _textAnimator.SetAnimation(_currentLine.GetAnimationParameters());
            yield return new WaitUntil(() => _currentLine.IsFinished);
            _currentLine.onLineEnd?.Invoke();
        }

        Debug.Log("Dialogue End");
        onDialogueEndAction.Invoke();
        IsDialogueActive = false;
        ResetDialogue();
        _animationsController.FadeOutScreen(_dialogueScreen);
    }

    public void StartDialogue()
    {
        IsDialogueActive = true;
        _animationsController.FadeInScreen(_dialogueScreen);
        StartCoroutine(DialogueSequence());
    }

    public void SkipLine()
    {
        _currentLine.SkipLine(this, dialogueText);
    }

    public void ResetDialogue()
    {
        IsDialogueActive = false;
        foreach (DialogueLine line in dialogues)
        {
            line.ResetLine();
        }
    }
}