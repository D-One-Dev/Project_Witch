using System.Collections;
using HealthSystem;
using UnityEngine;

public class PoisonedRockSplash : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private int damage;

    private void Start()
    {
        StartCoroutine(Life());
        DamageEnemeies();
    }

    private void DamageEnemeies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, enemies);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out IDamageable health)) health.TakeDamage(damage, DamageType.Poison, false);
        }
    }

    private IEnumerator Life()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
