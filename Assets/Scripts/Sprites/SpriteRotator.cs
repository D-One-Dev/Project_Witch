using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Sprite frontSprite, backSprite, leftSprite, rightSprite;
    [SerializeField] private Texture frontN, backN, leftN, rightN;
    [SerializeField] private SpriteRenderer _SR;
    [SerializeField] private Material _material;

    private void Start()
    {
        _material.EnableKeyword("_NORMALMAP");
    }
    void FixedUpdate()
    {
        transform.LookAt(player.position);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
        float angle = transform.localEulerAngles.y;
        {
            if (angle <= 45f || angle > 315f)
            {
                //Debug.Log(Shader.PropertyToID("_NORMALMAP"));
                _SR.sprite = frontSprite;
                //_material.SetTexture(1327, frontN);
                _material.SetTexture("_BumpMap", frontN);
            }
            else if (angle > 45f && angle <= 135f)
            {
                _SR.sprite = leftSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), leftN);
                _material.SetTexture("_BumpMap", leftN);
            }
            else if (angle > 135f && angle <= 225f)
            {
                _SR.sprite = backSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), backN);
                _material.SetTexture("_BumpMap", backN);
            }
            else
            {
                _SR.sprite = rightSprite;
                //_material.SetTexture(Shader.PropertyToID("_NORMALMAP"), rightN);
                _material.SetTexture("_BumpMap", rightN);
            }
        }
    }
}
