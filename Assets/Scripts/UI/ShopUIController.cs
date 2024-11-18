using TMPro;
using UnityEngine;
using Zenject;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private TMP_Text itemNameText, itemDescriptionText, itemCostText;
    [SerializeField] private GameObject[] items;
    private Controls _controls;
    private ShopItem currentItem;
    private GameObject currentCardUI;
    private ItemCard currentCard;
    private SavesController _savesController;

    private bool isShopScreenActive;

    public static ShopUIController instance;

    private PlayerMoney _playerMoney;

    private NewSpellCaster _newSpellCaster;

    private AnimationsController _animationsController;

    private (ShopItem, bool)[] _itemsDict;

    [Inject]
    public void Construct(PlayerMoney playerMoney, NewSpellCaster newSpellCaster, AnimationsController animationsController, SavesController savesController)
    {
        _playerMoney = playerMoney;
        _newSpellCaster = newSpellCaster;
        _animationsController = animationsController;
        _savesController = savesController;
        SetItemsDict();
    }

    private void Awake()
    {
        instance = this;
        _controls = new Controls();

        _controls.Gameplay.T.performed += ctx =>
        {
            SetItemsDict();
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
            _animationsController.FadeInScreen(shopScreen);
            GlobalGamePause.instance.isGamePaused = true;
            isShopScreenActive = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _animationsController.FadeOutScreen(shopScreen);
            GlobalGamePause.instance.isGamePaused = false;
            GlobalGamePause.instance.FixedUpdate();
            _newSpellCaster.ClearCastList();
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
        if (currentItem != null)
        {
            if(_playerMoney.CheckPurchaseAbility(currentCard.GetComponent<ItemCard>().cost))
            {
                _animationsController.ClickButton(currentCardUI);
                currentItem.OnBuy();
                _playerMoney.ChangeBalance(-currentCard.GetComponent<ItemCard>().cost);
            }
            else
            {
                Debug.LogFormat("Can't afford " + currentCard.itemName);
                _animationsController.ShakeBar(currentCardUI.GetComponent<RectTransform>());
            }
        }
    }

    public void ClearCurrentItem()
    {
        currentItem = null;
    }

    private void SetItemsDict()
    {
        _itemsDict = new (ShopItem, bool)[items.Length];
        if(_savesController.LoadShopItems() == null)
        {
            for(int i = 0; i < items.Length; i++)
            {
                _itemsDict[i] = (items[i].GetComponent<ShopItem>(), false);
            }
            _savesController.SaveShopItems(_itemsDict);
        }
        else
        {
            _itemsDict = _savesController.LoadShopItems().shopItems;
        }
        foreach (GameObject item in items)
        {
            ShopItem shopItem = item.GetComponent<ShopItem>();
            foreach (var a in _itemsDict)
            {
                if(a.Item1.GetType() == shopItem.GetType())
                {
                    if(a.Item2) item.SetActive(true);
                    else item.SetActive(false);
                    break;
                }
            }
        }
    }
}