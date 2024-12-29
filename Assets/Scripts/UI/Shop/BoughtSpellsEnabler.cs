using System.IO;
using UnityEngine;
using Zenject;

public class BoughtSpellsEnabler : MonoBehaviour
{
    public bool isUpdated;
    private SavesController _savesController;
    [SerializeField] private BoughtSpellWriter _boughtSpellWriter;
    [System.Serializable]
    public class BoughtSpell
    {
        public int Id;
        public GameObject SpellCard;
    }
    [SerializeField] private BoughtSpell[] spells;
    private (int, bool)[] _boughtSpells = new (int, bool)[0];

    [Inject]
    public void Construct(SavesController savesController)
    {
        _savesController = savesController;
    }
    private void OnEnable()
    {
        if(!File.Exists(Application.dataPath + "/boughtSpells.savefile") || (File.Exists(Application.dataPath + "/boughtSpells.savefile") && !isUpdated))_savesController.LoadBoughtSpells();
        _boughtSpells = _boughtSpellWriter.spells.spells;
        foreach(BoughtSpell spell in spells)
        {
            if(IsSpellBought(spell.Id)) spell.SpellCard.SetActive(true);
            else spell.SpellCard.SetActive(false);
        }
    }

    private bool IsSpellBought(int id)
    {
        foreach((int, bool) spell in _boughtSpells)
        {
            if(spell.Item1 == id && spell.Item2) return true;
        }
        return false;
    }
}