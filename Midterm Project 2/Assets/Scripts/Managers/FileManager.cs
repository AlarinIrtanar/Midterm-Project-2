using System;
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

    public List<World> worlds;
    public string worldUnlocksFileName;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        filePath = Application.persistentDataPath;
        optionsPath = filePath + "/options.dat";

        worlds = new List<World>();
        worldUnlocksFileName = ("/WorldUnlocks.dat");
        LoadWorldUnlocks();
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
    public void ClearWorlds()
    {
        worlds.Clear();
    }
    public void AddWorld(int levelCount)
    {
        World tempWorld = new World();
        tempWorld.levels = new List<Level>();

        for (int i = 0; i < levelCount; i++)
        {
            Level tempLevel = new Level();
            tempLevel.levelId = i;
            tempWorld.levels.Add(tempLevel);
        }
        worlds.Add(tempWorld);
        SaveWorldUnlocks();
    }
    public void UnlockLevel(int worldId, int levelId)
    {
        LoadWorldUnlocks();
/*        Debug.Log("World: " + worldId);
        Debug.Log("Level: " + levelId);
        Debug.Log("World Count: " + worlds.Count);
        Debug.Log("Level Count: " + worlds[worldId].levels.Count);*/
        if (levelId < worlds[worldId].levels.Count)
        {
            worlds[worldId].levels[levelId].isUnlocked = true;
        }
        else if(worldId < worlds.Count - 1)
        {
            worlds[worldId + 1].levels[0].isUnlocked = true;
        }
        else
        {
            PlayerPrefs.SetInt("AllLevelsCompleted", 1);
        }
        SaveWorldUnlocks();

    }
    public bool GetUnlock(int worldId, int levelId)
    {
        LoadWorldUnlocks();
/*        Debug.Log("World: " + worldId);
        Debug.Log("Level: " + levelId);
        Debug.Log("World Count: " + worlds.Count);
        Debug.Log("Level Count: " + worlds[worldId].levels.Count);*/

        if (worlds[worldId].levels[levelId] != null)
        {
            return worlds[worldId].levels[levelId].isUnlocked;
        }
        else
        {
            return false;
        }
    }

    public void SaveWorldUnlocks()
    {
        BinaryFormatter bout = new BinaryFormatter();
        FileStream fout = File.Open(filePath + worldUnlocksFileName, FileMode.OpenOrCreate);

        bout.Serialize(fout, worlds);

        fout.Close();
    }
    public void LoadWorldUnlocks()
    {
        if (File.Exists(filePath + worldUnlocksFileName))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream fin = File.Open(filePath + worldUnlocksFileName, FileMode.Open);

            worlds = (List<World>)bin.Deserialize(fin);
            fin.Close();
        }
        else
        {
            SaveWorldUnlocks();
        }
    }
    public void DeleteWorldUnlocks()
    {
        File.Delete(filePath + worldUnlocksFileName);
    }

}
[Serializable]
public class World
{
    public int worldId;
    public List<Level> levels;
}
[Serializable]
public class Level
{
    public int levelId;
    public bool isUnlocked;
}
