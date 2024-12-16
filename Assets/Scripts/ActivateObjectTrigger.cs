using UnityEngine;

public class ActivateObjectTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Obj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Obj.SetActive(true);
        }
    }
}