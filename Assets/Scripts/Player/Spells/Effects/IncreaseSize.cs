using Projectiles;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Increase Size")]
public class IncreaseSize : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectNameTag { get { return "SizeIncreaseEffect"; } }
    public override string EffectDescriptionTag { get { return "SizeIncreaseDescription"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.transform.localScale *= 2f;
            projectile.GetComponent<ProjectileBase>().damage = Mathf.RoundToInt(projectile.GetComponent<ProjectileBase>().damage * 1.5f);
        }
    }
}