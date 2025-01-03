using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ItemCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemNameId;
    public Sprite itemIcon;
    public string itemDescriptionId;
    public int cost;

    [SerializeField] private ShopItem item;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemIconImage;

    private AnimationsController _animationsController;
    private ShopUIController _shopUIController;

    [Inject]
    public void Construct(AnimationsController animationsController, ShopUIController shopUIController)
    {
        _animationsController = animationsController;
        _shopUIController = shopUIController;
    }

    private void Start()
    {
        itemNameText.text = LeanLocalization.GetTranslationText(itemNameId);
        itemIconImage.sprite = itemIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animationsController.SpellCardEnter(gameObject);
        ShopUIController.instance.SetCurrentItem(item, gameObject.GetComponentInChildren<Image>().gameObject, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animationsController.SpellCardExit(gameObject);
        ShopUIController.instance.ClearCurrentItem();
    }
}