using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class EntityHealth : MonoBehaviour
{
    public int health;
    protected int OriginHealth;
    [SerializeField] protected DamageType damageResistType;
    [SerializeField] protected DamageType damageVulnerabilityType;
    [SerializeField] protected float surfaceDamageCooldownTime;

    protected AnimationsController _animationsController;

    protected bool isDead;

    protected Coroutine surfaceDamageCoroutine = null;

    public UnityEvent OnDeath;
    public event HealthChanged OnHealthChanged;
    public delegate void HealthChanged(int health, int originHealth);

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    private void Start()
    {
        OriginHealth = health;
        UpdateUI();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DamagingSurface"))
        {
            TakeSurfaceDamage(other.gameObject.GetComponent<DamagingSurface>().damage);
        }
    }


    public void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        int coef = isElementStrengthened ? 4 : 2;
        if (damageResistType == damageType) damage /= coef;
        else if (damageVulnerabilityType == damageType) damage *= coef;
        if (health - damage > 0)
        {
            health -= damage;
            OnHealthChanged?.Invoke(health, OriginHealth);
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                _animationsController.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
            }
        }
        else
        {
            health = 0;
            if (!isDead) Death();
        }

        UpdateUI();
    }

    public void TakeSurfaceDamage(int damage)
    {
        if (surfaceDamageCoroutine == null)
        {
            surfaceDamageCoroutine = StartCoroutine(SurfaceDamageCooldown());
            if (health - damage > 0)
            {
                health -= damage;
                _animationsController.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
            }

            else
            {
                health = 0;
                if (!isDead) Death();
            }
        }

        UpdateUI();
    }

    private IEnumerator SurfaceDamageCooldown()
    {
        yield return new WaitForSeconds(surfaceDamageCooldownTime);
        surfaceDamageCoroutine = null;
    }

    public virtual void UpdateUI() { }

    protected virtual void Death()
    {
        isDead = true;
        OnDeath.Invoke();
        if (gameObject.TryGetComponent<EnemyMoneyCost>(out EnemyMoneyCost component)) component.DropMoney();
        Destroy(gameObject);
    }
}