using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ColumnWaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] column;
        
        private void Start() => StartCoroutine(SpawningColumnWaves());

        private IEnumerator SpawningColumnWaves()
        {
            yield return new WaitForSeconds(3);
            
            for (int i = 0; i < 12; i++)
            {
                StartCoroutine(SpawnColumnWave());
                
                yield return new WaitForSeconds(0.1f);
                
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3.4f);
            }

            yield return new WaitForSeconds(1);
            
            Destroy(gameObject);
        }
        
        private IEnumerator SpawnColumnWave()
        {
            GameObject newColumnC = Instantiate(column[Random.Range(0, column.Length)], transform.position, Quaternion.identity);
            
            newColumnC.transform.position = new Vector3(newColumnC.transform.position.x,
                column[0].transform.position.y, newColumnC.transform.position.z);
            
            for (int i = 1; i < 3; i++)
            {
                GameObject newColumn = Instantiate(column[Random.Range(0, column.Length)], transform.position, Quaternion.identity);
                
                newColumn.transform.position = new Vector3(newColumn.transform.position.x + 10.2f * i,
                    column[0].transform.position.y, newColumn.transform.position.z);
            }
            
            for (int i = 1; i < 3; i++)
            {
                GameObject newColumn = Instantiate(column[Random.Range(0, column.Length)], transform.position, Quaternion.identity);
                
                newColumn.transform.position = new Vector3(newColumn.transform.position.x - 10.2f * i,
                    column[0].transform.position.y, newColumn.transform.position.z);
            }
            
            yield return null;
        }
    }
}