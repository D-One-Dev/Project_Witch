using UnityEngine;

public class SteamCloud : MonoBehaviour
{
    [SerializeField] private GameObject steamCloudPrefab;
    public void CreateSteamCloud(Transform _transform, float puddleScale)
    {
        GameObject steamCloud = Instantiate(steamCloudPrefab, transform.position, steamCloudPrefab.transform.localRotation);
        steamCloud.transform.localScale *= puddleScale * 2;
    }
}