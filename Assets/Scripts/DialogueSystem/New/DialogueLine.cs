using TMPro;
using UnityEngine;
using Lean.Localization;

[System.Serializable]
public class DialogueLine : DialogueBase
{
    //[TextArea(minLines:1, maxLines:25)]
    [SerializeField] public string lineTag;

    [SerializeField] private TMP_FontAsset textFont;
    [SerializeField] private float delayBetweenCharacters;
    [SerializeField] private float delayBetweenLines;

    [Header("Text Animation Parameters")]
    [SerializeField] private TextAnimator.AnimationType animationType;
    [SerializeField] private float timeScale;
    [SerializeField] private float magnitude;

    private string _line;
    private Coroutine _lineAppearCoroutine;

    public void StartLine(MonoBehaviour monoBeh, TMP_Text dialogueText)
    {
        dialogueText.text = "";
        _line = LeanLocalization.GetTranslationText(lineTag);
        _lineAppearCoroutine = monoBeh.StartCoroutine(WriteText(_line, dialogueText, textFont, delayBetweenCharacters,
            delayBetweenLines));
    }

    public void SkipLine(MonoBehaviour monoBeh, TMP_Text dialogueText)
    {
        if (isDialogueInteractive)
        {
            if (dialogueText.text != _line)
            {
                monoBeh.StopCoroutine(_lineAppearCoroutine);
                dialogueText.text = _line;
            }
            else IsFinished = true;
        }
    }

    public void ResetLine()
    {
        IsFinished = false;
    }

    public (TextAnimator.AnimationType, float, float) GetAnimationParameters()
    {
        return (animationType, timeScale, magnitude);
    }
}