using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private Image healthbar;
    public override void UpdateUI()
    {
        healthbar.fillAmount = (float)health / OriginHealth;
    }

    protected override void Death()
    {
        Debug.Log("Player is dead!");
    }
}