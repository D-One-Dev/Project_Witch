using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Sprite frontSprite, backSprite, leftSprite, rightSprite;
    [SerializeField] private Texture frontN, backN, leftN, rightN;
    [SerializeField] private SpriteRenderer _SR;
    [SerializeField] private Material _material;
    void FixedUpdate()
    {
        transform.LookAt(player.position);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
        float angle = transform.localEulerAngles.y;
        {
            if (angle <= 45f || angle > 315f)
            {
                _SR.sprite = frontSprite;
                _material.SetTexture(Shader.PropertyToID("_NORMALMAP"), frontN);
            }
            else if (angle > 45f && angle <= 135f)
            {
                _SR.sprite = leftSprite;
                _material.SetTexture(Shader.PropertyToID("_NORMALMAP"), leftN);
            }
            else if (angle > 135f && angle <= 225f)
            {
                _SR.sprite = backSprite;
                _material.SetTexture(Shader.PropertyToID("_NORMALMAP"), backN);
            }
            else
            {
                _SR.sprite = rightSprite;
                _material.SetTexture(Shader.PropertyToID("_NORMALMAP"), rightN);
            }
        }
    }
}
