using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected LayerMask targetLayer;
        
        [SerializeField] protected string targetTag;

        [SerializeField] protected GameObject deathParticles;

        [SerializeField] protected onDestroyEvent onDestroy;

        public float lifeTime = 30f;
        public float lifeTimeAfterCollide;
        public int damage;
        public DamageType damageType;

        protected Transform target;

        [Serializable]
        protected class onDestroyEvent : UnityEvent<Transform, float> { }


        protected virtual void Start() => StartCoroutine(Life());

        protected void GiveDamage(GameObject targetObj, bool isElementStrengthened)
        {
            if (targetObj.TryGetComponent(out EnemyHealth enemyHealth)) enemyHealth.TakeDamage(damage, damageType, isElementStrengthened);
            StartCoroutine(Destroying());
        }

        private IEnumerator Destroying()
        {
            yield return new WaitForSeconds(lifeTimeAfterCollide);
            PlayDeathParticles();
            onDestroy?.Invoke(transform, transform.localScale.x);
            Destroy(gameObject);
        }

        protected virtual IEnumerator Life()
        {
            yield return new WaitForSeconds(lifeTime);
            PlayDeathParticles();
            onDestroy?.Invoke(transform, transform.localScale.x);
            Destroy(gameObject);
        }

        protected void PlayDeathParticles()
        {
            if(deathParticles != null)
            {
                GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity, transform.parent);
                particles.transform.localScale = transform.localScale;
            }
        }
    }
}