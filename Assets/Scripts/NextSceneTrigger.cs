using UnityEngine;
using Zenject;

public class NextSceneTrigger : MonoBehaviour
{
    [SerializeField] private int nextSceneID;
    [SerializeField] private GameObject loadingScreen;
    private SavesController _savesController;
    private AnimationsController _animationsController;

    [Inject]
    public void Construct(SavesController savesController, AnimationsController animationsController)
    {
        _savesController = savesController;
        _animationsController = animationsController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) TriggerNextScene();
    }

    public void TriggerNextScene()
    {
        _savesController.CurrentSceneID = nextSceneID;
        _savesController.Save();
        Debug.Log(loadingScreen);
        _savesController.ResetPlayerPos();
        _animationsController.ChangeScene(loadingScreen, nextSceneID);
    }
}