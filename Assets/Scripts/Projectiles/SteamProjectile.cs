using DG.Tweening;
using Projectiles;
using System.Collections;
using UnityEngine;

public class SteamProjectile : Projectile
{
    [SerializeField] private float slowingSpeed;
    private void FixedUpdate()
    {
        rb.velocity *= (1 - slowingSpeed);
    }

    public void DestroyProjectile()
    {
        transform.DOScale(Vector3.zero, .5f).OnKill(() => Destroy(gameObject));
    }

    protected override void OnCollisionEnter(Collision collision) { }

    protected override IEnumerator Life()
    {
        yield return new WaitForSeconds(lifeTime);
        PlayDeathParticles();
        onDestroy?.Invoke(transform, transform.localScale.x,false);
    }
}