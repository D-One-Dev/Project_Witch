using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject icon;
    private bool isActive;
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Gameplay.Interact.performed += ctx => Open();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    private void Open()
    {
        if (isActive)
        {
            icon.SetActive(false);
            animator.SetTrigger("Open");
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            icon.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
            icon.SetActive(false);
        }
    }
}