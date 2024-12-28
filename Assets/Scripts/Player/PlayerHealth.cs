using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerHealth : EntityHealth
{
    [Inject(Id = "DeathScreen")]
    private readonly GameObject _deathScreen;

    [Inject (Id = "TimeToStartHeal")]
    private readonly float _timeToStartHeal;

    [Inject (Id = "HealSpeed")]
    private readonly float _healSpeed;

    [Inject (Id = "PlayerSoundBase")]
    private readonly SoundBase _playerSoundBase;

    [Inject (Id = "AlexHurtSound")]
    private readonly AudioClip _alexHurtSound;

    private HPBarController _playerHealthBarController;

    public static Action OnPlayerDeath;

    private Coroutine _healTimer;

    private bool _canHeal = true;

    [Inject]
    public void Construct(HPBarController hPBarController)
    {
        _playerHealthBarController = hPBarController;
    }

    private void Start()
    {
        StartCoroutine(HealCycle());
        OriginHealth = health;
        UpdateUI();
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

    private void Heal(int amount)
    {
        if(health < OriginHealth)
        {
            if (health + amount <= OriginHealth) health += amount;
            else health = OriginHealth;
            UpdateUI();
        }
    }

    private IEnumerator HealTimer()
    {
        _canHeal = false;
        yield return new WaitForSeconds(_timeToStartHeal);
        _canHeal = true;
    }

    private IEnumerator HealCycle()
    {
        yield return new WaitForSeconds(1 / _healSpeed);
        if (_canHeal) Heal(1);
        StartCoroutine(HealCycle());
    }

    public override void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        _playerSoundBase.PlaySoundWithRandomPitch(_alexHurtSound, 1f, 1.25f);
        base.TakeDamage(damage, damageType, isElementStrengthened);
        if(_healTimer == null)
        {
            _healTimer = StartCoroutine(HealTimer());
        }
        else
        {
            StopCoroutine(_healTimer);
            _healTimer = StartCoroutine(HealTimer());
        }
    }
}