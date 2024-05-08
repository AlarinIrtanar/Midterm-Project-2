using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectController : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] List<SelectableLevels> levelsList;
    [SerializeField] List<GameObject> levelObjects;

    int levelSelectCount;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var level in levelsList)
        {
            levelObjects.Add(Instantiate(level.levelModel, new Vector3(-1000,-1000,-1000), new Quaternion(5,5,0,30)));
        }
    }

    private void OnEnable()
    {
        levelObjects[levelSelectCount].transform.position = new Vector3(0, 0, 0);
    }
    public void OnDisable()
    {
        foreach (var item in levelObjects)
        {
            item.transform.position = new Vector3(-1000, -1000, -1000);
        }
    }

    public void PressNext()
    {
        if(levelSelectCount < levelsList.Count - 1)
        {
            levelObjects[levelSelectCount].transform.position = new Vector3(-1000, -1000, -1000);
            levelSelectCount++;
            levelObjects[levelSelectCount].transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            levelObjects[levelSelectCount].transform.position = new Vector3(-1000, -1000, -1000);
            levelSelectCount = 0;
            levelObjects[levelSelectCount].transform.position = new Vector3(0, 0, 0);
        }
    }
    public void PressPrev()
    {
        if (levelSelectCount > 0)
        {
            levelObjects[levelSelectCount].transform.position = new Vector3(-1000, -1000, -1000);
            levelSelectCount--;
            levelObjects[levelSelectCount].transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            levelObjects[levelSelectCount].transform.position = new Vector3(-1000, -1000, -1000);
            levelSelectCount = levelsList.Count - 1;
            levelObjects[levelSelectCount].transform.position = new Vector3(0, 0, 0);
        }
    }
}
