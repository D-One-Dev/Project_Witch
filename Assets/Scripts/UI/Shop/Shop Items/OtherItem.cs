using UnityEngine;

[System.Serializable]
public class OtherItem : ShopItem
{
    public override void OnBuy()
    {
        Debug.Log("Dudka blyat i trubnik");
    }
}