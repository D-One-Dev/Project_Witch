using TMPro;
using UnityEngine;
using Zenject;
using Lean.Localization;

public class PlayerInteraction : MonoBehaviour
{
    [Inject(Id = "InGameHintScreen")]
    private readonly GameObject _hintScreen;
    [Inject(Id = "InGameHintText")]
    private readonly TMP_Text _hintText;
    private Controls _controls;

    private InGameHint _currentHint;
    private bool _isHintActive;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(Controls controls, AnimationsController animationsController)
    {
        _controls = controls;
        _animationsController = animationsController;

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
            _hintText.text = LeanLocalization.GetTranslationText(_currentHint.hintTag);
            //_hintText.text = _currentHint.hintText;
            _animationsController.FadeInScreen(_hintScreen);
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
            _animationsController.FadeOutScreen(_hintScreen);
        }
    }
}