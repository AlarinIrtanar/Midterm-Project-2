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
    public void Start()
    {
        FileManager.instance.LoadOptions();
        FileManager.instance.LoadWorldUnlocks();

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

        if (PlayerPrefs.HasKey("NextLevel"))
        {
            if(PlayerPrefs.GetInt("NextLevel") == 1)
            {
                levelSelect.onClick.Invoke();
                PlayerPrefs.SetInt("NextLevel", 0);
            }
        }

    }
    
    public void PressNewGame()
    {
        if (FileManager.instance != null)
        {
            PlayerPrefs.SetInt("AllLevelsCompleted", 0);
            FileManager.instance.DeleteWorldUnlocks();
            FileManager.instance.ClearWorlds();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void PressCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void PressQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
