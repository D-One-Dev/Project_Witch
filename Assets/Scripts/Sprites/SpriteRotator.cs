using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform player;
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

    private int bumpMap;

    private void Start()
    {
        if(_SR != null)
        {
            _material = _SR.material;
            _material.EnableKeyword("_BumpMap");
            bumpMap = Shader.PropertyToID("_BumpMap");
        }
    }
    void FixedUpdate()
    {
        transform.LookAt(player.position);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
        if(_SR != null)
        {
            float angle = transform.localEulerAngles.y;
            {
                if (angle <= 45f || angle > 315f)
                {
                    _SR.sprite = frontSprite;
                    _material.SetTexture(bumpMap, frontN);
                }
                else if (angle > 45f && angle <= 135f)
                {
                    _SR.sprite = leftSprite;
                    _material.SetTexture(bumpMap, leftN);
                }
                else if (angle > 135f && angle <= 225f)
                {
                    _SR.sprite = backSprite;
                    _material.SetTexture(bumpMap, backN);
                }
                else
                {
                    _SR.sprite = rightSprite;
                    _material.SetTexture(bumpMap, rightN);
                }
            }
        }
    }
}
