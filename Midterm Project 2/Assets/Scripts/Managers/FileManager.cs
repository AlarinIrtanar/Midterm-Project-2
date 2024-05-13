using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static FileManager instance;
    string filePath;
    string optionsPath;
    public List<bool> levelUnlocks;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        filePath = Application.persistentDataPath;
        optionsPath = filePath + "/options.dat";
    }


    public List<bool> GetLevelUnlocks()
    {
        return levelUnlocks;
    }
    public void SetLevelUnlocks(List<bool> levels)
    {
        levelUnlocks = levels;
    }
    public void SaveLevelUnlocks(string fileName)
    {
        BinaryFormatter bout = new BinaryFormatter();
        FileStream fout = File.Open(filePath + fileName, FileMode.OpenOrCreate);

        bout.Serialize(fout, levelUnlocks);
        fout.Close();
    }
    public void LoadLevelUnlocks(string fileName)
    {
        if (File.Exists(filePath + fileName))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream fin = File.Open(filePath + fileName, FileMode.Open);

            levelUnlocks = (List<bool>)bin.Deserialize(fin);
            fin.Close();
        }
    }
    public void SaveOptions()
    {
        BinaryFormatter bout = new BinaryFormatter();
        FileStream fout = File.Open(optionsPath, FileMode.OpenOrCreate);

        fout.Close();
    }
    public void LoadOptions()
    {
        if (File.Exists(optionsPath))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream fin = File.Open(optionsPath, FileMode.Open);

            fin.Close();
        }
        else
        {
            SaveOptions();
        }
    }


    public void SaveWorldUnlocks(string fileName)
    {
        BinaryFormatter bout = new BinaryFormatter();
        FileStream fout = File.Open(filePath + fileName, FileMode.OpenOrCreate);

        fout.Close();
    }
    public void LoadWorldUnlocks(string fileName)
    {
        if (File.Exists(filePath + fileName))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream fin = File.Open(filePath + fileName, FileMode.Open);

            fin.Close();
        }
        else
        {
            SaveWorldUnlocks(fileName);
        }
    }

}
