using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int originHealth;
    [SerializeField] private DamageType damageResistType;
    [SerializeField] private DamageType damageVulnerabilityType;
    [SerializeField] private float surfaceDamageCooldownTime;

    private bool isDead;

    private Coroutine surfaceDamageCoroutine = null;

    public UnityEvent onDeath;

    private void Start()
    {
        health = originHealth;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DamagingSurface"))
        {
            Debug.Log("Taking surface damage");
            TakeSurfaceDamage(other.gameObject.GetComponent<DamagingSurface>().damage);
        }
    }

    public void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        int coef = isElementStrengthened ? 4 : 2;
        if (damageResistType == damageType) damage /= coef;
        else if (damageVulnerabilityType == damageType) damage *= coef;
        if(health - damage > 0)
        {
            health -= damage;
            AnimationsController.instance.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
        }

        else
        {
            if (!isDead)
            {
                isDead = true;
                onDeath.Invoke();
                if(gameObject.TryGetComponent<EnemyMoneyCost>(out EnemyMoneyCost component)) component.DropMoney();
                Destroy(gameObject);
            }
        }
    }

    public void TakeSurfaceDamage(int damage)
    {
        if(surfaceDamageCoroutine == null)
        {
            Debug.Log("Taking surface damage");
            surfaceDamageCoroutine = StartCoroutine(SurfaceDamageCooldown());
            if (health - damage > 0)
            {
                health -= damage;
                AnimationsController.instance.DamageEnemy(GetComponentInChildren<SpriteRenderer>());
            }

            else
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
    }

    private IEnumerator SurfaceDamageCooldown()
    {
        yield return new WaitForSeconds(surfaceDamageCooldownTime);
        surfaceDamageCoroutine = null;
    }
}