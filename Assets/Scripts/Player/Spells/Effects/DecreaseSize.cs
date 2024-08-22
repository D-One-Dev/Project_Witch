using Projectiles;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Decrease Size")]
public class DecreaseSize : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectName { get { return "Size decrase"; } }
    public override string EffectDescription { get { return "Decreases size of projectiles and increases their speed"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.transform.localScale *= .75f;
            projectile.GetComponent<Projectile>().ChangeProjectileSpeed(1.5f);
        }
    }
}