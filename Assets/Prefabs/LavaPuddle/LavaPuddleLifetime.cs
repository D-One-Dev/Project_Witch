using UnityEngine;

public class LavaPuddleLifetime : MonoBehaviour
{
    public void DestroyPuddle()
    {
        Destroy(gameObject);
    }
}