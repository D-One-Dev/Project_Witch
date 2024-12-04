using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float startScale = 10;
    public float explosionScale = 20;
    
    private void Awake() => StartCoroutine(DestroyExplosionEffect());

    private IEnumerator DestroyExplosionEffect()
    {
        transform.localScale = new Vector3(startScale, startScale, startScale);

        while (transform.localScale.x < explosionScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + 1f, transform.localScale.y + 1f,
            transform.localScale.z + 1f);
            yield return new WaitForSeconds(0.0001f);
        }
            
        Destroy(gameObject);
    }
}