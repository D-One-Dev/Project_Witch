using UnityEngine;

[CreateAssetMenu(menuName = "Main Spell")]

public class Spell : ScriptableObject
{
    public SpellType Type;
    public GameObject[] objectPrefabs;
    public int manaCost;
    public string spellName;
    [TextArea]
    public string spellDescription;
    public Sprite spellIcon;
}
