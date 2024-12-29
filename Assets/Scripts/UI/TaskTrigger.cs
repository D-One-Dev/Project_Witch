using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private string taskId;
    [SerializeField] private GameObject nextTrigger;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TaskUI.Instance.ChangeTask(taskId);
            if(nextTrigger != null) nextTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}