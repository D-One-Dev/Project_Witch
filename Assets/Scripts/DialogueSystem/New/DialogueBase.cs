using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueBase
{
    [SerializeField] protected bool isDialogueInteractive;
    [HideInInspector] public bool IsFinished { get; protected set; }
    protected IEnumerator WriteText(string input, TMP_Text dialogueText, TMP_FontAsset textFont, float delayBetweenCharacters,
        float delayBetweenLines)
    {
        if(input != null)
        {
            dialogueText.font = textFont;
            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if(c == '<')
                {
                    string tag = c.ToString();
                    for(int j = i + 1; j < input.Length; j++)
                    {
                        char d = input[j];
                        tag += d;
                        if(d == '>')
                        {
                            dialogueText.text += tag;
                            i = j;
                            break;
                        }
                    }
                }
                else
                {
                    dialogueText.text += c;
                    yield return new WaitForSeconds(delayBetweenCharacters);
                }
            }

            if (!isDialogueInteractive)
            {
                yield return new WaitForSeconds(delayBetweenLines);
                IsFinished = true;
            }
            else
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(delayBetweenLines);
            IsFinished = true;
        }
    }
}