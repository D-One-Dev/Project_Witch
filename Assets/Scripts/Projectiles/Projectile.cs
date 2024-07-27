using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public bool isHoming;
    public float rotationSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float targetLockRadius;
    private Transform target;
    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (isHoming)
        {
            if(target == null)
            {
                Collider[] hit = Physics.OverlapSphere(transform.position, targetLockRadius, enemies);
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
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, targetLockRadius);
    }
}
