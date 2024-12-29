using UnityEngine;

[System.Serializable]
public class PoisonSpellShopItem : ShopItem
{
    private BoughtSpellWriter _boughtSpellWriter;
    public override void OnBuy()
    {
        _boughtSpellWriter = GameObject.FindObjectOfType<BoughtSpellWriter>();

        _boughtSpellWriter.BuySpell(3);
    }
}