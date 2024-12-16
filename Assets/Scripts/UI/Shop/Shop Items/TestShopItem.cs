using UnityEngine;

[System.Serializable]
public class TestShopItem : ShopItem
{
    public override void OnBuy()
    {
        Debug.Log("Bought test item");
    }
}