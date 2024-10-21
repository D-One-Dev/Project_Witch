using UnityEngine;
using Zenject;

public class SaveTrigger : MonoBehaviour
{
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
            _savesController.Save();
            Destroy(gameObject);
        }
    }
}