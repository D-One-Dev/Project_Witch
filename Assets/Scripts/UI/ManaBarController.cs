using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{
    [SerializeField] private Image manaBar;
    [SerializeField] private float smoothness;

    public static ManaBarController instance;

    private float barTarget;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, barTarget, smoothness);
    }

    public void UpdateFill(int maxMana, int currentMana)
    {
        barTarget = currentMana / (float) maxMana;
    }
}
