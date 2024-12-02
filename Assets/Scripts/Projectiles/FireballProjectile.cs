using HealthSystem;
using Projectiles;
using UnityEngine;

public class FireballProjectile : Projectile
{
    [SerializeField] private float areaDamageRadius;
    public void DamageArea()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, transform.localScale.x * areaDamageRadius, targetLayer);
        foreach (Collider enemy in enemies)
        {
            enemy.gameObject.GetComponent<IDamageable>().TakeDamage(damage / 2, damageType, isElementStrengthened);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * areaDamageRadius);
    }
}