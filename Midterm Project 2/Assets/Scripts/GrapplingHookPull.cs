using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplingHookPull : MonoBehaviour
{
    
    [Header("References")]
    private PlayerMovement playerMovement;
    public Transform camera;
    public Transform gunTip;
    public LineRenderer lineRenderer;
    public LayerMask whatIsGrappleable;

    [Header("Grappling")]
    public float grappleRange;
    public float grappleDelay;
    public float overshootYAxis;

    public Vector3 grapplePoint;


    [Header("Cooldown")]
    public float grapplingCooldown;
    public float grapplingCooldownTimer;

    //[Header("Input")]
    //public KeyCode grappleKey = KeyCode.Mouse1;
    string grappleButton; // Replacement by Matthew

    [Header("Audio")]
    public AudioSource grappleShootAudio;
    public AudioSource grapplePullAudio;


    private bool isGrappling;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (PlayerPrefs.HasKey("Grapple Button"))
        {
            grappleButton = PlayerPrefs.GetString("Grapple Button");
        }
        else
        {
            grappleButton = "mouse 1";
            PlayerPrefs.SetString("Grapple Button", "mouse 1");
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(grappleButton))
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
        if (grapplingCooldown > 0)
        {
            return;
        }
        grappleShootAudio.Play();
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
        grapplePullAudio.Play();
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y -1f, transform.position.z);


        float grapplePointRealativeYPosition = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRealativeYPosition + overshootYAxis;

        if (grapplePointRealativeYPosition < 0)
        {
            highestPointOnArc = overshootYAxis;
        }
        playerMovement.JumpToPosition(grapplePoint, highestPointOnArc);

        grapplingCooldown = grapplingCooldownTimer;
        Invoke(nameof(StopGrappling), 1f);
    }

    public void StopGrappling()
    {
        isGrappling = false;
        grapplePullAudio.Stop();

        grapplingCooldown = grapplingCooldownTimer;

        lineRenderer.enabled = false;
    }

    
} 
