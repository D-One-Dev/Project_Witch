using UnityEngine;

[CreateAssetMenu(menuName = "Main Spell")]

public class Spell : ScriptableObject
{
    public SpellType Type;
    public GameObject[] objectPrefabs;
    public int manaCost;
    public string spellNameTag;
    public string spellDescriptionTag;
    public Sprite spellIcon;
}
