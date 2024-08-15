using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string task;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TaskUI.Instance.ChangeTask(task);
            Destroy(gameObject);
        }
    }
}