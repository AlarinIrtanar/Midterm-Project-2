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

    // Start is called before the first frame update
    void Awake()
    {
        newWorldsList = new List<SelectableWorlds>();
        for (int i = 0; i < worldsList.Count; ++i)
        {
            currentWorld = Instantiate(worldsList[i], this.transform);
            newWorldsList.Add(currentWorld);
            //Debug.Log("Disabling New World " + (i + 1).ToString());
            newWorldsList[i].SetEnabled(false);
            //Debug.Log("New World " + (i + 1).ToString() + " Disabled");
        }
    }

    private void OnEnable()
    {
        //Debug.Log("Enabling World 1");
        newWorldsList[worldSelectCount].SetEnabled(true);
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
        for (int i = 0; i < worldsList.Count; ++i)
        {
            newWorldsList[i].SaveLevelUnlocks();
        }
    }
    public void PressNext()
    {
        if(worldSelectCount < worldsList.Count - 1)
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
    }
    public void PressPrev()
    {
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
    }
}
