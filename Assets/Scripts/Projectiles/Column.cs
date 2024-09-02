using System.Collections;
using UnityEngine;

namespace Projectiles
{
    public class Column : ProjectileBase
    {
        [SerializeField] private GameObject explosionEffect;
        
        protected override void Start()
        {
            base.Start();
            StartCoroutine(Appear());

            ParticleEffectScale = new Vector3(10, 10, 10);
            
            PlayEffectParticles();
        }

        private IEnumerator Appear()
        {
            while (transform.position.y < 0.35f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                yield return null;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                GiveDamage(other.gameObject, false);
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }
    }
}