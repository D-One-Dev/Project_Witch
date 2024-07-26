using UnityEngine;

[CreateAssetMenu(menuName = "Main Spell")]

public class Spell : ScriptableObject
{
    public SpellType Type;
    public GameObject objectPrefab;
    public int manaCost;
}
