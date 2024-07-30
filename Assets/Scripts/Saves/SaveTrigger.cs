using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SavesController.instance.Save();
            Destroy(gameObject);
        }
    }
}