using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using Zenject;

public class AnimationsController
{
    private AsyncOperation sceneLoadOperation;

    private SavesController _savesController;

    [Inject]
    public void Construct(SavesController savesController)
    {
        _savesController = savesController;
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
        if(isDraining) barParent.DOLocalMoveY(100f, .1f).SetLink(bar.gameObject).OnComplete(() =>
           { barParent.DOLocalMoveY(115f, .1f).SetLink(bar.gameObject); });
    }

    public void ShakeBar(RectTransform barParent)
    {
        barParent.DOShakePosition(.1f, 10f, 100).SetUpdate(UpdateType.Normal, true);
    }

    public void FadeInScreen(GameObject screen)
    {
        screen.GetComponent<CanvasGroup>().DOKill();
        screen.GetComponent<CanvasGroup>().alpha = 0f;
        screen.SetActive(true);
        screen.GetComponent<CanvasGroup>().DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        {
            screen.GetComponent<CanvasGroup>().alpha = 1f;
        });
    }
    public void FadeOutScreen(GameObject screen)
    {
        screen.GetComponent<CanvasGroup>().DOFade(0f, .5f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        {
            screen.SetActive(false);
            screen.GetComponent<CanvasGroup>().alpha = 0f;
        });
    }

    public void SpellCardEnter(GameObject spellCard)
    {
        spellCard.GetComponentInChildren<Image>().DOKill();
        spellCard.GetComponentInChildren<Image>().DOColor(new Color(.75f, .75f, .75f), .1f).SetUpdate(UpdateType.Normal, true);
    }

    public void SpellCardExit(GameObject spellCard)
    {
        spellCard.GetComponentInChildren<Image>().DOKill();
        spellCard.GetComponentInChildren<Image>().DOColor(new Color(1f, 1f, 1f), .1f).SetUpdate(UpdateType.Normal, true);
    }

    public void ChangeScene(GameObject loadingScreen, int sceneID)
    {
        loadingScreen.SetActive(true);
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneID);
        sceneLoadOperation.allowSceneActivation = false;
        loadingScreen.GetComponent<CanvasGroup>().DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            sceneLoadOperation.allowSceneActivation = true;
        });
    }

    public void ChangeScene(GameObject loadingScreen)
    {
        int sceneID = _savesController.GetSceneID();
        loadingScreen.SetActive(true);
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneID);
        sceneLoadOperation.allowSceneActivation = false;
        //Debug.Log(loadingScreen.GetComponent<CanvasGroup>());
        loadingScreen.GetComponent<CanvasGroup>().DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            sceneLoadOperation.allowSceneActivation = true;
        });
    }

    public void ImageInOutFade(Image image)
    {
        image.DOFade(1f, .5f).SetUpdate(UpdateType.Normal, true).OnComplete(() => { image.DOFade(0f, .5f).SetUpdate(UpdateType.Normal, true); });
    }

    public void CameraFOVChange(Camera cam, PlayerMovement playerMovement)
    {
        float defaultFOV = 0f;
        if(cam != null) defaultFOV = cam.fieldOfView;
        if (cam != null) cam.DOFieldOfView(defaultFOV * 2f, .1f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        {
            if (playerMovement != null) playerMovement.DoDash();
            if (cam != null) cam.DOFieldOfView(defaultFOV, .1f).SetEase(Ease.OutElastic);
        });
    }

    public void Cooldown(Image icon, float cooldownTime, Sprite disabledSprite, Sprite enabledSprite)
    {
        if(icon != null) icon.sprite = disabledSprite;
        if (icon != null) icon.DOFillAmount(0f, .1f).OnKill(() => {
            if (icon != null) icon.DOFillAmount(1f, cooldownTime - .1f).OnKill(() =>
        {
            if (icon != null)
            {
                icon.sprite = enabledSprite;
                icon.rectTransform.DOScale(.75f, .1f).OnKill(() => {if (icon != null) icon.rectTransform.DOScale(1f, .1f); });
            }
        });});

    }

    public void ChangeTask(GameObject taskUI, string task)
    {
        taskUI.GetComponent<CanvasGroup>().DOFade(0f, .5f).OnComplete(() =>
        {
            if (task.Length > 0) taskUI.GetComponentInChildren<Image>().enabled = true;
            else taskUI.GetComponentInChildren<Image>().enabled = false;
            taskUI.GetComponentInChildren<TMP_Text>().text = task;
            taskUI.GetComponent<CanvasGroup>().DOFade(1f, .5f);
        });
    }

    public void DamageEnemy(SpriteRenderer sr)
    {
        Material mat = sr.material;
        float alpha = mat.color.a;
        mat.DOColor(new Color(.1f, .1f, .1f, alpha), .1f).OnKill(() => mat.DOColor(new Color(1f, 1f, 1f, alpha), .1f));
    }
}