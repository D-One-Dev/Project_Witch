using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Lean.Localization;

public class SpellCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Spell spell;
    [SerializeField] private TMP_Text spellNameText;
    [SerializeField] private Image spellIconImage;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    private void Start()
    {
        spellNameText.text = LeanLocalization.GetTranslationText(spell.spellNameTag);
        spellIconImage.sprite = spell.spellIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animationsController.SpellCardEnter(gameObject);
        SpellSelectScreenController.instance.SetCurrentSpell(spell, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animationsController.SpellCardExit(gameObject);
        SpellSelectScreenController.instance.ClearCurrentSpell();
    }
}