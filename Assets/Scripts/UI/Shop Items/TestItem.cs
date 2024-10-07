using UnityEngine;

public class TestItem : ShopItem
{
    public override void OnBuy()
    {
        Debug.Log("Bought test item");
    }
}