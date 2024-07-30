using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class AnimationsController : MonoBehaviour
{
    public static AnimationsController instance;

    private AsyncOperation sceneLoadOperation;
    private void Awake()
    {
        instance = this;
    }

    public void ClickButton(GameObject button)
    {
        button.GetComponent<Image>().DOColor(new Color(.5f, .5f, .5f), .1f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
            { button.GetComponent<Image>().DOColor(Color.white, .1f).SetUpdate(UpdateType.Normal, true); });
        button.GetComponent<RectTransform>().DOScale(new Vector3(.85f, .85f, 1f), .1f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
            { button.GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), .1f).SetUpdate(UpdateType.Normal, true); });
    }

    public void UpdateBar(Image bar, float value, RectTransform barParent, bool isDraining)
    {
        bar.DOFillAmount(value, .2f).SetEase(Ease.OutBack).SetLink(bar.gameObject);
        if(isDraining) barParent.DOLocalMoveY(150f, .1f).SetLink(bar.gameObject).OnComplete(() =>
           { barParent.DOLocalMoveY(175f, .1f).SetLink(bar.gameObject); });
    }

    public void ShakeBar(RectTransform barParent)
    {
        barParent.DOShakePosition(.1f, 10f, 100);
    }

    public void FadeInScreen(GameObject screen, Tween fadeOutTween)
    {
        if (fadeOutTween != null) fadeOutTween.Kill();
        screen.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        screen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        screen.GetComponent<Image>().DOFade(.8f, .5f).SetUpdate(UpdateType.Normal, true);
    }
    public Tween FadeOutScreen(GameObject screen)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        return screen.GetComponent<Image>().DOFade(0f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() => {screen.SetActive(false); });
    }

    public Tween SpellCardEnter(GameObject spellCard, Tween spellCardExitTween)
    {
        if (spellCardExitTween != null) spellCardExitTween.Kill();
        return spellCard.GetComponentInChildren<Image>().DOColor(new Color(.75f, .75f, .75f), .1f).SetUpdate(UpdateType.Normal, true);
    }

    public Tween SpellCardExit(GameObject spellCard, Tween spellCardEnterTween)
    {
        if (spellCardEnterTween != null) spellCardEnterTween.Kill();
        return spellCard.GetComponentInChildren<Image>().DOColor(new Color(1f, 1f, 1f), .1f).SetUpdate(UpdateType.Normal, true);
    }

    public void ChangeScene(Image blackScreen, int sceneID)
    {
        blackScreen.enabled = true;
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneID);
        sceneLoadOperation.allowSceneActivation = false;
        blackScreen.DOColor(Color.black, 1f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            sceneLoadOperation.allowSceneActivation = true;
        });
    }

    public void ImageInOutFade(Image image)
    {
        image.DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() => { image.DOFade(0f, .5f).SetUpdate(UpdateType.Normal, true); });
    }
}
