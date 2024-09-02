using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private Transform cam;
    [SerializeField] private AudioSource _AS;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldownTime;
    [SerializeField] private Image dashIcon;
    [SerializeField] private float jumpHelpTime;
    [SerializeField] private float maxJumpAngle;
    [SerializeField] private Sprite effectDisabledSprite;
    [SerializeField] private Sprite effectEnabledSprite;
    private float lasGroundedTime;
    private bool isLandingSoundPlayed;
    private Controls _controls;
    private bool shift;
    private bool jump;
    private float grav;
    private bool canDash = true;
    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Shift.started += ctx => shift = true;
        _controls.Gameplay.Shift.canceled += ctx => shift = false;
        _controls.Gameplay.Space.started += ctx => jump = true;
        _controls.Gameplay.Space.canceled += ctx => jump = false;
        _controls.Gameplay.Dash.performed += ctx => Dash();
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
    private void Update()
    {
        if (_characterController.enabled)
        {
            if (_characterController.isGrounded) lasGroundedTime = Time.time;

            Vector2 input = _controls.Gameplay.Movement.ReadValue<Vector2>();
            Vector3 movement;
            if(shift) movement = movementSpeed * 2 * Time.deltaTime * (input.x * transform.right + input.y * transform.forward);
            else movement = movementSpeed * Time.deltaTime * (input.x * transform.right + input.y * transform.forward);
            _characterController.Move(movement);

            _characterController.Move(grav * Time.deltaTime * Vector3.up);
            if (_characterController.isGrounded) grav = 0f;
            else grav += gravity * Time.deltaTime;
            if (jump && (Time.time < lasGroundedTime + jumpHelpTime) && IsSurfaceJumpable())
            {
                grav = jumpSpeed;
                lasGroundedTime = 0;
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

    private void Dash()
    {
        if (canDash)
        {
            AnimationsController.instance.Cooldown(dashIcon, dashCooldownTime, effectDisabledSprite, effectEnabledSprite);
            AnimationsController.instance.CameraFOVChange(cam.GetComponent<Camera>(), this);
            canDash = false;
            StartCoroutine(DashCooldown());
        }
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }

    public void DoDash()
    {
        _characterController.Move(cam.transform.forward * dashDistance);
    }

    private bool IsSurfaceJumpable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle <= maxJumpAngle;
        }

        return false;
    }
}
