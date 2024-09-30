using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public int health;
    protected int OriginHealth;
    [SerializeField] protected DamageType damageResistType;
    [SerializeField] protected DamageType damageVulnerabilityType;
    [SerializeField] protected float surfaceDamageCooldownTime;

    protected bool isDead;

    protected Coroutine surfaceDamageCoroutine = null;

    public UnityEvent onDeath;
    public event HealthChanged onHealthChanged;
    public delegate void HealthChanged(int health, int originHealth);

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
            onHealthChanged?.Invoke(health, OriginHealth);
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                AnimationsController.instance.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
            }
        }
        else
        {
            Death();
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
                AnimationsController.instance.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
            }

            else
            {
                Death();
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
        if (!isDead)
        {
            isDead = true;
            onDeath.Invoke();
            if (gameObject.TryGetComponent<EnemyMoneyCost>(out EnemyMoneyCost component)) component.DropMoney();
            Destroy(gameObject);
        }
    }
}