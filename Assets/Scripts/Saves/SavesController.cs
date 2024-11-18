using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using FullSerializer;

public class SavesController : IInitializable
{
    public int CurrentSceneID;

    [Inject(Id = "PlayerTransform")]
    private readonly Transform _playerTransform;
    [Inject(Id = "SaveIconImage")]
    private readonly Image _savingIconImage;

    private PlayerMoney _playerMoney;
    private NewSpellCaster _newSpellCaster;
    private AnimationsController _animationsController;
    private ShopUIController _shopUIController;
    private static readonly fsSerializer _serializer = new fsSerializer();

    [Inject]
    public void Construct(PlayerMoney playerMoney, NewSpellCaster newSpellCaster, AnimationsController animationsController, ShopUIController shopUIController)
    {
        _playerMoney = playerMoney;
        _newSpellCaster = newSpellCaster;
        _animationsController = animationsController;
        _shopUIController = shopUIController;
    }

    public void Initialize()
    {
        if(_playerTransform != null) Load();
        else GetSceneID();
    }
    public class SaveFile
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

    public class ShopItemsFile
    {
        public (ShopItem, bool)[] shopItems;
    }



    public void Save()
    {
        _animationsController.ImageInOutFade(_savingIconImage);
        SaveFile file = new SaveFile();
        //file.sceneID = CurrentSceneID;
        file.sceneID = SceneManager.GetActiveScene().buildIndex;
        file.playerPosition = _playerTransform.position;
        file.playerRotation = _playerTransform.localRotation;
        file.leftSpell = _newSpellCaster.LeftSpell;
        file.rightSpell = _newSpellCaster.RightSpell;
        file.leftEffect = _newSpellCaster.LeftEffect;
        file.rightEffect = _newSpellCaster.RightEffect;
        file.money = _playerMoney.Balance;
        file.currentTask = TaskUI.Instance.currentTask;

        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.savefile", json);

        SaveShopItems(_shopUIController.GetItemsArray());
    }

    public void ResetPlayerPos()
    {
        SaveFile file = new SaveFile();
        file.sceneID = CurrentSceneID;
        file.playerPosition = Vector3.zero;
        file.playerRotation = _playerTransform.localRotation;
        file.leftSpell = _newSpellCaster.LeftSpell;
        file.rightSpell = _newSpellCaster.RightSpell;
        file.leftEffect = _newSpellCaster.LeftEffect;
        file.rightEffect = _newSpellCaster.RightEffect;
        file.money = _playerMoney.Balance;
        file.currentTask = TaskUI.Instance.currentTask;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.savefile", json);
    }

    public SaveFile Load()
    {
        if(File.Exists(Application.dataPath + "/save.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.savefile");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            CurrentSceneID = file.sceneID;
            if(file.playerPosition != Vector3.zero)
            {
                _playerTransform.gameObject.GetComponent<CharacterController>().enabled = false;
                _playerTransform.position = file.playerPosition;
                _playerTransform.localRotation = file.playerRotation;
                _playerTransform.gameObject.GetComponent<CharacterController>().enabled = true;
            }
            _newSpellCaster.LeftSpell = file.leftSpell;
            _newSpellCaster.RightSpell = file.rightSpell;
            _newSpellCaster.LeftEffect = file.leftEffect;
            _newSpellCaster.RightEffect = file.rightEffect;
            _playerMoney.SetBalance(file.money);
            TaskUI.Instance.ChangeTask(file.currentTask);
            return file;
        }
        return null;
    }

    public int GetSceneID()
    {
        if (File.Exists(Application.dataPath + "/save.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.savefile");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            CurrentSceneID = file.sceneID;
            return CurrentSceneID;
        }
        return 0;
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

    public void SaveShopItems((ShopItem, bool)[] items)
    {
        ShopItemsFile file = new ShopItemsFile();
        file.shopItems = items;
        _serializer.TrySerialize(file, out fsData data);
        File.WriteAllText(Application.dataPath + "/shopItems.savefile", data.ToString());
    }

    public ShopItemsFile LoadShopItems()
    {
        if (File.Exists(Application.dataPath + "/shopItems.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/shopItems.savefile");
            fsData data = fsJsonParser.Parse(json);
            ShopItemsFile file = null;
            _serializer.TryDeserialize(data, ref file);
            return file;
        }
        return null;
    }
}