using System.Collections;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource _AS;
    [SerializeField] private CharacterController _CC;
    [SerializeField] private AudioClip[] footstepsDefault, footstepsStone, footstepsGrass;
    [SerializeField] private LayerMask _layerMask;
    private bool running;
    private surfaceType _surfaceType;
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Shift.started += ctx => running = true;
        _controls.Gameplay.Shift.canceled += ctx => running = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private enum surfaceType
    {
        None = 0,
        Stone = 1,
        Grass = 2
    }

    private void FixedUpdate()
    {
        if (_CC.isGrounded)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _layerMask))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Sound_Stone":
                        _surfaceType = surfaceType.Stone;
                        break;
                    case "Sound_Grass":
                        _surfaceType = surfaceType.Grass;
                        break;
                    default:
                        _surfaceType = surfaceType.None;
                        break;
                }
            }
        }

        if (_controls.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero && _CC.isGrounded)
        {
            if (!_AS.isPlaying)
            {
                if (_AS.clip == null) ChangeSound();
                _AS.Play();
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
        if(_AS.isPlaying) return true;
        return false;
    }

    private void ChangeSound()
    {
        int soundIndex;
        switch (_surfaceType)
        {
            case surfaceType.None:
                soundIndex = Random.Range(0, footstepsDefault.Length);
                _AS.clip = footstepsDefault[soundIndex];
                break;
            case surfaceType.Stone:
                soundIndex = Random.Range(0, footstepsStone.Length);
                _AS.clip = footstepsStone[soundIndex];
                break;
            case surfaceType.Grass:
                soundIndex = Random.Range(0, footstepsGrass.Length);
                _AS.clip = footstepsGrass[soundIndex];
                break;
        }

        if (running) _AS.pitch = Random.Range(1.15f, 1.35f);//1.25f;
        else _AS.pitch = Random.Range(.9f, 1.1f);//1f;
        if (_controls.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero && _CC.isGrounded)
        {
            if (!_AS.isPlaying)
            {
                _AS.Play();
            }
        }
        StartCoroutine(WaitForEndOfSound());
    }
}