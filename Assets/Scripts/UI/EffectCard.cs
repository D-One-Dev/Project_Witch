using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Effect effect;
    [SerializeField] private TMP_Text effectNameText;
    [SerializeField] private Image effectIconImage;
    private void Start()
    {
        effectNameText.text = effect.EffectName;
        effectIconImage.sprite = effect.EffectIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardEnter(gameObject);
        SpellSelectScreenController.instance.SetCurrentEffect(effect, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardExit(gameObject);
        SpellSelectScreenController.instance.ClearCurrentEffect();
    }
}