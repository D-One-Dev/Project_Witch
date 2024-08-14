using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    public Dialogue dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icon.SetActive(true);
            DialogueManager.Instance.currentDialogue = dialogue;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icon.SetActive(false);
            DialogueManager.Instance.LeaveDialogue();
        }
    }
}
