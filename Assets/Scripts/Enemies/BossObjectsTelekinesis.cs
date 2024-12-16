using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class BossObjectsTelekinesis : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] objectsRb;

        private void Start()
        {
            for (int i = 0; i < objectsRb.Length; i++)
            {
                objectsRb[i].isKinematic = true;
            }

            StartCoroutine(Telekinesing());
        }
        
        private IEnumerator Telekinesing()
        {
            while (objectsRb[0].transform.position.y < 15)
            {
                for (int i = 0; i < objectsRb.Length; i++)
                {
                    objectsRb[i].transform.position = new Vector3(objectsRb[i].transform.position.x, objectsRb[i].transform.position.y + 0.25f, objectsRb[i].transform.position.z);
                    yield return new WaitForSeconds(0.001f);
                }
            }

            for (int i = 0; i < objectsRb.Length; i++)
            {
                StartCoroutine(ThrowOut(i));
            }
        }

        private IEnumerator ThrowOut(int objIndex)
        {
            int direction = Random.Range(0, 4);

            if (direction == 0)
            {
                while (objectsRb[objIndex].transform.position.x < 50)
                {
                    objectsRb[objIndex].transform.position = new Vector3(objectsRb[objIndex].transform.position.x + 0.4f, objectsRb[objIndex].transform.position.y, objectsRb[objIndex].transform.position.z);
                    yield return null;
                }
            }
            else if (direction == 1)
            {
                while (objectsRb[objIndex].transform.position.x > -50)
                {
                    objectsRb[objIndex].transform.position = new Vector3(objectsRb[objIndex].transform.position.x - 0.4f, objectsRb[objIndex].transform.position.y, objectsRb[objIndex].transform.position.z);
                    yield return null;
                }
            }
            else if (direction == 2)
            {
                while (objectsRb[objIndex].transform.position.z < 50)
                {
                    objectsRb[objIndex].transform.position = new Vector3(objectsRb[objIndex].transform.position.x, objectsRb[objIndex].transform.position.y, objectsRb[objIndex].transform.position.z + 0.4f);
                    yield return null;
                }
            }
            else if (direction == 3)
            {
                while (objectsRb[objIndex].transform.position.z > -50)
                {
                    objectsRb[objIndex].transform.position = new Vector3(objectsRb[objIndex].transform.position.x, objectsRb[objIndex].transform.position.y, objectsRb[objIndex].transform.position.z - 0.4f);
                    yield return null;
                }
            }

            objectsRb[objIndex].isKinematic = false;
        }
    }
}