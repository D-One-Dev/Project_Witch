using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskUI;
    public string currentTask;
    public static TaskUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeTask(string task)
    {
        currentTask = task;
        AnimationsController.instance.ChangeTask(taskUI, task);
    }
}
