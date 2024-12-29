using UnityEngine;
using FullSerializer;
public class BoughtSpellWriter : MonoBehaviour
{
    [SerializeField] private BoughtSpellsEnabler boughtSpellsEnabler;
    public ShopSpells spells;
    private static readonly fsSerializer _serializer = new fsSerializer();
    public void BuySpell(int id)
    {
        boughtSpellsEnabler.isUpdated = true;
        spells.spells[id] = (id, true);
    }
}