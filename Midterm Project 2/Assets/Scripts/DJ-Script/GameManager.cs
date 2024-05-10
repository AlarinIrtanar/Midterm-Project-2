using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public GameObject player;
   
    public PlayerController playerScript;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

    }
    public void YouWin()
    {
        Debug.Log("Germ");

    }

    public void YouLose()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying =  false;
#else
        Application.Quit();
#endif
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
