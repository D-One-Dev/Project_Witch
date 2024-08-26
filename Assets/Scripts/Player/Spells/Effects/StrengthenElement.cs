using Projectiles;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Strengthen Element")]
public class StrengthenElement : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectName { get { return "Strengthen element"; } }
    public override string EffectDescription { get { return "Increases damage to vulnurable enemies but decreases damage to resistant enemies"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().isElementStrengthened = true;
        }
    }
}