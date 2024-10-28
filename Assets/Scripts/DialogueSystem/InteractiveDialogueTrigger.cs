using UnityEngine;
using Zenject;

public class InteractiveDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private Dialogue dialogue;
    [Inject(Id = "InteractiveDialogueManager")]
    private readonly DialogueManager _dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
            _dialogueManager.SetDialogue(dialogue);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            _dialogueManager.LeaveDialogue();
        }
    }
}