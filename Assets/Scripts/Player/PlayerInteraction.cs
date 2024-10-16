using TMPro;
using UnityEngine;
using Zenject;

public class PlayerInteraction : MonoBehaviour
{
    [Inject(Id = "InGameHintScreen")]
    private readonly GameObject _hintScreen;
    [Inject(Id = "InGameHintText")]
    private readonly TMP_Text _hintText;
    private Controls _controls;

    private InGameHint _currentHint;
    private bool _isHintActive;

    [Inject]
    public void Construct(Controls controls)
    {
        _controls = controls;

        _controls.Gameplay.Interact.performed += ctx => Interact();
        PlayerHealth.OnPlayerDeath += OnDisable;
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
        if(_currentHint != null && !_isHintActive)
        {
            _hintText.text = _currentHint.hintText;
            AnimationsController.instance.FadeInScreen(_hintScreen);
            _isHintActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hint"))
        {
            _currentHint = other.GetComponent<InGameHint>();
            _currentHint.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hint") && _currentHint != null)
        {
            _currentHint.Deactivate();
            _currentHint = null;
            _isHintActive = false;
            AnimationsController.instance.FadeOutScreen(_hintScreen);
        }
    }
}