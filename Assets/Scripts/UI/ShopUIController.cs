using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private TMP_Text itemNameText, itemDescriptionText, itemCostText;
    private Controls _controls;
    private ShopItem currentItem;
    private GameObject currentCardUI;
    private ItemCard currentCard;

    private bool isShopScreenActive;

    public static ShopUIController instance;

    private void Awake()
    {
        instance = this;
        _controls = new Controls();

        _controls.Gameplay.T.performed += ctx =>
        {
            TriggerShopScreen();
        };

        _controls.Gameplay.Esc.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused && isShopScreenActive) TriggerShopScreen();
        };
        _controls.Gameplay.LMB.performed += ctx =>
        {
            if (GlobalGamePause.instance.isGamePaused) BuyItem();
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

    private void TriggerShopScreen()
    {
        if (!GlobalGamePause.instance.isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AnimationsController.instance.FadeInScreen(shopScreen);
            GlobalGamePause.instance.isGamePaused = true;
            isShopScreenActive = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AnimationsController.instance.FadeOutScreen(shopScreen);
            GlobalGamePause.instance.isGamePaused = false;
            GlobalGamePause.instance.FixedUpdate();
            NewSpellCaster.instance.ClearCastList();
            isShopScreenActive = false;
        }
    }

    public void SetCurrentItem(ShopItem item, GameObject itemCardUI, ItemCard itemCard)
    {
        currentItem = item;
        currentCardUI = itemCardUI;
        currentCard = itemCard;
        itemNameText.text = currentCard.itemName;
        itemDescriptionText.text = currentCard.itemDescription;
        itemCostText.text = "Cost: " + currentCard.cost;
    }

    private void BuyItem()
    {
        int playerMoney = PlayerMoney.Instance.Balance;
        if (currentItem != null)
        {
            if(currentCard.GetComponent<ItemCard>().cost <= playerMoney)
            {
                AnimationsController.instance.ClickButton(currentCardUI);
                currentItem.OnBuy();
                PlayerMoney.Instance.ChangeBalance(-currentCard.GetComponent<ItemCard>().cost);
            }
            else
            {
                Debug.LogFormat("Can't afford " + currentCard.itemName);
                AnimationsController.instance.ShakeBar(currentCardUI.GetComponent<RectTransform>());
            }
        }
    }

    public void ClearCurrentItem()
    {
        currentItem = null;
    }
}