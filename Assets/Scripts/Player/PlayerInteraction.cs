using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Controls _controls;
    [SerializeField] private GameObject hintScreen;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private InGameHint currentHint = null;
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
            AnimationsController.instance.FadeOutScreen(hintScreen);
        }
    }
}
