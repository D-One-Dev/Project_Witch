using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [FormerlySerializedAs("enemies")] [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float targetLockRadius;
    
    public float speed;
    public bool isHoming;
    public float rotationSpeed;
    public float lifeTime = 30f;
    public int damage;
    public DamageType damageType;
    
    private Transform target;
    private void Start()
    {
        StartCoroutine(Life());
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (isHoming)
        {
            if(target == null)
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

    public void SetTargetLayer(LayerMask layerMask) => targetLayer = layerMask;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(targetLayer))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, damageType);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, targetLockRadius);
    }

    private IEnumerator Life()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
