using System.Collections;
using UnityEngine;

namespace Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected LayerMask targetLayer;
        
        [SerializeField] protected string targetTag;

        [SerializeField] protected GameObject deathParticles;
        
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
            PlayDeathParticles();
            Destroy(gameObject);
        }

        private IEnumerator Life()
        {
            yield return new WaitForSeconds(lifeTime);
            PlayDeathParticles();
            Destroy(gameObject);
        }

        protected void PlayDeathParticles()
        {
            if(deathParticles != null)
            {
                GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity, transform.parent);
                //ParticleSystem ps = particles.GetComponent<ParticleSystem>();
                //var main = ps.main;
                //main.scalingMode = ParticleSystemScalingMode.Hierarchy;
                particles.transform.localScale = transform.localScale;
            }
        }
    }
}