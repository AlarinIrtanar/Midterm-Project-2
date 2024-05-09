using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookPull : MonoBehaviour
{
    /*[Header("References")]
    //private PlayerMovementGrappling playerMovement;
    public Transform camera;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;

    [Header("Grappling")]
    public float grappleRange;
    public float grappleDelay;

    public Vector3 grapplePoint;


    [Header("Cooldown")]
    public float grapplingCooldown;
    public float grapplingCooldownTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool isGrappling;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovementGrappling>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrappling();
        }

        if(grapplingCooldown > 0)
        {
            grapplingCooldown -= Time.deltaTime;
        }
    }

    private void StartGrappling()
    {
        if (grapplingCooldownTimer > 0)
        {
            return;
        }

        isGrappling = true;

        RaycastCommand hit;
        
        if(Physics.Raycast(camera.position, camera.forward, out hit, grappleRange, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(StartGrappling), grappleDelay);
        }
        else
        {
            grapplePoint = camera.position + camera.forward * grappleRange;

            Invoke(nameof(StopGrappling), grappleDelay);
        }
    }

    private void StopGrappling()
    {
        isGrappling = false;

        grapplingCooldownTimer = grapplingCooldown;
    }*/
} 
