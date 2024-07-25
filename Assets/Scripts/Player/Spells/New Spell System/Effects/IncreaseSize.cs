using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Increase Size")]
public class IncreaseSize : Effect
{
    public override int ManaCost { get { return 30; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.transform.localScale *= 1.5f;
        }
    }
}