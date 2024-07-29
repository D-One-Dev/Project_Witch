using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image blackScreen;

    public void NewGame()
    {
        AnimationsController.instance.ChangeScene(blackScreen, 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
