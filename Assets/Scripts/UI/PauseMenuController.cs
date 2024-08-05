using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Image blackScreen;
    private bool isPauseActive;
    private Controls _controls;

    private Tween pauseScreenFadeOutTween;

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
            AnimationsController.instance.FadeInScreen(pauseScreen);
            GlobalGamePause.instance.isGamePaused = true;
            isPauseActive = true;
        }
        else
        {
            if (isPauseActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AnimationsController.instance.FadeOutScreen(pauseScreen);
                GlobalGamePause.instance.isGamePaused = false;
                GlobalGamePause.instance.FixedUpdate();
                isPauseActive = false;
            }
        }
    }

    public void Quit()
    {
        //SavesController.instance.Save();
        AnimationsController.instance.ChangeScene(blackScreen, 0);
    }
}
