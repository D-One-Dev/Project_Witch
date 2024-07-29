using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellSelectScreenController : MonoBehaviour
{
    [SerializeField] private float pauseGameSmoothness;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private GameObject spellSelectScreen;
    [SerializeField] private TMP_Text spellNameText, spellDescriptionText, spellCostText;
    private Controls _controls;
    private bool isGamePused;
    private Tween spellSelectScreenFadeOutTween;
    private Spell currentSpell;
    private GameObject currentCard;
    private Effect currentEffect;

    public static SpellSelectScreenController instance;

    private void Awake()
    {
        instance = this;
        _controls = new Controls();
        _controls.Gameplay.Tab.performed += ctx => TriggerSpellSelectScreen();
        _controls.Gameplay.LMB.performed += ctx =>
        {
            if (isGamePused) SetLeftSpell();
        };
        _controls.Gameplay.RMB.performed += ctx =>
        {
            if (isGamePused) SetRightSpell();
        };
        _controls.Gameplay.Q.performed += ctx =>
        {
            if (isGamePused) SetLeftEffect();
        };
        _controls.Gameplay.E.performed += ctx =>
        {
            if (isGamePused) SetRightEffect();
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
            NewSpellCaster.instance.ClearCastList();
            NewSpellCaster.instance.UpdateSpellIcons();
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
