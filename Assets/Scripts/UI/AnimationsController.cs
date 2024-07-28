using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationsController : MonoBehaviour
{
    public static AnimationsController instance;
    private void Awake()
    {
        instance = this;
    }

    public void ClickButton(GameObject button)
    {
        button.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f), .1f).OnComplete(() =>
            { button.GetComponent<Image>().DOColor(Color.white, .1f); });
        button.GetComponent<RectTransform>().DOScale(new Vector3(.75f, .75f, 1f), .1f).OnComplete(() =>
            { button.GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), .1f); });
    }

    public void UpdateBar(Image bar, float value, RectTransform barParent, bool isDraining)
    {
        bar.DOFillAmount(value, .2f).SetEase(Ease.OutBack);
        if(isDraining) barParent.DOLocalMoveY(150f, .1f).OnComplete(() =>
            { barParent.DOLocalMoveY(175f, .1f); });
    }

    public void ShakeBar(RectTransform barParent)
    {
        barParent.DOShakePosition(.1f, 10f, 100);
    }
}
