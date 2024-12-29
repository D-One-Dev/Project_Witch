using UnityEngine;

[System.Serializable]
public class RockSpellShopItem : ShopItem
{
    private BoughtSpellWriter _boughtSpellWriter;
    public override void OnBuy()
    {
        _boughtSpellWriter = GameObject.FindObjectOfType<BoughtSpellWriter>();
        _boughtSpellWriter.BuySpell(2);
    }
}