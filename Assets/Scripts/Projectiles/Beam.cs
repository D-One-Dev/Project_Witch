using System.Collections;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(LineRenderer))]
    public class Beam : ProjectileBase
    {
        private LineRenderer _lineRenderer;

        private Vector3 _playerLastPosition;

        protected override void Start()
        {
            base.Start();
            _lineRenderer = GetComponent<LineRenderer>();
            _playerLastPosition = Player.Player.Instance.transform.position;

            StartCoroutine(ShootBeam());
        }

        private IEnumerator ShootBeam()
        {
            yield return new WaitForSeconds(0.12f);
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _playerLastPosition);
            
            if (_playerLastPosition.Equals(Player.Player.Instance.transform.position))
            {
                print("you are beamed");
                GiveDamage(Player.Player.Instance.gameObject);
            }
        }
    }
}