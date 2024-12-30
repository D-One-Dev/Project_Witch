using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float speed;
    [SerializeField] private float timeForWaitOnPoint;
    
    private float _currentProgress;

    private bool _isPlatformStay;

    private void FixedUpdate()
    {
        if (_isPlatformStay) return;
        
        _currentProgress += Time.fixedDeltaTime * speed;
        
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, _currentProgress);

        if (_currentProgress >= 1f)
        {
            _isPlatformStay = true;
            
            StartCoroutine(Stay());

            _currentProgress = 0f;
            (startPoint, endPoint) = (endPoint, startPoint);
        }
    }

    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(timeForWaitOnPoint);
        _isPlatformStay = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}