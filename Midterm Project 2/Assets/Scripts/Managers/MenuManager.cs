using System.Collections.Generic;
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
    [SerializeField] Slider sensiSlider;

    [Header("----- Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuOptions;
    [SerializeField] string mainMenuName;

    [Header("----- Audio -----")]
    //[SerializeField] AudioClip buttonPressSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioMixer mixer;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        float temp;
        mixer.GetFloat("MasterVolume", out temp);
        PlayerPrefs.SetFloat("MasterVolume", temp);

        mixer.GetFloat("MusicVolume", out temp);
        PlayerPrefs.SetFloat("MusicVolume", temp);

        mixer.GetFloat("SFXVolume", out temp);
        PlayerPrefs.SetFloat("SFXVolume", temp);

        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensiSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
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
            }
        }
    }

    public void PressResume()
    {
        audioSource.Play();
        Unpause();
        
    }
    public void PressNextLevel()
    {
        audioSource.Play();
        Time.timeScale = 1;

        SceneManager.LoadScene(mainMenuName); // temp
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
        Time.timeScale = 1;
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
    }
    public void PressCancel()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume") / 20);
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") /20);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume") / 20);
        }
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensiSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
    }

    void Unpause()
    {
        if (HUDManager.instance != null)
        {
            HUDManager.instance.reticle.gameObject.SetActive(true);
        }
        audioSource.Play();
        Time.timeScale = 1;
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
    }
    public void ShowLose()
    {
        Pause();

        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }
}
