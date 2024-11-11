using TMPro;
using UnityEngine;
using Zenject;
using Lean.Localization;

public class SpellSelectScreenController : MonoBehaviour
{
    [SerializeField] private GameObject spellSelectScreen;
    [SerializeField] private TMP_Text spellNameText, spellDescriptionText, spellCostText;
    private Controls _controls;

    private Spell currentSpell;
    private GameObject currentCard;
    private Effect currentEffect;

    private bool isSpellScreenActive;

    public static SpellSelectScreenController instance;

    private NewSpellCaster _newSpellCaster;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(NewSpellCaster newSpellCaster, AnimationsController animationsController)
    {
        _newSpellCaster = newSpellCaster;
        _animationsController = animationsController;
    }

    private void Awake()
    {
        instance = this;
        _controls = new Controls();
        _controls.Gameplay.Tab.performed += ctx => TriggerSpellSelectScreen();
        _controls.Gameplay.Esc.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused && isSpellScreenActive) TriggerSpellSelectScreen();
        };
        _controls.Gameplay.LMB.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused) SetLeftSpell();
        };
        _controls.Gameplay.RMB.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused) SetRightSpell();
        };
        _controls.Gameplay.Q.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused) SetLeftEffect();
        };
        _controls.Gameplay.E.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused) SetRightEffect();
        };

    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void TriggerSpellSelectScreen()
    {
        if (!GlobalGamePause.instance.isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _animationsController.FadeInScreen(spellSelectScreen);
            GlobalGamePause.instance.isGamePaused = true;
            isSpellScreenActive = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _animationsController.FadeOutScreen(spellSelectScreen);
            GlobalGamePause.instance.isGamePaused = false;
            GlobalGamePause.instance.FixedUpdate();
            _newSpellCaster.ClearCastList();
            isSpellScreenActive = false;
        }
    }

    public void SetCurrentSpell(Spell spell, GameObject spellCard)
    {
        currentSpell = spell;
        currentCard = spellCard;
        spellNameText.text = LeanLocalization.GetTranslationText(spell.spellNameTag);
        spellDescriptionText.text = LeanLocalization.GetTranslationText(spell.spellDescriptionTag);
        spellCostText.text = LeanLocalization.GetTranslationText("ManaCost") + spell.manaCost;
    }

    private void SetLeftSpell()
    {
        if (currentSpell != null)
        {
            _newSpellCaster.LeftSpell = currentSpell;
            _animationsController.ClickButton(currentCard);
            _newSpellCaster.UpdateSpellIcons();
        }
    }

    private void SetRightSpell()
    {
        if (currentSpell != null)
        {
            _newSpellCaster.RightSpell = currentSpell;
            _animationsController.ClickButton(currentCard);
            _newSpellCaster.UpdateSpellIcons();
        }
    }

    public void SetCurrentEffect(Effect effect, GameObject effectCard)
    {
        currentEffect = effect;
        currentCard = effectCard;
        spellNameText.text = LeanLocalization.GetTranslationText(effect.EffectNameTag);
        spellDescriptionText.text = LeanLocalization.GetTranslationText(effect.EffectDescriptionTag);
        spellCostText.text = LeanLocalization.GetTranslationText("ManaCost") + effect.ManaCost;
    }

    private void SetLeftEffect()
    {
        if (currentEffect != null)
        {
            _newSpellCaster.LeftEffect = currentEffect;
            _animationsController.ClickButton(currentCard);
            _newSpellCaster.UpdateSpellIcons();
        }
    }

    private void SetRightEffect()
    {
        if (currentEffect != null)
        {
            _newSpellCaster.RightEffect = currentEffect;
            _animationsController.ClickButton(currentCard);
            _newSpellCaster.UpdateSpellIcons();
        }
    }

    public void ClearCurrentSpell()
    {
        currentSpell = null;
    }

    public void ClearCurrentEffect()
    {
        currentEffect = null;
    }
}
