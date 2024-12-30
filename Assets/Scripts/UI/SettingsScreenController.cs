using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VolFx;
using Zenject;
using Lean.Localization;
using System;

public class SettingsScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text soundText;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private TMP_Text musicText;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private TMP_Text mouseSensText;
    [SerializeField] private Slider mouseSensSlider;

    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown voiceLanguageDropdown;
    [SerializeField] private TMP_Dropdown textLanguageDropdown;
    [SerializeField] private TMP_Dropdown windowTypeDropdown;
    [SerializeField] private TMP_Dropdown VSyncDropdown;

    private SavesController _savesController;
    private int soundVolume = -1;
    private int musicVolume = -1;
    private float mouseSens = -1f;
    private int graphics = -1;
    private int voiceLanguage = -1;
    private int textLanguage = -1;
    private int windowType = -1;
    private int VSync = -1;

    [Inject]
    public void Construct(SavesController savesController)
    {
        _savesController = savesController;
    }

    private void Start()
    {
        LoadSettings();
    }

    private void OnEnable()
    {
        LoadSettings();
    }

    public void OnSoundVolumeChanged()
    {
        soundVolume = soundSlider.value.RoundToInt();
        soundText.text = LeanLocalization.GetTranslationText("SoundSetting") + soundVolume;
        SaveSettings();
    }

    public void OnMusicVolumeChanged()
    {
        musicVolume = musicSlider.value.RoundToInt();
        musicText.text = LeanLocalization.GetTranslationText("MusicSetting") + musicVolume;
        SaveSettings();
    }

    public void OnMouseSensChanged()
    {
        mouseSens = mouseSensSlider.value;
        mouseSensText.text = LeanLocalization.GetTranslationText("MouseSensSetting") + Math.Round(mouseSens, 1);
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
        switch (textLanguage)
        {
            case 0:
                LeanLocalization.SetCurrentLanguageAll("Russian");
                break;
            case 1:
                LeanLocalization.SetCurrentLanguageAll("English");
                break;
            default:
                LeanLocalization.SetCurrentLanguageAll("English");
                break;
        }
        soundText.text = LeanLocalization.GetTranslationText("SoundSetting") + soundVolume;
        musicText.text = LeanLocalization.GetTranslationText("MusicSetting") + musicVolume;
        mouseSensText.text = LeanLocalization.GetTranslationText("MouseSensSetting") + Math.Round(mouseSens, 1);
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
        _savesController.SaveSettings(soundVolume, musicVolume, graphics, voiceLanguage, textLanguage, windowType, VSync, mouseSens);
    }

    public void LoadSettings()
    {
        if (!File.Exists(Application.dataPath + "/settings.savefile")) _savesController.SaveSettings(50, 50, 3, 0, 0, 0, 0, 1f);
        
        SavesController.SettingsFile file = _savesController.LoadSettings();
        
        if (file != null)
        {
            soundVolume = file.soundVolume;
            musicVolume = file.musicVolume;
            graphics = file.graphics;
            voiceLanguage = file.voiceLanguage;
            textLanguage = file.textLanguage;
            windowType = file.windowType;
            VSync = file.VSync;
            mouseSens = file.mouseSens;

            if (soundText) soundText.text = LeanLocalization.GetTranslationText("SoundSetting") + soundVolume;
            if (musicText) musicText.text = LeanLocalization.GetTranslationText("MusicSetting") + musicVolume;
            if (mouseSensText) mouseSensText.text = LeanLocalization.GetTranslationText("MouseSensSetting") + Math.Round(mouseSens, 1);

            if (soundSlider) soundSlider.value = soundVolume;
            if (musicSlider) musicSlider.value = musicVolume;
            if (mouseSensSlider) mouseSensSlider.value = mouseSens;

            if (graphicsDropdown) graphicsDropdown.value = graphics;
            if (voiceLanguageDropdown) voiceLanguageDropdown.value = voiceLanguage;
            if (textLanguageDropdown) textLanguageDropdown.value = textLanguage;
            if (windowTypeDropdown) windowTypeDropdown.value = windowType;
            if (VSyncDropdown) VSyncDropdown.value = VSync;
        }
    }
}
