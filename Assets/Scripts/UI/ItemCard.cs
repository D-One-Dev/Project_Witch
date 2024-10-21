using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;
    public int cost;

    [SerializeField] private ShopItem item;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemIconImage;
    private void Start()
    {
        itemNameText.text = itemName;
        itemIconImage.sprite = itemIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardEnter(gameObject);
        ShopUIController.instance.SetCurrentItem(item, gameObject.GetComponentInChildren<Image>().gameObject, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimationsController.instance.SpellCardExit(gameObject);
        ShopUIController.instance.ClearCurrentItem();
    }
}