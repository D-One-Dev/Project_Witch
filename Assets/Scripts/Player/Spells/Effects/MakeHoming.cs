using System.Collections.Generic;
using Projectiles;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Make Homing")]
public class MakeHoming : Effect
{
    public override int ManaCost { get { return 10; } }
    public override string EffectNameTag { get { return "MakeHomingEffect"; } }
    public override string EffectDescriptionTag { get { return "MakeHomingDescription"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().isHoming = true;
        }
    }
}