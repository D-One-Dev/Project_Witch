using UnityEngine;

public class LavaPuddleLifetime : MonoBehaviour
{
    public void DestroyPuddle()
    {
        Destroy(transform.parent.gameObject);
    }
}