using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer _SR;
    [Space]
    [SerializeField] private Sprite frontSprite;//, backSprite, leftSprite, rightSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [Space]
    [SerializeField] private Texture frontN;//, backN, leftN, rightN;
    [SerializeField] private Texture backN;
    [SerializeField] private Texture leftN;
    [SerializeField] private Texture rightN;
    /*[SerializeField]*/ private Material _material;

    private int bumpMap;

    private void Start()
    {
        _material = _SR.material;
        //_material.EnableKeyword("_NORMALMAP");
        _material.EnableKeyword("_BumpMap");
        bumpMap = Shader.PropertyToID("_BumpMap");
    }
    void FixedUpdate()
    {
        transform.LookAt(player.position);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
        float angle = transform.localEulerAngles.y;
        {
            if (angle <= 45f || angle > 315f)
            {
                _SR.sprite = frontSprite;
                //_material.SetTexture(1327, frontN);
                _material.SetTexture(bumpMap, frontN);
            }
            else if (angle > 45f && angle <= 135f)
            {
                _SR.sprite = leftSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), leftN);
                //_material.SetTexture(1327, leftN);
                _material.SetTexture(bumpMap, leftN);
            }
            else if (angle > 135f && angle <= 225f)
            {
                _SR.sprite = backSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), backN);
                //_material.SetTexture(1327, backN);
                _material.SetTexture(bumpMap, backN);
            }
            else
            {
                _SR.sprite = rightSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), rightN);
                //_material.SetTexture(1327, rightN);
                _material.SetTexture(bumpMap, rightN);
            }
        }
    }
}
