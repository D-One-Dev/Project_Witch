using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Increase Size")]
public class IncreaseSize : Effect
{
    public override int ManaCost { get { return 30; } }

    public override string EffectName { get { return "Size increase"; } }
    public override string EffectDescription { get { return "Increases size of projectiles and their damage"; } }
    public override void Activate(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            projectile.transform.localScale *= 2f;
        }
    }
}