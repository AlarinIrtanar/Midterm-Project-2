using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    float horizontalInput;
    float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft;
    bool wallRight;

    [Header("Exiting")]
    bool exitingWall;
    public float exitWallTime;
    float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    PlayerMovement pm;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        // Check if the player is high enough in the air to start wall running
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0f && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            // Wallrun timer
            if (wallRunTimer > 0f)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0f && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // Wall jump
            if (Input.GetKeyDown(jumpKey))
                WallJump();
        }

        // State 2 - Exiting Wall
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0f)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0f)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // Forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Push into wall if not trying to get away from wall
        if (!(wallLeft && horizontalInput > 0f) && !(wallRight && horizontalInput < 0f))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
    }
    
    private void WallJump()
    {
        // Enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // Apply force (Also reset y velocity before to make jump feel natural)
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
