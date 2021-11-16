using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveManager saveManager;
    public string path;

    private void Awake()
    {
        //Saves saves in C:\Users\atom5\AppData\LocalLow\MBK\Light\Saves
        path = Application.persistentDataPath + "/Saves";
    }

    public bool Save(PlayerData saveData)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream file = new FileStream(path + "/Save.dat", FileMode.Create);
        binaryFormatter.Serialize(file, saveData);
        file.Close();

        return true;
    }

    public PlayerData Load()
    {
        if (!File.Exists(path + "/Save.dat"))
        {
            return null;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream file = new FileStream(path + "/Save.dat", FileMode.Open);

        PlayerData playerData = binaryFormatter.Deserialize(file) as PlayerData;
        file.Close();
        return playerData;
    }
}
