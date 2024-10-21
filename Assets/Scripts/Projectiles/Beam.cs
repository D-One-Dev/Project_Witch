using System.Collections;
using UnityEngine;
using Zenject;

namespace Projectiles
{
    [RequireComponent(typeof(LineRenderer))]
    public class Beam : ProjectileBase
    {
        private LineRenderer _lineRenderer;

        [Inject(Id = "PlayerTransform")]
        private readonly Transform _player;

        private Vector3 _playerLastPosition;

        protected override void Start()
        {
            base.Start();
            _lineRenderer = GetComponent<LineRenderer>();
            _playerLastPosition = _player.position;

            StartCoroutine(ShootBeam());
        }

        private IEnumerator ShootBeam()
        {
            yield return new WaitForSeconds(0.12f);
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _playerLastPosition);
            
            if (_playerLastPosition.Equals(_player.position))
            {
                print("you are beamed");
                GiveDamage(_player.gameObject, false);
            }
        }
    }
}