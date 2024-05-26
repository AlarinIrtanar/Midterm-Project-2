using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Button levelSelect;
    [SerializeField] Button btnNewGameNo;
    [SerializeField] GameObject mainMenu;
    [SerializeField] LevelSelectController levelSelectMenu;
    [SerializeField] GameObject newGameMenu;

    [Header("Audio")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource buttonAud;

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

        levelSelect.Select();

        mainMenuActive = true;
        mainMenuActiveLoc = mainMenuInactiveLoc = mainMenu.transform.position;

        mainMenuInactiveLoc.x -= Screen.width;


        newGameMenuActive = false;
        newGameMenuActiveLoc = newGameMenuInactiveLoc = newGameMenu.transform.position;

        newGameMenuInactiveLoc.x += Screen.width;
        newGameMenu.transform.position = newGameMenuInactiveLoc;

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
            if (Mathf.Abs(mainMenu.transform.position.x - mainMenuActiveLoc.x) < 0.1f)
            {
                mainMenu.transform.position = mainMenuActiveLoc;
            }
        }
        else
        {
            mainMenu.transform.position = Vector3.Lerp(mainMenu.transform.position, mainMenuInactiveLoc, Time.deltaTime * 5);
            if (Mathf.Abs(mainMenu.transform.position.x - mainMenuInactiveLoc.x) < 0.1f)
            {
                mainMenu.transform.position = mainMenuInactiveLoc;
                if (mainMenu.activeSelf == true)
                {
                    mainMenu.SetActive(false);
                }
            }
        }

        if (newGameMenuActive)
        {
            newGameMenu.transform.position = Vector3.Lerp(newGameMenu.transform.position, newGameMenuActiveLoc, Time.deltaTime * 5);
            if (Mathf.Abs(newGameMenu.transform.position.x - newGameMenuActiveLoc.x) < 0.1f)
            {
                newGameMenu.transform.position = newGameMenuActiveLoc;
            }
        }
        else
        {
            newGameMenu.transform.position = Vector3.Lerp(newGameMenu.transform.position, newGameMenuInactiveLoc, Time.deltaTime * 5);
            if (Mathf.Abs(newGameMenu.transform.position.y - newGameMenuInactiveLoc.y) < 0.1f)
            {
                newGameMenu.transform.position = newGameMenuInactiveLoc;
                if (newGameMenu.activeSelf == true)
                {
                    newGameMenu.SetActive(false);
                }
            }
        }
    }
    public void PressNewGame()
    {
        if (FileManager.instance != null)
        {
            PlayerPrefs.SetInt("AllLevelsCompleted", 0);
            FileManager.instance.DeleteWorldUnlocks();
            FileManager.instance.ResetUnlocks();

            //levelSelectMenu.Awake();

            //levelSelectMenu.gameObject.SetActive(false);
            //levelSelectMenu.gameObject.SetActive(true);
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

    public void ButtonPress()
    {
        buttonAud.Play();
    }
    public void ToggleMainMenuActive()
    {
        mainMenuActive = !mainMenuActive;
        if (mainMenuActive)
        {
            mainMenu.SetActive(true);
            levelSelect.Select();
        }
    }
    public void ToggleNewGameMenuActive()
    {
        newGameMenuActive = !newGameMenuActive;
        if (newGameMenuActive)
        {
            newGameMenu.SetActive(true);
            btnNewGameNo.Select();
        }
    }
}
