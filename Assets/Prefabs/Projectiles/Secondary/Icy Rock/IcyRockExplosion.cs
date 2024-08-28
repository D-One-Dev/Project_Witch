using UnityEngine;

public class IcyRockExplosion : MonoBehaviour
{
    [SerializeField] private GameObject iceShard;
    [SerializeField] private int shardsAmount;
    public void Explode(Transform _tansform, float projectileScale)
    {
        Transform projectilesLayer = GameObject.Find("Projectiles").transform;
        Vector3 pos = transform.position;
        shardsAmount = (int)(shardsAmount * 2 * projectileScale);
        Debug.Log(shardsAmount);
        for (int i = 0; i < shardsAmount; i++)
        {
            Vector3 randomForward = Vector3.up + Vector3.right * Random.Range(-10f, 10f) + Vector3.forward * Random.Range(-10f, 10f);
            GameObject shard = Instantiate(iceShard, pos, Quaternion.identity, projectilesLayer);
            shard.transform.forward = randomForward;
            shard.transform.localScale *= projectileScale * 2;
        }
    }
}