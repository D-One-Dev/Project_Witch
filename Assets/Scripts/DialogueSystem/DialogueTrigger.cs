using UnityEngine;
using Zenject;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private Dialogue dialogue;
    private DialogueManager _dialogueManager;

    [Inject]
    public void Construct(DialogueManager dialogueManager)
    {
        _dialogueManager = dialogueManager;
    }

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