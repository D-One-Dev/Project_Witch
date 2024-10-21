using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject settingsScreen;
    private bool isPauseActive;
    private Controls _controls;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Esc.performed += ctx => TriggerPauseScreen();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void TriggerPauseScreen()
    {
        if (!GlobalGamePause.instance.isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _animationsController.FadeInScreen(pauseScreen);
            GlobalGamePause.instance.isGamePaused = true;
            isPauseActive = true;
        }
        else
        {
            if(settingsScreen.activeSelf) _animationsController.FadeOutScreen(settingsScreen);
            else if (isPauseActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _animationsController.FadeOutScreen(pauseScreen);
                GlobalGamePause.instance.isGamePaused = false;
                GlobalGamePause.instance.FixedUpdate();
                isPauseActive = false;
                settingsScreen.SetActive(false);
            }
        }

    }

    public void Quit()
    {
        _animationsController.ChangeScene(loadingScreen, 0);
    }

    public void RestartLevel()
    {
        _animationsController.ChangeScene(loadingScreen, SceneManager.GetActiveScene().buildIndex);
    }
}
