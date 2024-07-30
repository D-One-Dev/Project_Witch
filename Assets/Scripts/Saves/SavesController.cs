using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SavesController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image savingIconImage;
    public static SavesController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Load();
    }
    private class SaveFile
    {
        public Vector3 playerPosition;
        public quaternion playerRotation;
        public Spell leftSpell, rightSpell;
        public Effect leftEffect, rightEffect;
    }

    public void Save()
    {
        AnimationsController.instance.ImageInOutFade(savingIconImage);
        SaveFile file = new SaveFile();
        file.playerPosition = player.transform.position;
        file.playerRotation = player.transform.localRotation;
        file.leftSpell = NewSpellCaster.instance.leftSpell;
        file.rightSpell = NewSpellCaster.instance.rightSpell;
        file.leftEffect = NewSpellCaster.instance.leftEffect;
        file.rightEffect = NewSpellCaster.instance.rightEffect;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.savefile", json);
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/save.savefile"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.savefile");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = file.playerPosition;
            player.transform.localRotation = file.playerRotation;
            player.GetComponent<CharacterController>().enabled = true;
            NewSpellCaster.instance.leftSpell = file.leftSpell;
            NewSpellCaster.instance.rightSpell = file.rightSpell;
            NewSpellCaster.instance.leftEffect = file.leftEffect;
            NewSpellCaster.instance.rightEffect = file.rightEffect;
        }
    }
}
