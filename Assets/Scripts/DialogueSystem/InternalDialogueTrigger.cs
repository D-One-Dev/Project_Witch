using UnityEngine;

public class InternalDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueHolder holder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            holder.StartDialogue();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}