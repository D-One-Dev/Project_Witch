using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealth : EntityHealth
{
    [Inject(Id = "Healthbar")]
    private readonly Image _healthbar;
    [Inject(Id = "DeathScreen")]
    private readonly GameObject _deathScreen;

    public static Action OnPlayerDeath;

    public override void UpdateUI()
    {
        _healthbar.fillAmount = (float)health / OriginHealth;
    }

    protected override void Death()
    {
        isDead = true;
        Debug.Log("Player is dead!");
        AnimationsController.instance.FadeInScreen(_deathScreen);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
    }
}