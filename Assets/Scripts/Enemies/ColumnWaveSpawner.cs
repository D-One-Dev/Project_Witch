using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ColumnWaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject column;
        private void Start() => StartCoroutine(SpawningColumWaves());

        private IEnumerator SpawningColumWaves()
        {
            for (int i = 0; i < 20; i++)
            {
                SpawnColumnWave();
                yield return new WaitForSeconds(0.1f);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2f);
            }

            yield return new WaitForSeconds(4);
            
            Destroy(gameObject);
        }
        
        private void SpawnColumnWave()
        {
            GameObject newColumnC = Instantiate(column, transform.position, transform.rotation);
            
            newColumnC.transform.position = new Vector3(newColumnC.transform.position.x,
                column.transform.position.y, newColumnC.transform.position.z);
            
            for (int i = 1; i < 20; i++)
            {
                GameObject newColumn = Instantiate(column, transform.position, transform.rotation);
                
                newColumn.transform.position = new Vector3(newColumn.transform.position.x + 2f * i,
                    column.transform.position.y, newColumn.transform.position.z);
            }
            
            for (int i = 1; i < 20; i++)
            {
                GameObject newColumn = Instantiate(column, transform.position, transform.rotation);
                
                newColumn.transform.position = new Vector3(newColumn.transform.position.x - 2f * i,
                    column.transform.position.y, newColumn.transform.position.z);
            }
        }
    }
}