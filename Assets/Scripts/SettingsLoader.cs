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
        mixer.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-80f, 0f, file.soundVolume / 100f));
        mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80f, 0, file.musicVolume / 100f));

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
            default:
                QualitySettings.SetQualityLevel(2);
                break;

        }
    }
}
