using UnityEngine;

[CreateAssetMenu(menuName = "Effect Object Spell")]
public class EffectObjectSpell : ISpell
{
    public override SpellType Type { get { return SpellType.EffectObject; } }

    //public GameObject objectPrefab;
    //public float objectSpeed;
    public Effect effect;
    //public int manaCost;

    public enum Effect
    {
        SizeIncrease = 0,
        none = 1
    }

}