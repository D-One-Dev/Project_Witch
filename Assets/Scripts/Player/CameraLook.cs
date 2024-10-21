using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float defaultSens;
    public float currentSens;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CharacterController _characterController;
    private float xRotation = 0f;
    private Controls _controls;

    public static CameraLook instance;

    private void Awake()
    {
        instance = this;
        _controls = new Controls();
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
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (PlayerPrefs.GetFloat("PlayerPosY", -10000f) != -10000f)
        {
            float playerRotX = PlayerPrefs.GetFloat("PlayerRotX", 0);
            transform.localRotation = Quaternion.Euler(playerRotX, 0f, 0f);
            xRotation = transform.localRotation.eulerAngles.x;
        }
    }
    void Update()
    {
        if (_characterController.enabled)
        {
            Vector2 mouseDelta = _controls.Gameplay.MouseDelta.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * currentSens;
            float mouseY = mouseDelta.y * currentSens;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        if (GlobalGamePause.instance.isGamePaused) currentSens = Mathf.Lerp(currentSens, 0f, 10f * Time.deltaTime);
        else currentSens = Mathf.Lerp(currentSens, defaultSens, 10f * Time.deltaTime);
    }
    public void ChangeSens(int value)
    {
        float sens = (float)value / 50 * .15f;
        defaultSens = sens;
        currentSens = sens;
    }
}
