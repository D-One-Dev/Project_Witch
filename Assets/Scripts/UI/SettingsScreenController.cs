using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VolFx;
using static SavesController;

public class SettingsScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text soundText;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private TMP_Text musicText;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown voiceLanguageDropdown;
    [SerializeField] private TMP_Dropdown textLanguageDropdown;
    [SerializeField] private TMP_Dropdown windowTypeDropdown;
    [SerializeField] private TMP_Dropdown VSyncDropdown;

    private int soundVolume = -1;
    private int musicVolume = -1;
    private int graphics = -1;
    private int voiceLanguage = -1;
    private int textLanguage = -1;
    private int windowType = -1;
    private int VSync = -1;

    private void OnEnable()
    {
        LoadSettings();
    }

    public void OnSoundVolumeChanged()
    {
        soundVolume = soundSlider.value.RoundToInt();
        soundText.text = "sounds: " + soundVolume;
        SaveSettings();
    }

    public void OnMusicVolumeChanged()
    {
        musicVolume = musicSlider.value.RoundToInt();
        musicText.text = "sounds: " + musicVolume;
        SaveSettings();
    }

    public void OnGraphicsChanged()
    {
        graphics = graphicsDropdown.value;
        SaveSettings();
    }
    public void OnVoiceLanguageChanged()
    {
        voiceLanguage = voiceLanguageDropdown.value;
        SaveSettings();
    }

    public void OnTextLanguageChanged()
    {
        textLanguage = textLanguageDropdown.value;
        SaveSettings();
    }

    public void OnWindowTypeChanged()
    {
        windowType = windowTypeDropdown.value;
        SaveSettings();
    }

    public void OnVSyncChanged()
    {
        VSync = VSyncDropdown.value;
        SaveSettings();
    }

    public void SaveSettings()
    {
        SavesController.instance.SaveSettings(soundVolume, musicVolume, graphics, voiceLanguage, textLanguage, windowType, VSync);
    }

    public void LoadSettings()
    {
        SettingsFile file = SavesController.instance.LoadSettings();
        if(file != null)
        {
            soundVolume = file.soundVolume;
            musicVolume = file.musicVolume;
            graphics = file.graphics;
            voiceLanguage = file.voiceLanguage;
            textLanguage = file.textLanguage;
            windowType = file.windowType;
            VSync = file.VSync;

            if (soundText) soundText.text = "sounds: " + soundVolume;
            if (musicText) musicText.text = "sounds: " + musicVolume;

            if (soundSlider) soundSlider.value = soundVolume;
            if (musicSlider) musicSlider.value = musicVolume;

            if (graphicsDropdown) graphicsDropdown.value = graphics;
            if (voiceLanguageDropdown) voiceLanguageDropdown.value = voiceLanguage;
            if (textLanguageDropdown) textLanguageDropdown.value = textLanguage;
            if (windowTypeDropdown) windowTypeDropdown.value = windowType;
            if (VSyncDropdown) VSyncDropdown.value = VSync;
        }
    }
}
