using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public float timer = 120f;
    [SerializeField] GameObject PlayerPreFab;
    [SerializeField] GameObject playerSpawnPos;

    public static GameManager Instance;
    public Camera mainCamera;


    public GameObject player;
   
    public PlayerController playerScript;

    public AudioSource timeLow;

    public int score;
    // Start is called before the first frame update
    private void Awake()
    {
        mainCamera = Camera.main;
        Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }
    public void YouWin()
    {
        score += (int)timer;



        if (PlayerPrefs.HasKey("SelectedWorld") && PlayerPrefs.HasKey("SelectedLevel"))
        {
            int world = PlayerPrefs.GetInt("SelectedWorld");
            int level = PlayerPrefs.GetInt("SelectedLevel");

            FileManager.instance.UnlockLevel(world, level);
        }

        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowWin();
        }

        PlayerPrefs.SetInt("LevelUnlock", 1);

    }

    public void YouLose()
    {
        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowLose();
        }
    }
    public void YouLose(string deathItemName)
    {
        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowLose(deathItemName);
        }
    }

    public void Respawn(GameObject death)
    {
        if (death != null)
            Destroy(death.transform.parent.transform.parent.gameObject);
        //if (player == null)
        //{
            player = Instantiate(PlayerPreFab, playerSpawnPos.transform.position, Quaternion.identity);
        //}
    }



    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer < 30)
        {
            if (!timeLow.isPlaying)
            {
                timeLow.Play();
            }
            if (timer < 0)
            {
                YouLose();
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
