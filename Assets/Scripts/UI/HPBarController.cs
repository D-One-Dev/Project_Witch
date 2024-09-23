using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private RectTransform hpBarParent;
    [SerializeField] private float smoothness;
    [SerializeField] private Image[] hpFlasks;

    public static HPBarController instance;

    private float _barTarget;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateFill(int maxHp, int currentHp)
    {
        _barTarget = currentHp / (float) maxHp;
        if (hpBar.fillAmount > _barTarget)
            AnimationsController.instance.UpdateBar(hpBar, _barTarget, hpBarParent, true);
        else AnimationsController.instance.UpdateBar(hpBar, _barTarget, hpBarParent, false);
    }
}