using Projectiles;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Decrease Size")]
public class DecreaseSize : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectNameTag { get { return "DecreaseSizeEffect"; } }
    public override string EffectDescriptionTag { get { return "DecreaseSizeDescription"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.transform.localScale *= .75f;
            projectile.GetComponent<Projectile>().ChangeProjectileSpeed(1.5f);
        }
    }
}