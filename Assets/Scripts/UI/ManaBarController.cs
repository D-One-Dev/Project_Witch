using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{
    [SerializeField] private Image manaBar;
    [SerializeField] private RectTransform manaBarParent;
    [SerializeField] private float smoothness;
    [SerializeField] private Image[] manaFlasks;

    public static ManaBarController instance;

    private float _barTarget;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateFill(int maxMana, int currentMana)
    {
        _barTarget = currentMana / (float) maxMana;
        if(manaBar.fillAmount > _barTarget)
            AnimationsController.instance.UpdateBar(manaBar, _barTarget, manaBarParent, true);
        else AnimationsController.instance.UpdateBar(manaBar, _barTarget, manaBarParent, false);
    }

    public void ShakeManaBar()
    {
        AnimationsController.instance.ShakeBar(manaBarParent);
    }
}