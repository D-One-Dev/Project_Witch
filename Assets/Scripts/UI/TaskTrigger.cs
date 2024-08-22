using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string task;
    [SerializeField] private GameObject nextTrigger;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TaskUI.Instance.ChangeTask(task);
            if(nextTrigger != null) nextTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}