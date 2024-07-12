using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private Transform cam;
    private Controls _controls;
    private bool shift;
    private float grav;
    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Shift.performed += ctx => shift = true;
        _controls.Gameplay.Shift.canceled += ctx => shift = false;
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
            if (_characterController.isGrounded) grav = 0;
            else grav += gravity;
            _characterController.Move(grav * Time.deltaTime * Vector3.up);
        }
    }
}
