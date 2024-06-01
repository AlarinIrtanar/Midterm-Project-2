using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool isPaused;

    [Header("----- Components -----")]
    [SerializeField] TMP_Text scoreText;
    public static MenuManager instance;
    [SerializeField] TMP_Text loseText;

    [Header("----- Sliders -----")]
    [SerializeField] Slider sensiSlider;
    [SerializeField] Slider speedSlider;

    [Header("----- Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuOptions;
    [SerializeField] string mainMenuName;
    [SerializeField] string loadingScreenName;

    [Header("----- Audio -----")]
    //[SerializeField] AudioClip buttonPressSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioMixer mixer;

    [Header("----- Buttons -----")]
    [SerializeField] Button pauseResume;
    [SerializeField] Button loseRestart;
    [SerializeField] Button winNextLevel;
    [SerializeField] Button optionsApply;

    string pauseButton;

    // Start is called before the first frame update
    void Awake()
    {

        instance = this;

        // Pause Button
        if (PlayerPrefs.HasKey("Pause Button"))
            pauseButton = PlayerPrefs.GetString("Pause Button");
        else
            pauseButton = "escape";

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

        // Sensitivity
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensiSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            PlayerPrefs.SetFloat("Sensitivity", sensiSlider.value);
        }

        // Game Speed
        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            speedSlider.value = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("GameSpeed", speedSlider.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseButton))
        {
            if (isPaused && menuActive == menuPause)
            {
                PressCancel();
                menuOptions.SetActive(false);
                Unpause();
            }
            else if (!isPaused && menuActive == null)
            {
                Pause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
                pauseResume.Select();
            }
        }
    }

    public void PressResume()
    {
        audioSource.Play();
        Unpause();
        
    }
    public void PressLevelSelect()
    {
        audioSource.Play();
        PlayerPrefs.SetInt("NextLevel", 0);

        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            Time.timeScale = 1;
        }

        SceneManager.LoadScene(mainMenuName);
    }
    public void PressNextLevel()
    {
        if (PlayerPrefs.HasKey("SelectedWorld") && PlayerPrefs.HasKey("SelectedLevel"))
        {
            int world = PlayerPrefs.GetInt("SelectedWorld");
            int level = PlayerPrefs.GetInt("SelectedLevel");

            FileManager.instance.UnlockLevel(world, level);

            PlayerPrefs.SetInt("SelectedWorld", world);
            PlayerPrefs.SetInt("SelectedLevel", level + 1) ;
        }

        audioSource.Play();
        PlayerPrefs.SetInt("NextLevel", 1);

        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            Time.timeScale = 1;
        }
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("AllLevelsCompleted", 1);
            PlayerPrefs.SetInt("NextLevel", 0);
            SceneManager.LoadScene(mainMenuName);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    public void PressRestart()
    {
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Unpause();
    }
    public void PressQuit()
    {
        audioSource.Play();

        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            Time.timeScale = 1;
        }

        SceneManager.LoadScene(mainMenuName);
    }
    public void PressApply()
    {
        float temp;
        mixer.GetFloat("MasterVolume", out temp);
        PlayerPrefs.SetFloat("MasterVolume", temp);

        mixer.GetFloat("MusicVolume", out temp);
        PlayerPrefs.SetFloat("MusicVolume", temp);

        mixer.GetFloat("SFXVolume", out temp);
        PlayerPrefs.SetFloat("SFXVolume", temp);

        PlayerPrefs.SetFloat("Sensitivity", sensiSlider.value);

        PlayerPrefs.SetFloat("GameSpeed", speedSlider.value);
        pauseResume.Select();
    }
    public void PressCancel()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }

        // Sensitivity
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensiSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        // Game Speed
        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            speedSlider.value = PlayerPrefs.GetFloat("GameSpeed");
        }
        pauseResume.Select();
    }

    void Unpause()
    {
        if (HUDManager.instance != null)
        {
            HUDManager.instance.reticle.gameObject.SetActive(true);
        }
        audioSource.Play();

        if (PlayerPrefs.HasKey("GameSpeed"))
        {
            Time.timeScale = PlayerPrefs.GetFloat("GameSpeed");
        }
        else
        {
            Time.timeScale = 1;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = !isPaused;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }
    void Pause()
    {
        if(HUDManager.instance != null)
        {
            HUDManager.instance.reticle.gameObject.SetActive(false);
        }
        audioSource.Play();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isPaused = !isPaused;
    }
    public void ShowWin()
    {
        Pause();

        if (GameManager.Instance != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        }

        menuActive = menuWin;
        menuActive.SetActive(isPaused);

        winNextLevel.Select();
    }
    public void ShowLose()
    {
        Pause();

        menuActive = menuLose;
        menuActive.SetActive(isPaused);

        loseText.text = "RIP";

        loseRestart.Select();
    }
    public void ShowLose(string deathItemName)
    {
        Pause();

        menuActive = menuLose;
        menuActive.SetActive(isPaused);

        loseText.text = "Killed By: " + deathItemName;

        loseRestart.Select();
    }
    public void PressOptions()
    {
        optionsApply.Select();
    }
}
