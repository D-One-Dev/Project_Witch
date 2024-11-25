using HealthSystem;
using UnityEngine;
using UnityEngine.Events;

public class PartOfColliderTrigger : MonoBehaviour, IDamageable
{
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collider> onTriggerStay;
    public UnityEvent<Collider> onTriggerExit;
    public UnityEvent<int, DamageType, bool> onPJHit;
    
    public void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened)
    {
        onPJHit?.Invoke(damage, damageType, isElementStrengthened);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
}