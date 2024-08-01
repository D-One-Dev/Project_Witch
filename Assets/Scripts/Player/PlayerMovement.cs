using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private Transform cam;
    [SerializeField] private AudioSource _AS;
    [SerializeField] private AudioClip jumpSound;
    private bool isLandingSoundPlayed;
    private Controls _controls;
    private bool shift;
    private bool jump;
    private float grav;
    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Shift.started += ctx => shift = true;
        _controls.Gameplay.Shift.canceled += ctx => shift = false;
        _controls.Gameplay.Space.started += ctx => jump = true;
        _controls.Gameplay.Space.canceled += ctx => jump = false;
    }
    private void OnEnable()
    {
        _controls.Enable();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }
    private void Start()
    {
        if (PlayerPrefs.GetFloat("PlayerPosY", -10000f) != -10000f)
        {
            float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0);
            float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0);
            float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ", 0);

            float playerRotY = PlayerPrefs.GetFloat("PlayerRotY", 0);

            transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
            transform.rotation = Quaternion.Euler(0f, playerRotY, 0f);
        }
        _characterController.enabled = true;
    }
    void Update()
    {
        if (_characterController.enabled)
        {
            Vector2 input = _controls.Gameplay.Movement.ReadValue<Vector2>();
            Vector3 movement;
            if(shift) movement = movementSpeed * 2 * Time.deltaTime * (input.x * transform.right + input.y * transform.forward);
            else movement = movementSpeed * Time.deltaTime * (input.x * transform.right + input.y * transform.forward);
            _characterController.Move(movement);

            _characterController.Move(grav * Time.deltaTime * Vector3.up);
            if (_characterController.isGrounded && grav < 0) grav = 0;
            else grav += gravity * Time.deltaTime;
            if (jump && _characterController.isGrounded)
            {
                grav = jumpSpeed;
                _AS.PlayOneShot(jumpSound);
            }
        }

        if(grav == 0f && !isLandingSoundPlayed)
        {
            _AS.PlayOneShot(jumpSound);
            isLandingSoundPlayed = true;
        }
        else if(grav > 5f || grav < -5f) isLandingSoundPlayed = false;
    }
}
