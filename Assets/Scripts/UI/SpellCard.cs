using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Spell spell;
    [SerializeField] private TMP_Text spellNameText;
    [SerializeField] private Image spellIconImage;

    private Tween cardTween;

    private void Start()
    {
        spellNameText.text = spell.name;
        spellIconImage.sprite = spell.spellIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardTween = AnimationsController.instance.SpellCardEnter(gameObject, cardTween);
        SpellSelectScreenController.instance.SetCurrentSpell(spell, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardTween = AnimationsController.instance.SpellCardExit(gameObject, cardTween);
        SpellSelectScreenController.instance.ClearCurrentSpell();
    }
}