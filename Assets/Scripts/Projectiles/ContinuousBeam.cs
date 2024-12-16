using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(LineRenderer))]
    public class ContinuousBeam : ProjectileBase
    {
        private LineRenderer _lineRenderer;

        protected override void Start()
        {
            base.Start();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward * 100);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                GiveDamage(other.gameObject, false);
            }
        }
    }
}