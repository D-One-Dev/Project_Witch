using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    public class MultiplyingEnemyUnitExpander : MonoBehaviour
    {
        [SerializeField] private GameObject enemyUnitPrefab;

        [SerializeField] private float maxScale, minScale;
        
        private void Start()
        {
            GetComponent<EnemyHealth>().onDeath.AddListener(OnMultiply);

            if (transform.localScale.x < maxScale)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 10), 50, Random.Range(0, 10)) * 1000);
            }
        }

        private void OnMultiply()
        {
            if (transform.localScale.x <= minScale) return;
            
            for (int i = 0; i < 3; i++)
            {
                GameObject child = Instantiate(enemyUnitPrefab, transform.position, transform.rotation);
                child.transform.localScale /= 2;
                child.GetComponent<EnemyHealth>().originHealth = GetComponent<EnemyHealth>().originHealth / 2;
            }
        }
    }
}