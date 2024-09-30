using System.Collections;
using UnityEngine;

namespace Projectiles
{
    public class Column : ProjectileBase
    {
        [SerializeField] private GameObject targetAimView;
        [SerializeField] private GameObject explosionEffect;

        private GameObject _targetAimViewCache;

        [SerializeField] private float timeForAppear = 0.3f;
        [SerializeField] private float appearStep = 0.1f;
        [SerializeField] private bool isColumnAnimDisappearing;
        [SerializeField] private bool isColumnDisappearWithEffect;

        private GameObject _deathParticlesCache;

        [SerializeField] private float effectsScale = 10;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Appear());
            StartCoroutine(DestroyTargetAimView());
            
            if (targetAimView != null) _targetAimViewCache = Instantiate(targetAimView, transform.position, targetAimView.transform.rotation);
            transform.position = new Vector3(transform.position.x, -10f, transform.position.z);

            if (!isColumnDisappearWithEffect)
            {
                _deathParticlesCache = deathParticles;
                deathParticles = null;
            }

            ParticleEffectScale = new Vector3(effectsScale, effectsScale, effectsScale);
        }

        private IEnumerator Appear()
        {
            yield return new WaitForSeconds(timeForAppear);

            while (transform.position.y < 0.35f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + appearStep, transform.position.z);
                yield return null;
            }

            if (isColumnAnimDisappearing) StartCoroutine(Disappear());
        }
        
        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(lifeTime - 1f);
            while (transform.position.y > -10f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - appearStep, transform.position.z);
                yield return null;
            }
        }

        private IEnumerator DestroyTargetAimView()
        {
            yield return new WaitForSeconds(timeForAppear + 0.2f);
            if (targetAimView != null)Destroy(_targetAimViewCache);
            yield return new WaitForSeconds(timeForAppear + 0.6f);
            
            deathParticles = _deathParticlesCache;
            PlayEffectParticles();
            deathParticles = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                GiveDamage(other.gameObject, false);
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                print("catched");
            }
        }
    }
}