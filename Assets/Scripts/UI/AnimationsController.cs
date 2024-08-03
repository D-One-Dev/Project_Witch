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

    public void FadeInScreen(GameObject screen)
    {
        screen.GetComponent<CanvasGroup>().DOKill();
        screen.GetComponent<CanvasGroup>().alpha = 0f;
        screen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        screen.GetComponent<CanvasGroup>().DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true);
    }
    public Tween FadeOutScreen(GameObject screen)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        return screen.GetComponent<CanvasGroup>().DOFade(0f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() => {screen.SetActive(false);});
    }

    public Tween SpellCardEnter(GameObject spellCard)
    {
        spellCard.GetComponentInChildren<Image>().DOKill();
        return spellCard.GetComponentInChildren<Image>().DOColor(new Color(.75f, .75f, .75f), .1f).SetUpdate(UpdateType.Normal, true);
    }

    public Tween SpellCardExit(GameObject spellCard)
    {
        spellCard.GetComponentInChildren<Image>().DOKill();
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

    public void CameraFOVChange(Camera cam)
    {
        float defaultFOV = cam.fieldOfView;
        cam.DOFieldOfView(defaultFOV * 2f, .05f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        { cam.DOFieldOfView(defaultFOV, .05f).SetEase(Ease.OutElastic);});
    }

    public void Cooldown(Image icon, float cooldownTime)
    {
        icon.DOFillAmount(0f, .1f).OnKill(() => { icon.DOFillAmount(1f, cooldownTime - .1f);});
    }
}
