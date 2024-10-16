using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerMovement : IInitializable, ITickable
{
    private CharacterController _characterController;
    [Inject(Id = "PlayerTransform")]
    private readonly Transform _playerTransform;
    [Inject(Id = "MovementSpeed")]
    private readonly float _movementSpeed;
    [Inject(Id = "JumpSpeed")]
    private readonly float _jumpSpeed;
    [Inject(Id = "Gravity")]
    private readonly float _gravity;
    [Inject(Id = "Camera")]
    private readonly Transform _cam;
    [Inject(Id = "PlayerAudioSource")]
    private readonly AudioSource _playerAudioSource;
    [Inject(Id = "JumpSound")]
    private readonly AudioClip _jumpSound;
    [Inject(Id = "DashDistance")]
    private readonly float _dashDistance;
    [Inject(Id = "DashCooldownTime")]
    private readonly float _dashCooldownTime;
    [Inject(Id = "DashIcon")]
    private readonly Image _dashIcon;
    [Inject(Id = "JumpHelpTime")]
    private readonly float _jumpHelpTime;
    [Inject(Id = "MaxJumpAngle")]
    private readonly float _maxJumpAngle;
    [Inject(Id = "EffectDisabledSprite")]
    private readonly Sprite _effectDisabledSprite;
    [Inject(Id = "EffectEnabledSprite")]
    private readonly Sprite _effectEnabledSprite;
    [Inject(Id = "PlayerInstaller")]
    private readonly MonoInstaller _playerInstaller;

    private Controls _controls;
    private float _lastGroundedTime;
    private bool _isLandingSoundPlayed;
    private bool _shift;
    private bool _jump;
    private float _grav;
    private bool _canDash = true;

    [Inject]
    public void Construct(Controls controls, CharacterController characterController)
    {
        _controls = controls;
        _characterController = characterController;

        _controls.Gameplay.Shift.started += ctx => _shift = true;
        _controls.Gameplay.Shift.canceled += ctx => _shift = false;
        _controls.Gameplay.Space.started += ctx => _jump = true;
        _controls.Gameplay.Space.canceled += ctx => _jump = false;
        _controls.Gameplay.Dash.performed += ctx => Dash();
        PlayerHealth.OnPlayerDeath += DisableControls;

        controls.Enable();
    }

    public void Initialize()
    {
        if (PlayerPrefs.GetFloat("PlayerPosY", -10000f) != -10000f)
        {
            float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0);
            float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0);
            float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ", 0);

            float playerRotY = PlayerPrefs.GetFloat("PlayerRotY", 0);

            _playerTransform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
            _playerTransform.rotation = Quaternion.Euler(0f, playerRotY, 0f);
        }
        _characterController.enabled = true;
    }

    public void Tick()
    {
        if (_characterController.enabled)
        {
            if(_characterController.isGrounded) _lastGroundedTime = Time.time;

            Vector2 input = _controls.Gameplay.Movement.ReadValue<Vector2>();
            Vector3 movement;
            if(_shift) movement = _movementSpeed * 2 * Time.deltaTime * (input.x * _playerTransform.right + input.y * _playerTransform.forward);
            else movement = _movementSpeed * Time.deltaTime * (input.x * _playerTransform.right + input.y * _playerTransform.forward);
            _characterController.Move(movement);

            _characterController.Move(_grav * Time.deltaTime * Vector3.up);
            if (_characterController.isGrounded) _grav = 0f;
            else _grav += _gravity * Time.deltaTime;
            if (_jump && (Time.time < _lastGroundedTime + _jumpHelpTime) && IsSurfaceJumpable())
            {
                _grav = _jumpSpeed;
                _lastGroundedTime = 0;
                _playerAudioSource.PlayOneShot(_jumpSound);
            }
        }

        if(_grav == 0f && !_isLandingSoundPlayed)
        {
            _playerAudioSource.PlayOneShot(_jumpSound);
            _isLandingSoundPlayed = true;
        }
        else if(_grav > 5f || _grav < -5f) _isLandingSoundPlayed = false;
    }

    private void Dash()
    {
        if (_canDash)
        {
            AnimationsController.instance.Cooldown(_dashIcon, _dashCooldownTime, _effectDisabledSprite, _effectEnabledSprite);
            AnimationsController.instance.CameraFOVChange(_cam.GetComponent<Camera>(), this);
            _canDash = false;
            _playerInstaller.StartCoroutine(DashCooldown());
        }
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldownTime);
        _canDash = true;
    }

    public void DoDash()
    {
        _characterController.Move(_cam.forward * _dashDistance);
    }

    private bool IsSurfaceJumpable()
    {
        if (Physics.Raycast(_playerTransform.position, Vector3.down, out RaycastHit hit))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle <= _maxJumpAngle;
        }

        return false;
    }

    private void DisableControls()
    {
        _controls.Disable();
    }
}