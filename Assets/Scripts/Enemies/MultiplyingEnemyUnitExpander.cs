using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(EntityHealth))]
    public class MultiplyingEnemyUnitExpander : MonoBehaviour
    {
        private DiContainer _container;

        [SerializeField] private GameObject enemyUnitPrefab;

        [SerializeField] private float maxScale, minScale;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Start()
        {
            GetComponent<EntityHealth>().onDeath.AddListener(OnMultiply);

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
                GameObject child = _container.InstantiatePrefab(enemyUnitPrefab, transform.position, transform.rotation, null);
                child.transform.localScale /= 2;
                child.GetComponent<EntityHealth>().health /= 2;
            }
        }
    }
}