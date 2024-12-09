using UnityEngine;

namespace Enemies
{
    public class SlimePoisonPuddle : MonoBehaviour
    {
        [SerializeField] private GameObject puddlePrefab;
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private Transform mainTransform;
        [SerializeField] private float yOffset;
        [SerializeField] private LayerMask lm;
        [SerializeField] private float puddleScale;

        public void CreatePuddle()
        {
            RaycastHit hit;
            if (Physics.Raycast(spawnTransform.position, Vector3.down, out hit, Mathf.Infinity, lm))
            {
                GameObject puddle = Instantiate(puddlePrefab, hit.point + Vector3.down * yOffset, puddlePrefab.transform.localRotation);
                puddle.transform.localScale *= puddleScale * 2 * (mainTransform.localScale.x / 2);
                puddle.transform.up = hit.normal;
            }
        }
    }
}