using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float speed;
    private float _currentProgress;

    private void Update()
    {
        _currentProgress += Time.deltaTime * speed;
        
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, _currentProgress);

        if (_currentProgress >= 1f)
        {
            _currentProgress = 0f;

            (startPoint, endPoint) = (endPoint, startPoint);
        }
    }
}