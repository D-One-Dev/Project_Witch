using UnityEngine;

public abstract class ISpell : ScriptableObject
{
    public abstract SpellType Type
    {
        get;
    }

    public int manaCost;
}