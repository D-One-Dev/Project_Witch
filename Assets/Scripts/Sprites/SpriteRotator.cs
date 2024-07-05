using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform player;
    void FixedUpdate()
    {
        transform.forward = player.transform.forward;
    }
}
