using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Spell spell;
    [SerializeField] private TMP_Text spellNameText;
    [SerializeField] private Image spellIconImage;

    private void Start()
    {
        spellNameText.text = spell.name;
        spellIconImage.sprite = spell.spellIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardEnter(gameObject);
        SpellSelectScreenController.instance.SetCurrentSpell(spell, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardExit(gameObject);
        SpellSelectScreenController.instance.ClearCurrentSpell();
    }
}