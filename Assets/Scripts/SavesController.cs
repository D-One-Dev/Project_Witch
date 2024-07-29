using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class SavesController : MonoBehaviour
{
    [SerializeField] private GameObject player;
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
    }

    public void Save()
    {
        SaveFile file = new SaveFile();
        file.playerPosition = player.transform.position;
        file.playerRotation = player.transform.localRotation;
        string json = JsonUtility.ToJson(file);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string json = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveFile file = JsonUtility.FromJson<SaveFile>(json);
            player.transform.position = file.playerPosition;
            player.transform.localRotation = file.playerRotation;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
