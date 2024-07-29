using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpellSelectScreenController : MonoBehaviour
{
    [SerializeField] private GameObject spellSelectScreen;
    [SerializeField] private TMP_Text spellNameText, spellDescriptionText, spellCostText;
    private Controls _controls;

    private Tween spellSelectScreenFadeOutTween;
    private Spell currentSpell;
    private GameObject currentCard;
    private Effect currentEffect;

    private bool isSpellScreenActive;

    public static SpellSelectScreenController instance;

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
            AnimationsController.instance.FadeInScreen(spellSelectScreen, spellSelectScreenFadeOutTween);
            GlobalGamePause.instance.isGamePaused = true;
            isSpellScreenActive = true;
        }
        else
        {
            spellSelectScreenFadeOutTween = AnimationsController.instance.FadeOutScreen(spellSelectScreen);
            GlobalGamePause.instance.isGamePaused = false;
            GlobalGamePause.instance.FixedUpdate();
            NewSpellCaster.instance.ClearCastList();
            NewSpellCaster.instance.UpdateSpellIcons();
            isSpellScreenActive = false;
        }
    }

    public void SetCurrentSpell(Spell spell, GameObject spellCard)
    {
        currentSpell = spell;
        currentCard = spellCard;
        spellNameText.text = spell.name;
        spellDescriptionText.text = spell.spellDescription;
        spellCostText.text = "Mana cost: " + spell.manaCost;
    }

    private void SetLeftSpell()
    {
        if (currentSpell != null)
        {
            NewSpellCaster.instance.leftSpell = currentSpell;
            AnimationsController.instance.ClickButton(currentCard);
        }
    }

    private void SetRightSpell()
    {
        if (currentSpell != null)
        {
            NewSpellCaster.instance.rightSpell = currentSpell;
            AnimationsController.instance.ClickButton(currentCard);
        }
    }

    public void SetCurrentEffect(Effect effect, GameObject effectCard)
    {
        currentEffect = effect;
        currentCard = effectCard;
        spellNameText.text = effect.EffectName;
        spellDescriptionText.text = effect.EffectDescription;
        spellCostText.text = "Mana cost: " + effect.ManaCost;
    }

    private void SetLeftEffect()
    {
        if (currentEffect != null)
        {
            NewSpellCaster.instance.leftEffect = currentEffect;
            AnimationsController.instance.ClickButton(currentCard);
        }
    }

    private void SetRightEffect()
    {
        if (currentEffect != null)
        {
            NewSpellCaster.instance.rightEffect = currentEffect;
            AnimationsController.instance.ClickButton(currentCard);
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
