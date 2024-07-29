using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Effect effect;
    [SerializeField] private TMP_Text effectNameText;
    [SerializeField] private Image effectIconImage;

    private Tween cardTween;

    private void Start()
    {
        effectNameText.text = effect.EffectName;
        effectIconImage.sprite = effect.EffectIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardTween = AnimationsController.instance.SpellCardEnter(gameObject, cardTween);
        SpellSelectScreenController.instance.SetCurrentEffect(effect, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardTween = AnimationsController.instance.SpellCardExit(gameObject, cardTween);
        SpellSelectScreenController.instance.ClearCurrentEffect();
    }
}