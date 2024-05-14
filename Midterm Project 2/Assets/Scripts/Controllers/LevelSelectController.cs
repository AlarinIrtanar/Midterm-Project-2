using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] List<SelectableWorlds> worldsList;

    int worldSelectCount;
    SelectableWorlds currentWorld;
    List<SelectableWorlds> newWorldsList;


    [Header("Audio")]
    [SerializeField] AudioClip buttonPress;
    [SerializeField] AudioClip buttonPressNextPage;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        newWorldsList = new List<SelectableWorlds>();
        if (worldsList.Count == FileManager.instance.worlds.Count)
        {
            for (int i = 0; i < worldsList.Count; ++i)
            {
                currentWorld = Instantiate(worldsList[i], this.transform);
                newWorldsList.Add(currentWorld);
                newWorldsList[i].SetEnabled(false);
            }
        }
        else
        {
            FileManager.instance.ClearWorlds();
            for (int i = 0; i < worldsList.Count; ++i)
            {
                currentWorld = Instantiate(worldsList[i], this.transform);
                newWorldsList.Add(currentWorld);
                newWorldsList[i].SetEnabled(false);

                FileManager.instance.AddWorld(newWorldsList[i].levelName.Count);
            }
            newWorldsList[0].levelImages[0].GetComponent<LevelInfoController>().SetUnlocked(true);
            FileManager.instance.UnlockLevel(0, 0);
            FileManager.instance.SaveWorldUnlocks();
        }
        FileManager.instance.LoadWorldUnlocks();
    }

    private void OnEnable()
    {
        //Debug.Log("Enabling World 1");
        newWorldsList[worldSelectCount].SetEnabled(true);
        FileManager.instance.LoadWorldUnlocks();

        for (int i = 0; i < newWorldsList.Count; i++) 
        {
            for (int j = 0; j < newWorldsList[i].levelImages.Count; j++) 
            {
                newWorldsList[i].levelImages[j].GetComponent<LevelInfoController>().SetUnlocked(FileManager.instance.GetUnlock(i, j));
            }
        }

        //Debug.Log("World 1 Enabled");
    }
    public void OnDisable()
    {
        for (int i = 0; i < worldsList.Count; ++i)
        {
            //Debug.Log("Disabling World " + (i + 1).ToString());
            newWorldsList[i].SetEnabled(false);
            //Debug.Log("World " + i.ToString() + " Disabled");
        }
    }
    public void PressMainMenu()
    {
        audioSource.PlayOneShot(buttonPress);
        FileManager.instance.SaveWorldUnlocks();
    }
    public void PressNext()
    {
        audioSource.PlayOneShot(buttonPressNextPage);
        if (worldSelectCount < worldsList.Count - 1)
        {
            newWorldsList[worldSelectCount].SetEnabled(false);
            worldSelectCount++;
            newWorldsList[worldSelectCount].SetEnabled(true);
        }
        else
        {
            newWorldsList[worldSelectCount].SetEnabled(false);
            worldSelectCount = 0;
            newWorldsList[worldSelectCount].SetEnabled(true);
        }
        PlayerPrefs.SetInt("SelectedWorld", worldSelectCount);
    }
    public void PressPrev()
    {
        audioSource.PlayOneShot(buttonPressNextPage);
        if (worldSelectCount > 0)
        {
            newWorldsList[worldSelectCount].SetEnabled(false);
            worldSelectCount--;
            newWorldsList[worldSelectCount].SetEnabled(true);
        }
        else
        {
            newWorldsList[worldSelectCount].SetEnabled(false);
            worldSelectCount = worldsList.Count - 1;
            newWorldsList[worldSelectCount].SetEnabled(true);
        }
        PlayerPrefs.SetInt("SelectedWorld", worldSelectCount);
    }

    public void PlayButtonPress()
    {
        audioSource.PlayOneShot(buttonPress);
    }

}
