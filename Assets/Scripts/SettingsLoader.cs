using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using static SavesController;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixer;
    private SavesController _savesController;

    public static SettingsLoader Instance;

    [Inject]
    public void Construct(SavesController savesController)
    {
        _savesController = savesController;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateSettings(_savesController.LoadSettings());
    }

    public void UpdateSettings(SettingsFile file)
    {
        if (file.soundVolume == 0) mixer.audioMixer.SetFloat("SoundVolume", -80f);
        else mixer.audioMixer.SetFloat("SoundVolume", Mathf.Log10(file.soundVolume / 100f) * 20);
        if (file.musicVolume == 0) mixer.audioMixer.SetFloat("MusicVolume", -80f);
        else mixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(file.musicVolume / 100f) * 20);

        switch (file.graphics)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
            case 3:
                QualitySettings.SetQualityLevel(3);
                break;
            default:
                QualitySettings.SetQualityLevel(3);
                break;
        }

        switch (file.windowType)
        {
            case 0:
                Screen.fullScreen = true;
                break;
            case 1:
                Screen.fullScreen = false;
                break;
            default:
                Screen.fullScreen = true;
                break;
        }

        switch (file.VSync)
        {
            case 0:
                QualitySettings.vSyncCount = 1;
                break;
            case 1:
                QualitySettings.vSyncCount = 0;
                break;
            default:
                QualitySettings.vSyncCount = 1;
                break;
        }
    }
}
