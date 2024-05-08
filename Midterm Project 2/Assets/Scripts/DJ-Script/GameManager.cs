using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
