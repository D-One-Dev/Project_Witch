using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{
    [SerializeField] private Image manaBar;
    [SerializeField] private RectTransform manaBarParent;
    [SerializeField] private float smoothness;

    public static ManaBarController instance;

    private float barTarget;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateFill(int maxMana, int currentMana)
    {
        barTarget = currentMana / (float) maxMana;
        if(manaBar.fillAmount > barTarget)
            AnimationsController.instance.UpdateBar(manaBar, currentMana / (float)maxMana, manaBarParent, true);
        else AnimationsController.instance.UpdateBar(manaBar, currentMana / (float)maxMana, manaBarParent, false);
    }

    public void ShakeManaBar()
    {
        AnimationsController.instance.ShakeBar(manaBarParent);
    }
}
