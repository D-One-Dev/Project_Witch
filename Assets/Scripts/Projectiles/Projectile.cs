using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
    public class Projectile : ProjectileBase
    {
        [SerializeField] protected float targetLockRadius;
        public float speed;
        public bool isHoming;
        public bool isElementStrengthened;
        public float rotationSpeed;
        
        protected override void Start()
        {
            base.Start();
            rb.velocity = transform.forward * speed;
        }

        private void Update()
        {
            if (isHoming)
            {
                if (target == null)
                {
                    Collider[] hit = Physics.OverlapSphere(transform.position, targetLockRadius, targetLayer);
                    if(hit.Length > 0)
                    {
                        Debug.Log("Locked");
                        target = hit[0].transform;
                    }
                }
                else
                {
                    Vector3 direction = target.position - transform.position;
                    direction.Normalize();
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                    rb.velocity = transform.forward * speed;
                }
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(targetTag))
            {
                GiveDamage(collision.gameObject, isElementStrengthened);
            }
            else
            {
                onDestroy?.Invoke(transform, transform.localScale.x);
                PlayDeathParticles();
                Destroy(gameObject);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, targetLockRadius);
        }

        public void ChangeProjectileSpeed(float value)
        {
            speed *= value;
            rb.velocity = transform.forward * speed;
        }
    }
}