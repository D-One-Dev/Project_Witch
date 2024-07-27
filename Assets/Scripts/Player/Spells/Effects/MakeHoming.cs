using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Make Homing")]
public class MakeHoming : Effect
{
    public override int ManaCost { get { return 30; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.GetComponent<Projectile>().isHoming = true;
        }
    }
}