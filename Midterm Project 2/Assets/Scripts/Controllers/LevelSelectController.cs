using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject winScreen;
    public Button nextWorld;
    [SerializeField] GameObject levelMenu;
    [SerializeField] Button btnMainMenu;
    [SerializeField] TMP_Text worldName;

    bool levelMenuActive;
    bool notFirstTime;
    public bool inPlace;

    Vector3 levelMenuActiveLoc;
    Vector3 levelMenuInactiveLoc;

    [Header("Levels")]
    [SerializeField] List<SelectableWorlds> worldsList;

    int worldSelectCount;
    SelectableWorlds currentWorld;
    List<SelectableWorlds> newWorldsList;


    [Header("Audio")]
    [SerializeField] AudioSource buttonAud;
    [SerializeField] AudioSource pageAud;


    // Start is called before the first frame update
    public void Start()
    {
        int lastPlayedWorld = 0;
        Time.timeScale = 1;
        notFirstTime = false;
        levelMenuActive = false;
        levelMenuActiveLoc = levelMenuInactiveLoc = levelMenu.transform.position;

        levelMenuInactiveLoc.y -= Screen.height;
        levelMenu.transform.position = levelMenuInactiveLoc;

        if (PlayerPrefs.HasKey("LevelUnlock") && PlayerPrefs.GetInt("LevelUnlock") == 1)
        {
            levelMenu.transform.position = levelMenuActiveLoc;
        }

        newWorldsList = new List<SelectableWorlds>();
        if (worldsList.Count == FileManager.instance.worlds.Count)
        {

            for (int i = 0; i < worldsList.Count; ++i)
            {
                currentWorld = Instantiate(worldsList[i], this.gameObject.transform);
                newWorldsList.Add(currentWorld);
                newWorldsList[i].SetEnabled(false);
            }
        }
        else
        {

            FileManager.instance.ClearWorlds();
            for (int i = 0; i < worldsList.Count; ++i)
            {
                currentWorld = Instantiate(worldsList[i], this.gameObject.transform);
                newWorldsList.Add(currentWorld);
                newWorldsList[i].SetEnabled(false);

                FileManager.instance.AddWorld(newWorldsList[i].levelName.Count);
            }
            //newWorldsList[0].levelImages[0].GetComponent<LevelInfoController>().SetUnlocked(true);
            FileManager.instance.UnlockLevel(0, 0);
            FileManager.instance.SaveWorldUnlocks();
        }
        FileManager.instance.LoadWorldUnlocks();
        if (PlayerPrefs.HasKey("AllLevelsCompleted") && PlayerPrefs.GetInt("AllLevelsCompleted") == 1)
        {
            winScreen.SetActive(true);
        }
        if (PlayerPrefs.HasKey("SelectedWorld"))
        {
            worldSelectCount = PlayerPrefs.GetInt("SelectedWorld");
            lastPlayedWorld = worldSelectCount;
        }
        else
        {
            worldSelectCount = 0;
        }
        newWorldsList[worldSelectCount].SetEnabled(true);
        worldName.text = newWorldsList[worldSelectCount].worldName;
        notFirstTime = true;
        OnEnable();

        if (PlayerPrefs.HasKey("LevelUnlock") && PlayerPrefs.GetInt("LevelUnlock") == 1)
        {
            ToggleLevelMenuActive();

            int world = lastPlayedWorld;

            PlayerPrefs.SetInt("SelectedWorld", world);
            SetActiveWorld(world);

            levelMenu.transform.position = levelMenuActiveLoc;
            StartCoroutine(NextLevel0());
        }
        else
        {
            SetActiveWorld(lastPlayedWorld);
        }
        
        if (!winScreen.activeSelf && PlayerPrefs.HasKey("NextLevel") && PlayerPrefs.GetInt("NextLevel") == 1 && PlayerPrefs.HasKey("SelectedLevel"))
        {
            int world = lastPlayedWorld;
            int level = PlayerPrefs.GetInt("SelectedLevel");

            if (world < newWorldsList.Count - 1 && level >= newWorldsList[world].levelImages.Count)
            {
                world++;
                level = 0;
            }

            PlayerPrefs.SetInt("SelectedWorld", world);
            SetActiveWorld(world);

            PlayerPrefs.SetInt("LevelUnlock", 0);
            PlayerPrefs.SetInt("NextLevel", 0);
            
            if (world < newWorldsList.Count && level < newWorldsList[world].levelImages.Count)
            {
                newWorldsList[world].levelImages[level].GetComponent<LevelInfoController>().levelSelected();
            }
        }
        if (PlayerPrefs.HasKey("SelectedWorld"))
        {
            worldSelectCount = PlayerPrefs.GetInt("SelectedWorld");
        }
        else
        {
            worldSelectCount = 0;
        }
    }
    IEnumerator NextLevel0()
    {
        yield return new WaitForEndOfFrame();
        PlayerPrefs.SetInt("LevelUnlock", 0);
    }
    private void Update()
    {
        if (levelMenuActive)
        {
            levelMenu.transform.position = Vector3.Lerp(levelMenu.transform.position, levelMenuActiveLoc, Time.deltaTime * 5);
            if (Mathf.Abs(levelMenu.transform.position.y - levelMenuActiveLoc.y) < 2f)
            {
                levelMenu.transform.position = levelMenuActiveLoc;
                if (inPlace == false)
                {
                    inPlace = true;
                    //newWorldsList[0].levelImages[0].GetComponent<LevelInfoController>().SetUnlocked(true);
                    //FileManager.instance.UnlockLevel(0, 0);
                }
            }
        }
        else
        {
            levelMenu.transform.position = Vector3.Lerp(levelMenu.transform.position, levelMenuInactiveLoc, Time.deltaTime * 5);
            if (Mathf.Abs(levelMenu.transform.position.y - levelMenuInactiveLoc.y) < 0.1f)
            {
                levelMenu.transform.position = levelMenuInactiveLoc;
                if(this.gameObject.activeSelf == true)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
    private void OnEnable()
    {
        if (notFirstTime)
        {
;
            if (PlayerPrefs.HasKey("SelectedWorld"))
            {
                worldSelectCount = PlayerPrefs.GetInt("SelectedWorld");
            }
            else
            {
                worldSelectCount = 0;
            }
            SetActiveWorld(worldSelectCount);
            worldName.text = newWorldsList[worldSelectCount].worldName;
            //PlayerPrefs.SetInt("SelectedWorld", worldSelectCount);
            FileManager.instance.LoadWorldUnlocks();
            FileManager.instance.UnlockLevel(0, 0);

            for (int i = 0; i < newWorldsList.Count; i++)
            {
                for (int j = 0; j < newWorldsList[i].levelImages.Count; j++)
                {
                    newWorldsList[i].levelImages[j].GetComponent<LevelInfoController>().SetUnlocked(FileManager.instance.GetUnlock(i, j));
                }
            }
            FileManager.instance.SaveWorldUnlocks();
        }



    }
    public void OnDisable()
    {
        for (int i = 0; i < worldsList.Count; ++i)
        {

            newWorldsList[i].SetEnabled(false);

        }
    }
    public void PressMainMenu()
    {
        buttonAud.Play();
        FileManager.instance.SaveWorldUnlocks();
    }
    public void PressNext()
    {
        pageAud.Play();
        if (worldSelectCount < worldsList.Count - 1)
        {
            worldSelectCount++;
        }
        else
        {
            worldSelectCount = 0;
        }
        SetActiveWorld(worldSelectCount);
    }
    public void PressPrev()
    {
        pageAud.Play();
        if (worldSelectCount > 0)
        {
            worldSelectCount--;
        }
        else
        {
            worldSelectCount = worldsList.Count - 1;
        }
        SetActiveWorld(worldSelectCount);
    }
    void SetActiveWorld(int worldID)
    {
        for (int world = 0; world < newWorldsList.Count; world++)
        {
            newWorldsList[world].SetEnabled(false);
        }
        newWorldsList[worldID].SetEnabled(true);

        worldName.text = newWorldsList[worldSelectCount].worldName;
        for (int level = 0; level < newWorldsList[worldSelectCount].levelImages.Count; level++)
        {
            newWorldsList[worldSelectCount].levelImages[level].GetComponent<LevelInfoController>().SetUnlocked(FileManager.instance.GetUnlock(worldSelectCount, level));
        }
    }
    public void PressContinue()
    {
        if (PlayerPrefs.HasKey("AllLevelsCompleted"))
        {
            winScreen.SetActive(false);
            PlayerPrefs.SetInt("AllLevelsCompleted", 0);
        }
    }
    public void PlayButtonPress()
    {
        buttonAud.Play();
    }

    public void ToggleLevelMenuActive()
    {


        levelMenuActive = !levelMenuActive;

        if (PlayerPrefs.HasKey("SelectedWorld"))
        {
            worldSelectCount = PlayerPrefs.GetInt("SelectedWorld");
        }
        else
        {
            worldSelectCount = 0;
        }

        if (levelMenuActive)
        {
            this.gameObject.SetActive(true);

            for (int level = 0; level < newWorldsList[worldSelectCount].levelImages.Count; level++)
            {

                newWorldsList[worldSelectCount].levelImages[level].GetComponent<LevelInfoController>().SetUnlocked(FileManager.instance.GetUnlock(worldSelectCount, level));

            }
            btnMainMenu.Select();
        }
    }
}
