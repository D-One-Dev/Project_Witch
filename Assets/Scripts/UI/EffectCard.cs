using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Lean.Localization;

public class EffectCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Effect effect;
    [SerializeField] private TMP_Text effectNameText;
    [SerializeField] private Image effectIconImage;
    
    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    private void Start()
    {
        effectNameText.text = LeanLocalization.GetTranslationText(effect.EffectNameTag);
        effectIconImage.sprite = effect.EffectIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animationsController.SpellCardEnter(gameObject);
        SpellSelectScreenController.instance.SetCurrentEffect(effect, gameObject.GetComponentInChildren<Image>().gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animationsController.SpellCardExit(gameObject);
        SpellSelectScreenController.instance.ClearCurrentEffect();
    }
}