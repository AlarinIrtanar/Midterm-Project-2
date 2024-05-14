using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AudioSource FootstepSource;

    Rigidbody rb;
    bool playingFootsteps;
    // Start is called before the first frame update
    void Start()
    {
        rb = playerMovement.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !playingFootsteps && playerMovement.state == PlayerMovement.MovementState.Walking && rb.velocity.magnitude > 0)
        {
            StartCoroutine(PlayFootseps(.5f));
        }
        else if (!playingFootsteps && playerMovement.state == PlayerMovement.MovementState.Sprinting && rb.velocity.magnitude > 0)
        {
            StartCoroutine(PlayFootseps(.35f));
        }
        else if (!playingFootsteps && playerMovement.state == PlayerMovement.MovementState.Crouching && rb.velocity.magnitude > 0)
        {
            StartCoroutine(PlayFootseps(.75f));
        }
    }

    IEnumerator PlayFootseps(float delay)
    {
        playingFootsteps = true;
        Debug.Log(rb.velocity.magnitude);
        FootstepSource.Play();
        yield return new WaitForSeconds(delay);
        playingFootsteps = false;
    }
}
