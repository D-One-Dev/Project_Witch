using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject icon;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private SoundBase soundBase;
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
            soundBase.PlaySoundWithRandomPitch(clips[Random.Range(0, clips.Length)]);
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