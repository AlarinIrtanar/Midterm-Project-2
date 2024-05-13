using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public static MenuManager instance;
    [SerializeField] TMP_Text scoreText;

    [Header("----- Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] string mainMenuName;

    [Header("----- Audio -----")]
    //[SerializeField] AudioClip buttonPressSound;
    [SerializeField] AudioSource audioSource;

    bool isPaused;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (isPaused && menuActive == menuPause)
            {
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
        Unpause(); // Temp

        if (PlayerPrefs.HasKey("SelectedWorld") && PlayerPrefs.HasKey("SelectedLevel"))
        {
            string world = PlayerPrefs.GetString("SelectedWorld");
            int level = PlayerPrefs.GetInt("SelectedLevel");
            FileManager.instance.LoadLevelUnlocks(world);
            List<bool> unlocks = FileManager.instance.GetLevelUnlocks();
            if (unlocks.Count > level)
            {
                unlocks[level] = true;
                FileManager.instance.SetLevelUnlocks(unlocks);
                FileManager.instance.SaveLevelUnlocks(world);
            }
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
        SceneManager.LoadScene(mainMenuName);
    }

    void Unpause()
    {
        if (HUDManager.instance != null)
        {
            HUDManager.instance.reticle.gameObject.SetActive(false);
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
