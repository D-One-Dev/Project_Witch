using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private Image healthbar;
    [SerializeField] private GameObject deathScreen;

    public static Action OnPlayerDeath;

    public override void UpdateUI()
    {
        healthbar.fillAmount = (float)health / OriginHealth;
    }

    protected override void Death()
    {
        isDead = true;
        Debug.Log("Player is dead!");
        AnimationsController.instance.FadeInScreen(deathScreen);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
    }
}