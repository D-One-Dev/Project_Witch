using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private float mouseSens;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    private Controls _controls;

    public static CameraLook instance;

    private void Awake()
    {
        instance = this;
        _controls = new Controls();
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
        //if (PlayerInteraction.instance.playerStatus == 0)
        //{
            Vector2 mouseDelta = _controls.Gameplay.MouseDelta.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * mouseSens;//Time.deltaTime * mouseSens;
            float mouseY = mouseDelta.y * mouseSens;//Time.deltaTime * mouseSens;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        //}
    }

    public void ChangeSens(int value)
    {
        float sens = (float)value / 50 * .15f;
        mouseSens = sens;
    }
}
