using UnityEngine;

public class PosionPuddle : MonoBehaviour
{
    [SerializeField] private GameObject puddlePrefab;
    [SerializeField] private float yOffset;
    [SerializeField] private LayerMask lm;

    public void CreatePuddle(Transform _transform, float puddleScale, bool isEnemyHit)
    {
        if (!isEnemyHit)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, lm))
            {
                GameObject puddle = Instantiate(puddlePrefab, hit.point + Vector3.down * yOffset, puddlePrefab.transform.localRotation);
                puddle.transform.localScale *= puddleScale * 2;
                puddle.transform.up = hit.normal;
            }
        }
    }
}