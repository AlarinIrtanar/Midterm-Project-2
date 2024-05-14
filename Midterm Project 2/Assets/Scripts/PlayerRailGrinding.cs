using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRailGrinding : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    private PlayerMovement playerMovement;

>>>>>>> Stashed changes
    [Header("Inputs")]
    [SerializeField] bool jump;
    [SerializeField] Vector3 input;

    [Header("Variables")]
    public bool onRail;
    [SerializeField] float grindingSpeed;
    [SerializeField] float heightOffset;
    float timeForFullSpline;
    float elapsedTime;
    [SerializeField] float lerpSpeed = 10f;

    [Header("Scripts")]
    //[SerializeField] RailScript currentRailScript;
    Rigidbody rb;


    void Start()
    {
<<<<<<< Updated upstream
        
    }

    void Update()
=======
        rb = GetComponent<Rigidbody>();
    }

    //jump off rails here

    private void FixedUpdate()
    {

    }

    private void Update()
>>>>>>> Stashed changes
    {
        
    }
}
