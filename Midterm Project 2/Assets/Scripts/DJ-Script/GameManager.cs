using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   

    public static GameManager Instance;


    public GameObject player;
   
    public PlayerController playerScript;

    public float timer;

    public AudioSource timeLow;

    public int score;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timer = 5f;

    }
    public void YouWin()
    {
        score += (int)timer;
        //Debug.Log("Germ");

        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowWin();
        }

    }

    public void YouLose()
    {
        if (MenuManager.instance != null)
        {
            MenuManager.instance.ShowLose();
        }
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);
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
