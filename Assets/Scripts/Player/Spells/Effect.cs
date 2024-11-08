using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public virtual int ManaCost { get; }

    public virtual string EffectNameTag { get; }
    public virtual string EffectDescriptionTag { get; }

    public Sprite EffectIcon;
    public virtual void Activate(List<GameObject> projectiles) {}
}