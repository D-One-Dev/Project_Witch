using UnityEngine;

public class DamagingSurfaceLifetime : MonoBehaviour
{
    public void DestroySurface()
    {
        Destroy(transform.parent.gameObject);
    }
}