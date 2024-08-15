using UnityEngine;
using UnityEngine.Audio;
using static SavesController;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixer;

    public static SettingsLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateSettings(SavesController.instance.LoadSettings());
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
    }
}
