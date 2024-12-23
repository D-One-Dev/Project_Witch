using UnityEngine;

public class GlobalGamePause : MonoBehaviour
{
    [SerializeField] private float pauseGameSmoothness;
    [SerializeField] private CharacterController _characterController;
    public static GlobalGamePause instance;
    public bool isGamePaused;

    private void Awake()
    {
        instance = this;
    }

    public void FixedUpdate()
    {
        if (isGamePaused) Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0f, pauseGameSmoothness);
        else Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, pauseGameSmoothness);
        if (Time.timeScale == 0f) _characterController.enabled = false;
        else _characterController.enabled = true;
    }

    public void SetPause(bool state)
    {
        isGamePaused = state;
        FixedUpdate();
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}