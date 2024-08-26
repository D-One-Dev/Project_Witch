using System.Collections;
using UnityEngine;

namespace Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected LayerMask targetLayer;
        
        [SerializeField] protected string targetTag;
        
        public float lifeTime = 30f;
        public float lifeTimeAfterCollide;
        public int damage;
        public DamageType damageType;

        protected Transform target;

        protected virtual void Start() => StartCoroutine(Life());

        protected void GiveDamage(GameObject targetObj, bool isElementStrengthened)
        {
            if (targetObj.TryGetComponent(out EnemyHealth enemyHealth)) enemyHealth.TakeDamage(damage, damageType, isElementStrengthened);
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            yield return new WaitForSeconds(lifeTimeAfterCollide);
            Destroy(gameObject);
        }

        private IEnumerator Life()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}