using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class Column : ProjectileBase
    {
        [SerializeField] private GameObject targetAimView;
        [SerializeField] private GameObject explosionEffect;

        private GameObject _targetAimViewCache;

        [SerializeField] private float timeForAppear = 0.3f;
        [SerializeField] private float appearStep = 0.1f;
        [SerializeField] private float endPosition = 0.35f;
        [SerializeField] private bool isColumnAnimDisappearing;
        [SerializeField] private bool isColumnDisappearWithEffect;
        [SerializeField] private bool isColumnAppearWithRandomPosition;

        private GameObject _deathParticlesCache;

        [SerializeField] private float effectsScale = 10;

        protected override void OnEnable()
        {
            base.OnEnable();
            
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

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Appear()
        {
            yield return new WaitForSeconds(timeForAppear);
            
            if (isColumnAppearWithRandomPosition) endPosition = Random.Range(endPosition - 0.1f, endPosition + 0.1f);

            while (transform.position.y < endPosition)
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