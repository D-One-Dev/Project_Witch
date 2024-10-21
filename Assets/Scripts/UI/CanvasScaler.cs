using UnityEngine;
using Zenject;

public class CanvasScaler : MonoBehaviour
{
    [Range(0f, 1000f)]
    [SerializeField] private float ratio;
    [SerializeField] private Transform canvas;
    [Inject(Id = "PlayerTransform")]
    private readonly Transform player;
    void FixedUpdate()
    {
        transform.localScale = ratio * Vector3.Distance(canvas.position, player.position) * Vector3.one;
    }
}
