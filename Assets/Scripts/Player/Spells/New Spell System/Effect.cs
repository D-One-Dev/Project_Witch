using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public virtual int ManaCost { get; }
    public virtual void Activate(List<GameObject> projectiles)
    {

    }
}