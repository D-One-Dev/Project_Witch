using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueLine : DialogueBase
{
    [TextArea(minLines:1, maxLines:25)]
    [SerializeField] public string line;

    [SerializeField] private TMP_FontAsset textFont;
    [SerializeField] private float delayBetweenCharacters;
    [SerializeField] private float delayBetweenLines;

    [Header("Text Animation Parameters")]
    [SerializeField] private TextAnimator.AnimationType animationType;
    [SerializeField] private float timeScale;
    [SerializeField] private float magnitude;

    private Coroutine _lineAppearCoroutine;

    public void StartLine(MonoBehaviour monoBeh, TMP_Text dialogueText)
    {
        dialogueText.text = "";
        _lineAppearCoroutine = monoBeh.StartCoroutine(WriteText(line, dialogueText, textFont, delayBetweenCharacters,
            delayBetweenLines));
    }

    public void SkipLine(MonoBehaviour monoBeh, TMP_Text dialogueText)
    {
        if (isDialogueInteractive)
        {
            if (dialogueText.text != line)
            {
                monoBeh.StopCoroutine(_lineAppearCoroutine);
                dialogueText.text = line;
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