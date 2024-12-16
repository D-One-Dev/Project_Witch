using HealthSystem;
using UnityEngine;

namespace Enemies
{
    public class AttackInArea : MonoBehaviour
    {
        [SerializeField] protected string targetTag;
        public int damage;
        public DamageType damageType;
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                if (other.gameObject.TryGetComponent(out IDamageable entityHealth)) entityHealth.TakeDamage(damage, damageType, false);
            }
        }
    }
}