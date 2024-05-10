using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public static MenuManager instance;
    //[SerializeField] Player player;

    [Header("----- Menus -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] string mainMenuName;

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
        Unpause();
    }
    public void PressNextLevel()
    {
        Unpause(); // Temp
    }
    public void PressRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Unpause();
    }
    public void PressQuit()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    void Unpause()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = !isPaused;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }
    void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isPaused = !isPaused;
    }














}
