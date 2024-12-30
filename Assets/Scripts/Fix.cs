using System.IO;
using UnityEngine;

public class Fix : MonoBehaviour
{
    private void Awake()
    {
        if(File.Exists(Application.dataPath + "/boughtSpells.savefile")) File.Delete(Application.dataPath + "/boughtSpells.savefile");
        if (File.Exists(Application.dataPath + "/boughtSpells.savefile.meta"))File.Delete(Application.dataPath + "/boughtSpells.savefile.meta");
    }
}
