using UnityEngine;
using Zenject;

public class NextSceneTrigger : MonoBehaviour
{
    [SerializeField] private int nextSceneID;
    [SerializeField] private GameObject loadingScreen;
    private SavesController _savesController;

    [Inject]
    public void Construct(SavesController savesController)
    {
        _savesController = savesController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _savesController.CurrentSceneID = nextSceneID;
            _savesController.Save();
            _savesController.ResetPlayerPos();
            AnimationsController.instance.ChangeScene(loadingScreen, nextSceneID);
        }
    }
}