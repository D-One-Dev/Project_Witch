using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerFootsteps : IFixedTickable
{
    [Inject(Id = "PlayerAudioSource")]
    private readonly AudioSource _playerAudioSource;
    [Inject(Id = "PlayerFootstepsDefault")]
    private readonly AudioClip[] _footstepsDefault;
    [Inject(Id = "PlayerFootstepsStone")]
    private readonly AudioClip[] _footstepsStone;
    [Inject(Id = "PlayerFootstepsGrass")]
    private readonly AudioClip[] _footstepsGrass;
    [Inject(Id = "PlayerLayerMask")]
    private readonly LayerMask _layerMask;
    [Inject(Id = "PlayerTransform")]
    private readonly Transform _playerTransform;
    [Inject(Id = "PlayerInstaller")]
    private readonly MonoInstaller _installer;
    private CharacterController _characterController;

    private bool _running;
    private SurfaceType _surfaceType;
    private Controls _controls;

    [Inject]
    public void Construct(Controls controls, CharacterController characterController)
    {
        _controls = controls;
        _characterController = characterController;

        _controls.Gameplay.Shift.started += ctx => _running = true;
        _controls.Gameplay.Shift.canceled += ctx => _running = false;
        PlayerHealth.OnPlayerDeath += DisableControls;
        _controls.Enable();
    }

    private enum SurfaceType
    {
        None = 0,
        Stone = 1,
        Grass = 2
    }

    public void FixedTick()
    {
        if (_characterController.isGrounded)
        {
            if(Physics.Raycast(_playerTransform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Sound_Stone":
                        _surfaceType = SurfaceType.Stone;
                        break;
                    case "Sound_Grass":
                        _surfaceType = SurfaceType.Grass;
                        break;
                    default:
                        _surfaceType = SurfaceType.None;
                        break;
                }
            }
        }

        if (_controls.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero && _characterController.isGrounded)
        {
            if (!_playerAudioSource.isPlaying)
            {
                if (_playerAudioSource.clip == null) ChangeSound();
                _playerAudioSource.Play();
            }
        }
    }

    private IEnumerator WaitForEndOfSound()
    {
        yield return new WaitWhile(IsSoundPlaying);
        ChangeSound();
    }

    private bool IsSoundPlaying()
    {
        if(_playerAudioSource.isPlaying) return true;
        return false;
    }

    private void ChangeSound()
    {
        int soundIndex;
        switch (_surfaceType)
        {
            case SurfaceType.None:
                soundIndex = Random.Range(0, _footstepsDefault.Length);
                _playerAudioSource.clip = _footstepsDefault[soundIndex];
                break;
            case SurfaceType.Stone:
                soundIndex = Random.Range(0, _footstepsStone.Length);
                _playerAudioSource.clip = _footstepsStone[soundIndex];
                break;
            case SurfaceType.Grass:
                soundIndex = Random.Range(0, _footstepsGrass.Length);
                _playerAudioSource.clip = _footstepsGrass[soundIndex];
                break;
        }

        if (_running) _playerAudioSource.pitch = Random.Range(1.15f, 1.35f);
        else _playerAudioSource.pitch = Random.Range(.9f, 1.1f);
        if (_controls.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero && _characterController.isGrounded)
        {
            if (!_playerAudioSource.isPlaying)
            {
                _playerAudioSource.Play();
            }
        }
        _installer.StartCoroutine(WaitForEndOfSound());
    }

    private void DisableControls()
    {
        _controls.Disable();
    }
}