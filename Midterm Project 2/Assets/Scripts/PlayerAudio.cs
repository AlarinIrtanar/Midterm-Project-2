using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AudioSource FootstepSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if ( playerMovement.state == PlayerMovement.MovementState.Walking || playerMovement.state == PlayerMovement.MovementState.Sprinting)
        //{
        //    Debug
        //}
    }

    //IEnumerator PlayFootseps()
    //{
    //    //yield return waitfor
    //}
}
