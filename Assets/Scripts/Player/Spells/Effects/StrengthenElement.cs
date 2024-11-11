using Projectiles;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Strengthen Element")]
public class StrengthenElement : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectNameTag { get { return "StrengthenElementEffect"; } }
    public override string EffectDescriptionTag { get { return "StrengthenElementDescription"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().isElementStrengthened = true;
        }
    }
}