using UnityEngine;

[CreateAssetMenu(menuName = "Main Spell")]

public class Spell : ScriptableObject
{
    public GameObject objectPrefab;
    public int manaCost;
}
