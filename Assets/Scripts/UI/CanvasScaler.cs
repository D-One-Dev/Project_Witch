using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    [Range(0f, 1000f)]
    [SerializeField] private float ratio;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform player;
    void FixedUpdate()
    {
        transform.localScale = Vector3.one * Vector3.Distance(canvas.position, player.position) * ratio;
    }

    private void OnValidate()
    {
        transform.localScale = Vector3.one * Mathf.Pow(Vector3.Distance(canvas.position, player.position), 2) * ratio;
    }
}
