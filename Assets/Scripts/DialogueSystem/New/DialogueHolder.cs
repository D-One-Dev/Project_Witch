using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class DialogueHolder : MonoBehaviour
{
    public bool IsDialogueActive { get; private set; }
    [Inject(Id = "DialogueText")]
    private readonly TMP_Text dialogueText;
    [Inject(Id = "DialogueScreen")]
    protected readonly GameObject _dialogueScreen;
    [SerializeField] private DialogueLine[] dialogues;

    private DialogueLine _currentLine;
    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }
    private IEnumerator DialogueSequence()
    {
        foreach (DialogueLine line in dialogues)
        {
            _currentLine = line;
            _currentLine.StartLine(this, dialogueText);
            yield return new WaitUntil(() => _currentLine.IsFinished);
        }

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