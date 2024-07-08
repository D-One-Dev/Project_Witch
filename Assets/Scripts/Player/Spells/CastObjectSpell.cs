using UnityEngine;

[CreateAssetMenu(menuName = "Cast Object Spell")]
public class CastObjectSpell : ISpell
{
    public override SpellType Type { get { return SpellType.CastObject; } }

    public GameObject objectPrefab;
    public float objectSpeed;
    public int manaCost;
}