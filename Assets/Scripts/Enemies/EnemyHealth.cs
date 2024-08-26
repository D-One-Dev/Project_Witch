using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private DamageType damageResistType;
    [SerializeField] private DamageType damageVulnerabilityType;

    public UnityEvent onDeath;

    public void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        int coef = isElementStrengthened ? 4 : 2;
        if (damageResistType == damageType) damage /= coef;
        else if (damageVulnerabilityType == damageType) damage *= coef;
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
