using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [Inject(Id = "LoadingScreen")]
    private readonly GameObject _loadingScreen;
    [Inject(Id = "LoadGameButton")]
    private readonly Button _loadButton;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    public void Start()
    {
        if(File.Exists(Application.dataPath + "/save.savefile"))
        {
            _loadButton.interactable = true;
        }
        else
        {
             _loadButton.interactable = false;
        }
    }

    public void NewGame()
    {
        if(File.Exists(Application.dataPath + "/save.savefile")) File.Delete(Application.dataPath + "/save.savefile");
        if (File.Exists(Application.dataPath + "/save.savefile.meta"))File.Delete(Application.dataPath + "/save.savefile.meta");
        if(File.Exists(Application.dataPath + "/shopItems.savefile")) File.Delete(Application.dataPath + "/shopItems.savefile");
        if (File.Exists(Application.dataPath + "/shopItems.savefile.meta"))File.Delete(Application.dataPath + "/shopItems.savefile.meta");
        if(File.Exists(Application.dataPath + "/boughtSpells.savefile")) File.Delete(Application.dataPath + "/boughtSpells.savefile");
        if (File.Exists(Application.dataPath + "/boughtSpells.savefile.meta"))File.Delete(Application.dataPath + "/boughtSpells.savefile.meta");
        _animationsController.ChangeScene(_loadingScreen, 1);
    }

    public void LoadGame()
    {
        _animationsController.ChangeScene(_loadingScreen);
    }

    public void QuitGame()

    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}