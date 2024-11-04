using System.Collections;
using UnityEngine;

public class InternalDialogueManager : DialogueManager
{
    protected override IEnumerator OnPhraseEnd(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);
        Trigger();
    }
}