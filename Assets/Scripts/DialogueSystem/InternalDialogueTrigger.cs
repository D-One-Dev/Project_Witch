using UnityEngine;
using Zenject;

public class InternalDialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [Inject(Id = "InternalDialogueManager")]
    private readonly DialogueManager _dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _dialogueManager.SetDialogue(dialogue);
            _dialogueManager.Trigger();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
