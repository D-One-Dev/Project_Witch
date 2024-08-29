using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    [SerializeField] private int nextSceneID;
    [SerializeField] private GameObject loadingScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SavesController.instance.currentSceneID = nextSceneID;
            SavesController.instance.Save();
            SavesController.instance.ResetPlayerPos();
            AnimationsController.instance.ChangeScene(loadingScreen, nextSceneID);
        }
    }
}