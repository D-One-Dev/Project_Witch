using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SavesController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image savingIconImage;
    public int currentSceneID;
    public static SavesController instance;

    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(PlayerMoney playerMoney)
    {
        _playerMoney = playerMoney;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(player != null) Load();
        else LoadSceneID();
    }
    private class SaveFile
    {
        public int sceneID;
        public Vector3 playerPosition;
        public quaternion playerRotation;
        public Spell leftSpell, rightSpell;
        public Effect leftEffect, rightEffect;
        public int money;
        public string currentTask;
    }

    public class SettingsFile
    {
        public int soundVolume;
        public int musicVolume;
        public int graphics;
        public int voiceLanguage;
        public int textLanguage;
        public int windowType;
        public int VSync;
    }


    public void Save()
    {
        AnimationsController.instance.ImageInOutFade(savingIconImage);
        SaveFile file = new SaveFile();
        file.sceneID = currentSceneID;
        file.playerPosition = player.transform.position;
        file.playerRotation = player.transform.localRotation;
        file.leftSpell = NewSpellCaster.instance.leftSpell;
        file.rightSpell = NewSpellCaster.instance.rightSpell;
        file.leftEffect = NewSpellCaster.instance.leftEffect;
        file.rightEffect = NewSpellCaster.instance.rightEffect;
        file.money = _playerMoney.Balance;
        file.currentTask = TaskUI.Instance.currentTask;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.savefile", json);
    }

    public void ResetPlayerPos()
    {
        SaveFile file = new SaveFile();
        file.sceneID = currentSceneID;
        file.playerPosition = Vector3.zero;
        file.playerRotation = player.transform.localRotation;
        file.leftSpell = NewSpellCaster.instance.leftSpell;
        file.rightSpell = NewSpellCaster.instance.rightSpell;
        file.leftEffect = NewSpellCaster.instance.leftEffect;
        file.rightEffect = NewSpellCaster.instance.rightEffect;
        file.money = _playerMoney.Balance;
        file.currentTask = TaskUI.Instance.currentTask;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.savefile", json);
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/save.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.savefile");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            currentSceneID = file.sceneID;
            if(file.playerPosition != Vector3.zero)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = file.playerPosition;
                player.transform.localRotation = file.playerRotation;
                player.GetComponent<CharacterController>().enabled = true;
            }
            NewSpellCaster.instance.leftSpell = file.leftSpell;
            NewSpellCaster.instance.rightSpell = file.rightSpell;
            NewSpellCaster.instance.leftEffect = file.leftEffect;
            NewSpellCaster.instance.rightEffect = file.rightEffect;
            _playerMoney.SetBalance(file.money);
            TaskUI.Instance.ChangeTask(file.currentTask);
        }
    }

    private void LoadSceneID()
    {
        if (File.Exists(Application.dataPath + "/save.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.savefile");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            currentSceneID = file.sceneID;
        }
    }

    public void SaveSettings(int soundVolume, int musicVolume, int graphics, int voiceLanguage, int textLanguage, int windowType, int VSync)
    {
        SettingsFile file = new SettingsFile();
        if (file.soundVolume != -1) file.soundVolume = soundVolume;
        else file.soundVolume = 100;
        if (file.musicVolume != -1) file.musicVolume = musicVolume;
        else file.musicVolume = 100;
        if (file.graphics != -1) file.graphics = graphics;
        else file.graphics = 2;
        if (file.voiceLanguage != -1) file.voiceLanguage = voiceLanguage;
        else file.voiceLanguage = 1;
        if (file.textLanguage != -1) file.textLanguage = textLanguage;
        else file.textLanguage = 1;
        if (file.windowType != -1) file.windowType = windowType;
        else file.windowType = 0;
        if (file.VSync != -1) file.VSync = VSync;
        else file.VSync = 0;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/settings.savefile", json);

        SettingsLoader.Instance.UpdateSettings(file);
    }

    public SettingsFile LoadSettings()
    {
        if (File.Exists(Application.dataPath + "/settings.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/settings.savefile");
            SettingsFile file = JsonUtility.FromJson<SettingsFile>(json);
            return file;
        }
        return null;
    }
}