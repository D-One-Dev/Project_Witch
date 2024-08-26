using UnityEngine;

[CreateAssetMenu(menuName = "Main Spell")]

public class Spell : ScriptableObject
{
    public SpellType Type;
    public GameObject[] objectPrefabs;
    public int manaCost;
    public string spellName;
    public string spellDescription;
    public Sprite spellIcon;
}
