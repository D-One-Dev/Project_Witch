using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        if(File.Exists(Application.dataPath + "/save.savefile"))
        {
            loadButton.interactable = true;
        }
        else
        {
             loadButton.interactable = false;
        }
    }

    public void NewGame()
    {
        if(File.Exists(Application.dataPath + "/save.savefile")) File.Delete(Application.dataPath + "/save.savefile");
        if (File.Exists(Application.dataPath + "/save.savefile.meta"))File.Delete(Application.dataPath + "/save.savefile.meta");
        AnimationsController.instance.ChangeScene(blackScreen, 1);
    }

    public void LoadGame()
    {
        AnimationsController.instance.ChangeScene(blackScreen, 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
