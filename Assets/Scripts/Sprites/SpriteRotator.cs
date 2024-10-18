using UnityEngine;
using Zenject;

public class SpriteRotator : MonoBehaviour
{
    [Inject(Id = "PlayerTransform")]
    private readonly Transform _playerTransform;
    [SerializeField] private SpriteRenderer _SR;
    [Space]
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [Space]
    [SerializeField] private Texture frontN;
    [SerializeField] private Texture backN;
    [SerializeField] private Texture leftN;
    [SerializeField] private Texture rightN;
    private Material _material;

    private int _bumpMap;

    private void Start()
    {
        if(_playerTransform == null)
        {
            Debug.LogErrorFormat("Player transform on " + GetComponentInParent<Transform>().gameObject.name + " not set");
        }
        if(_SR != null)
        {
            _material = _SR.material;
            _material.EnableKeyword("_BumpMap");
            _bumpMap = Shader.PropertyToID("_BumpMap");
        }
    }
    void FixedUpdate()
    {
        if(_playerTransform != null)
        {
            transform.LookAt(_playerTransform.position);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
            if (_SR != null)
            {
                float angle = transform.localEulerAngles.y;
                {
                    if (angle <= 45f || angle > 315f)
                    {
                        _SR.sprite = frontSprite;
                        _material.SetTexture(_bumpMap, frontN);
                    }
                    else if (angle > 45f && angle <= 135f)
                    {
                        _SR.sprite = rightSprite;
                        _material.SetTexture(_bumpMap, rightN);
                    }
                    else if (angle > 135f && angle <= 225f)
                    {
                        _SR.sprite = backSprite;
                        _material.SetTexture(_bumpMap, backN);
                    }
                    else
                    {
                        _SR.sprite = leftSprite;
                        _material.SetTexture(_bumpMap, leftN);
                    }
                }
            }
        }

    }
}