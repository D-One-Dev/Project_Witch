using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected LayerMask targetLayer;
        
        [SerializeField] protected string targetTag;

        [SerializeField] protected GameObject deathParticles;

        [SerializeField] protected onDestroyEvent onDestroy;
        protected Rigidbody rb;

        public float lifeTime = 30f;
        public float lifeTimeAfterCollide;
        public int damage;
        public DamageType damageType;

        protected Vector3 ParticleEffectScale;

        protected Transform target;

        [Serializable]
        protected class onDestroyEvent : UnityEvent<Transform, float, bool> { }
        
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(Life());
            ParticleEffectScale = transform.localScale;
        }

        protected void GiveDamage(GameObject targetObj, bool isElementStrengthened)
        {
            if (targetObj.TryGetComponent(out EnemyHealth enemyHealth)) enemyHealth.TakeDamage(damage, damageType, isElementStrengthened);
            StartCoroutine(Destroying());
        }

        protected virtual IEnumerator Destroying()
        {
            yield return new WaitForSeconds(lifeTimeAfterCollide);
            DestroyProjectTile(true);
        }

        protected virtual IEnumerator Life()
        {
            yield return new WaitForSeconds(lifeTime);
            DestroyProjectTile(false);
        }

        protected void DestroyProjectTile(bool isEnemyHit)
        {
            PlayEffectParticles();
            onDestroy?.Invoke(transform, transform.localScale.x, isEnemyHit);
            Destroy(gameObject);
        }

        protected void PlayEffectParticles()
        {
            if (deathParticles != null)
            {
                GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity, transform.parent);
                particles.transform.localScale = ParticleEffectScale;
            }
        }
    }
}