using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookPull : MonoBehaviour
{
    
    [Header("References")]
    private PlayerMovement playerMovement;
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

    LineRenderer lineRenderer;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
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

    private void LateUpdate()
    {
        if(isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    //throws grappling hook but doesn't start pulling yet
    private void StartGrappling()
    {
        if (grapplingCooldownTimer > 0)
        {
            return;
        }

        isGrappling = true;

        RaycastHit hit;
        
        if(Physics.Raycast(camera.position, camera.forward, out hit, grappleRange, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrappling), grappleDelay);
        }
        else
        {
            grapplePoint = camera.position + camera.forward * grappleRange;

            Invoke(nameof(StopGrappling), grappleDelay);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
    }

    //start pulling towards target
    private void ExecuteGrappling()
    {
        
    }

    private void StopGrappling()
    {
        isGrappling = false;

        grapplingCooldownTimer = grapplingCooldown;

        lineRenderer.enabled = false;
    }
} 
