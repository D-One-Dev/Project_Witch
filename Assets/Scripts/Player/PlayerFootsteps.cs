using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioSource _AS;
    [SerializeField] private CharacterController _CC;
    [SerializeField] private AudioClip footstepsDefault, footstepsStone;
    private bool running;
    private Controls _controls;
    [SerializeField] private LayerMask _layerMask;


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

    private void FixedUpdate()
    {
        if (running) _AS.pitch = 1.25f;
        else _AS.pitch = 1f;

        if (_CC.isGrounded)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.gameObject.CompareTag("Sound_Stone")) _AS.clip = footstepsStone;
                else _AS.clip = footstepsDefault;
            }
        }

        if (_controls.Gameplay.Movement.ReadValue<Vector2>() != Vector2.zero && _CC.isGrounded)
        {
            _AS.loop = true;
            if (!_AS.isPlaying)
            {
                _AS.Play();
            }
        }

        else
        {
            _AS.loop = false;
        }
    }
}
