using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button levelSelect;
    [SerializeField] AudioMixer mixer;
    [SerializeField] GameObject mainMenu;
    [SerializeField] LevelSelectController levelSelectMenu;
    [SerializeField] GameObject newGameMenu;

    bool mainMenuActive;
    bool newGameMenuActive;

    Vector3 mainMenuActiveLoc;
    Vector3 mainMenuInactiveLoc;

    Vector3 newGameMenuActiveLoc;
    Vector3 newGameMenuInactiveLoc;
    public void Start()
    {
        FileManager.instance.LoadOptions();
        FileManager.instance.LoadWorldUnlocks();

        mainMenuActive = true;
        mainMenuActiveLoc = mainMenuInactiveLoc = mainMenu.transform.position;

        mainMenuInactiveLoc.x -= 1000;


        newGameMenuActive = false;
        newGameMenuActiveLoc = newGameMenuInactiveLoc = newGameMenu.transform.position;

        newGameMenuActiveLoc.x -= 1500;

        float temp;
        // Master Volume
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            mixer.GetFloat("MasterVolume", out temp);
            PlayerPrefs.SetFloat("MasterVolume", temp);
        }

        // Music Volume
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            mixer.GetFloat("MusicVolume", out temp);
            PlayerPrefs.SetFloat("MusicVolume", temp);
        }

        // SFX Volume
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
        else
        {
            mixer.GetFloat("SFXVolume", out temp);
            PlayerPrefs.SetFloat("SFXVolume", temp);
        }



        // Shoot Button
        if (!PlayerPrefs.HasKey("Shoot Button"))
        {
            PlayerPrefs.SetString("Shoot Button", "mouse 0");
        }

        // Grapple Button
        if (!PlayerPrefs.HasKey("Grapple Button"))
        {
            PlayerPrefs.SetString("Grapple Button", "mouse 1");
        }

        // Crouch Button
        if (!PlayerPrefs.HasKey("Crouch Button"))
        {
            PlayerPrefs.SetString("Crouch Button", "left ctrl");
        }

        // Sprint Button
        if (!PlayerPrefs.HasKey("Sprint Button"))
        {
            PlayerPrefs.SetString("Sprint Button", "left shift");
        }

        // Jump Button
        if (!PlayerPrefs.HasKey("Jump Button"))
        {
            PlayerPrefs.SetString("Jump Button", "space");
        }

        // Sensitivity
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 1f);
        }

        // Game Speed
        if (!PlayerPrefs.HasKey("GameSpeed"))
        {
            PlayerPrefs.SetFloat("GameSpeed", 1f);
        }

        if (PlayerPrefs.HasKey("NextLevel") && PlayerPrefs.GetInt("NextLevel") == 1)
        {
            mainMenu.transform.position = mainMenuInactiveLoc;
            levelSelect.onClick.Invoke();
            PlayerPrefs.SetInt("NextLevel", 0);
        }

    }

    void Update()
    {
        if (mainMenuActive)
        {
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, mainMenuActiveLoc, Time.deltaTime * 5);
        }
        else
        {
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, mainMenuInactiveLoc, Time.deltaTime * 5);
        }

        if (newGameMenuActive)
        {
            newGameMenu.transform.position = Vector3.Lerp(newGameMenu.transform.position, newGameMenuActiveLoc, Time.deltaTime * 5);
        }
        else
        {
            newGameMenu.transform.position = Vector3.Lerp(newGameMenu.transform.position, newGameMenuInactiveLoc, Time.deltaTime * 5);
        }
    }
    public void PressNewGame()
    {
        if (FileManager.instance != null)
        {
            PlayerPrefs.SetInt("AllLevelsCompleted", 0);
            FileManager.instance.DeleteWorldUnlocks();
            FileManager.instance.ClearWorlds();

            levelSelectMenu.Awake();

            levelSelectMenu.gameObject.SetActive(false);
            levelSelectMenu.gameObject.SetActive(true);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void PressQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ToggleMainMenuActive()
    {
        mainMenuActive = !mainMenuActive;
    }
    public void ToggleNewGameMenuActive()
    {
        newGameMenuActive = !newGameMenuActive;
    }
}
