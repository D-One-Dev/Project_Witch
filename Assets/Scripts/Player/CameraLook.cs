using UnityEngine;
using Zenject;

public class CameraLook : ITickable, IInitializable
{
    [Inject(Id = "DefaultMouseSens")]
    private readonly float _defaultSens;
    [Inject(Id = "PlayerTransform")]
    private readonly Transform _playerBody;
    [Inject(Id = "Camera")]
    private readonly Transform _cam;
    private CharacterController _characterController;
    private float _xRotation;
    private float _currentSens;
    private Controls _controls;

    [Inject]
    public void Construct(CharacterController characterController, Controls controls)
    {
        _characterController = characterController;
        _controls = controls;
        PlayerHealth.OnPlayerDeath += DisableControls;
        _controls.Enable();
    }
    public void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (PlayerPrefs.GetFloat("PlayerPosY", -10000f) != -10000f)
        {
            float playerRotX = PlayerPrefs.GetFloat("PlayerRotX", 0);
            _cam.localRotation = Quaternion.Euler(playerRotX, 0f, 0f);
            _xRotation = _cam.localRotation.eulerAngles.x;
        }
    }
    public void Tick()
    {
        if (_characterController.enabled)
        {
            Vector2 mouseDelta = _controls.Gameplay.MouseDelta.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * _currentSens;
            float mouseY = mouseDelta.y * _currentSens;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cam.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerBody.Rotate(Vector3.up * mouseX);
        }

        if (GlobalGamePause.instance.isGamePaused) _currentSens = Mathf.Lerp(_currentSens, 0f, 10f * Time.deltaTime);
        else _currentSens = Mathf.Lerp(_currentSens, _defaultSens, 10f * Time.deltaTime);
    }
    public void ChangeSens(int value)
    {
        float sens = (float)value / 50 * .15f;
        _currentSens = sens;
    }

    private void DisableControls()
    {
        _controls.Disable();
    }
}