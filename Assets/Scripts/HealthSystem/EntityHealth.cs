using System.Collections;
using System.Collections.Generic;
using HealthSystem;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class EntityHealth : MonoBehaviour, IDamageable
{
    public int health;
    protected int OriginHealth;
    [SerializeField] protected DamageType damageResistType;
    [SerializeField] protected DamageType damageVulnerabilityType;
    [SerializeField] protected float surfaceDamageCooldownTime;

    protected AnimationsController _animationsController;

    protected bool isDead;
    [SerializeField] protected bool isDestroyAfterDead = true;

    protected Coroutine surfaceDamageCoroutine = null;

    public UnityEvent OnDeath;
    public event HealthChanged OnHealthChanged;
    public delegate void HealthChanged(int health, int originHealth);
    
    [SerializeField] private List<PartOfColliderTrigger> partOfColliderTriggers = new ();

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

    private void OnEnable()
    {
        for (int i = 0; i < partOfColliderTriggers.Count; i++)
        {
            partOfColliderTriggers[i].onTriggerStay.AddListener(Triggered);
            partOfColliderTriggers[i].onPJHit.AddListener(TakeDamage);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < partOfColliderTriggers.Count; i++)
        {
            partOfColliderTriggers[i].onTriggerStay.RemoveListener(Triggered);
            partOfColliderTriggers[i].onPJHit.RemoveListener(TakeDamage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Triggered(other);
    }

    private void Triggered(Collider other)
    {
        if (other.gameObject.CompareTag("DamagingSurface"))
        {
            TakeSurfaceDamage(other.gameObject.GetComponent<DamagingSurface>().damage);
            print(other);
        }
    }


    public virtual void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        int coef = isElementStrengthened ? 4 : 2;
        if((damageResistType & damageType) != DamageType.None) damage /= coef;
        if ((damageVulnerabilityType & damageType) != DamageType.None) damage *= coef;
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
        if (gameObject.TryGetComponent(out EnemyMoneyCost component)) component.DropMoney();
        if (isDestroyAfterDead) Destroy(gameObject);
    }
}