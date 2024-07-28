using DG.Tweening;
using UnityEngine;

public class SpellSelectScreenController : MonoBehaviour
{
    [SerializeField] private float pauseGameSmoothness;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject spellSelectScreen;
    private Controls _controls;
    private bool isGamePused;
    private Tween spellSelectScreenFadeOutTween;

    private void Awake()
    {
        _controls = new Controls();
        _controls.Gameplay.Tab.performed += ctx => TriggerSpellSelectScreen();
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
        if (isGamePused) Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0f, pauseGameSmoothness);
        else Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, pauseGameSmoothness);
        if(Time.timeScale == 0f) _characterController.enabled = false;
        else _characterController.enabled = true;
    }

    private void TriggerSpellSelectScreen()
    {
        if (!isGamePused)
        {
            AnimationsController.instance.FadeInScreen(spellSelectScreen, spellSelectScreenFadeOutTween);
            isGamePused = true;
        }
        else
        {
            spellSelectScreenFadeOutTween = AnimationsController.instance.FadeOutScreen(spellSelectScreen);
            isGamePused = false;
            FixedUpdate();
        }
    }
}
