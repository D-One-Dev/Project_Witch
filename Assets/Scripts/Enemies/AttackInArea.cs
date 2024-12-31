using System.Collections;
using HealthSystem;
using UnityEngine;

namespace Enemies
{
    public class AttackInArea : MonoBehaviour
    {
        [SerializeField] protected string targetTag;
        public int damage;
        public DamageType damageType;

        [SerializeField] private bool isAttackWhileStaying;

        private bool _isCooldownForAttack;

        [SerializeField] private float cooldownForAttackWhileStaying;
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                if (other.gameObject.TryGetComponent(out IDamageable entityHealth)) entityHealth.TakeDamage(damage, damageType, false);
            }
        }

        protected void OnTriggerStay(Collider other)
        {
            if (!isAttackWhileStaying) return;
            
            if (other.gameObject.CompareTag(targetTag) && !_isCooldownForAttack)
            {
                _isCooldownForAttack = true;
                
                if (other.gameObject.TryGetComponent(out IDamageable entityHealth)) entityHealth.TakeDamage(damage, damageType, false);

                StopAllCoroutines();
                StartCoroutine(CooldownForAttack());
            }
        }

        private IEnumerator CooldownForAttack()
        {
            yield return new WaitForSeconds(cooldownForAttackWhileStaying);
            _isCooldownForAttack = false;
        }

        public void DisableDamage() 
        {
            damage = 0;
        }
    }
}