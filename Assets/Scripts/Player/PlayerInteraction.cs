using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Controls _controls;
    public InGameHint currentHint;
    [SerializeField] private GameObject hintScreen;
    [SerializeField] private TMP_Text hintText;
    private bool isHintActive;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Interact.performed += ctx => Interact();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Interact()
    {
        if(currentHint != null && !isHintActive)
        {
            hintText.text = currentHint.hintText;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AnimationsController.instance.FadeInScreen(hintScreen);
            isHintActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hint"))
        {
            currentHint = other.GetComponent<InGameHint>();
            currentHint.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hint") && currentHint != null)
        {
            currentHint.Deactivate();
            currentHint = null;
            isHintActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AnimationsController.instance.FadeOutScreen(hintScreen);
        }
    }
}
