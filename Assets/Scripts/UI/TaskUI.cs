using Lean.Localization;
using UnityEngine;
using Zenject;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskUI;
    public string currentTask;
    public static TaskUI Instance;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeTask(string taskId)
    {
        //currentTask = task;
        //_animationsController.ChangeTask(taskUI, task);
        string task = LeanLocalization.GetTranslationText(taskId);
        currentTask = task;
        _animationsController.ChangeTask(taskUI, task);
    }
}
