using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private DamageType damageResistType;
    [SerializeField] private DamageType damageVulnerabilityType;

    public UnityEvent onDeath;

    public void TakeDamage(int damage, DamageType damageType)
    {
        if (damageResistType == damageType) damage /= 2;
        else if (damageVulnerabilityType == damageType) damage *= 2;
        if(health - damage > 0)
        {
            health -= damage;
        }

        else
        {
            onDeath.Invoke();
            gameObject.TryGetComponent<EnemyMoneyCost>(out EnemyMoneyCost component);
            if (component) component.DropMoney();
            Destroy(gameObject);
        }
    }
}
