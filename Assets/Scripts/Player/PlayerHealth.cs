using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealth : EntityHealth
{
    [Inject(Id = "DeathScreen")]
    private readonly GameObject _deathScreen;

    private HPBarController _playerHealthBarController;

    public static Action OnPlayerDeath;

    [Inject]
    public void Construct(HPBarController hPBarController)
    {
        _playerHealthBarController = hPBarController;
    }

    public override void UpdateUI()
    {
        _playerHealthBarController.UpdateFill(OriginHealth, health);
    }

    protected override void Death()
    {
        isDead = true;
        Debug.Log("Player is dead!");
        _animationsController.FadeInScreen(_deathScreen);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
    }
}