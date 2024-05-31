using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;

    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Sprinting")]
    public float sprintStaminaRegenSpeed;
    public float sprintStaminaDrainSpeed;
    float sprintStamina = 1f;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    //[Header("Keybinds")]
    //public KeyCode jumpKey = KeyCode.Space;
    string jumpButton; // Replacement by Matthew
    //public KeyCode sprintKey = KeyCode.LeftShift;
    string sprintButton; // Replacement by Matthew
    //public KeyCode crouchKey = KeyCode.LeftControl;
    string crouchButton; // Replacement by Matthew
    string shootButton; // Replacement by Matthew

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Camera")]
    [SerializeField] GameObject cam; // player camera
    private PlayerCam playerCamScript; // PlayerCam script
    public bool viewBobbing;
    public float viewBobbingIntensityMultiplierVert;
    public float viewBobbingIntensityMultiplierHoriz;
    public float viewBobbingSpeed;
    float viewBobbingIntensity;
    float viewBobbingTargetIntensity;
    float viewBobbingProgress;
    bool doingViewBobbing;

    // Speed-based fov shifting
    [SerializeField] float fovShiftIntensity;
    [SerializeField] float fovShiftFalloffIntensity;

    //[Header("Shooting")]
    //[SerializeField] int shootDamage;
    //[SerializeField] float shootCooldown;
    //[SerializeField] float shootDistance;
    //bool isShooting;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    // Steps
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] float2 audStepVolRange;
    [SerializeField] float stepSize;
    float curDistStepped;

    // Jumps
    [SerializeField] AudioClip[] audJumps;
    [SerializeField] float2 audJumpVolRange;

    // Landing
    [SerializeField] AudioClip[] audLandings;
    [SerializeField] float2 audLandingVolRange;
    bool prevLanded;
    bool isLanded;
    float prevYVel;
    const float landSoundCooldown = 0.25f; // time to tick for
    float landSoundTimer; // what ticks

    private PlayerRailGrinding playerRailGrinding;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    public Rigidbody rb;

    public MovementState state;


    public enum MovementState
    {
        Walking,
        Sprinting,
        Wallrunning,
        Crouching,
        Sliding,
        Air
    }

    public bool sliding;
    public bool wallrunning;
    public bool activeGrappling;

    private void Start()
    {
        playerRailGrinding = GetComponent<PlayerRailGrinding>();

        // Jump Button
        if (PlayerPrefs.HasKey("Jump Button"))
            jumpButton = PlayerPrefs.GetString("Jump Button");
        else
            jumpButton = "space";

        // Sprint Button
        if (PlayerPrefs.HasKey("Sprint Button"))
            sprintButton = PlayerPrefs.GetString("Sprint Button");
        else
            sprintButton = "left shift";

        // Crouch Button
        if (PlayerPrefs.HasKey("Crouch Button"))
            crouchButton = PlayerPrefs.GetString("Crouch Button");
        else
            crouchButton = "left ctrl";

        //// Shoot Button
        //if (PlayerPrefs.HasKey("Shoot Button"))
        //    shootButton = PlayerPrefs.GetString("Shoot Button");
        //else
        //    shootButton = "mouse 0";

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        playerCamScript = cam.GetComponent<PlayerCam>();

        //SpawnPlayer();
    }

    private void Update()
    {
        // Ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        grounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.3f, playerHeight * 0.5f + 0.2f - 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        DoViewBobbing();

        // handle drag
        if (grounded && !activeGrappling)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void RestRestrictions()
    {
        activeGrappling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            RestRestrictions();
            //activeGrappling = false;
            GetComponent<GrapplingHookPull>().StopGrappling();
        }
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When to jump
        if (Input.GetKey(jumpButton) && readyToJump && (grounded || OnSlope()))
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start crouch
        if (Input.GetKeyDown(crouchButton) && !playerRailGrinding.onRail)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // Stop crouch
        if (Input.GetKeyUp(crouchButton) || playerRailGrinding.onRail)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        //// Do shooting
        //if (!MenuManager.instance.isPaused && !isShooting && Input.GetKeyDown(shootButton))
        //    StartCoroutine(Shoot());
    }

    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.Wallrunning;
            desiredMoveSpeed = wallrunSpeed;
            DoStepping();

            viewBobbingTargetIntensity = 0.5f;
        }

        // Move - Sliding
        else if (sliding)
        {
            state = MovementState.Sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;

            viewBobbingTargetIntensity = 0f;
        }

        // Mode - Crouching
        else if (Input.GetKey(crouchButton) && !playerRailGrinding.onRail)
        {
            state = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;

            viewBobbingTargetIntensity = 0.7f;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintButton) && sprintStamina > 0f)
        {
            state = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;


            if (horizontalInput != 0f || verticalInput != 0f)
            {
                // Drain sprint stamina (Moving)
                sprintStamina -= sprintStaminaDrainSpeed * Time.deltaTime;
                if (sprintStamina < 0f) sprintStamina = 0f;
            }
            else
            {
                // Regen sprint stamina (Not moving)
                sprintStamina += sprintStaminaRegenSpeed * Time.deltaTime;
                if (sprintStamina > 1f) sprintStamina = 1f;
            }

            if (HUDManager.instance != null)
            {
                HUDManager.instance.SetStamina(sprintStamina, 1);
            }

            DoStepping();

            viewBobbingTargetIntensity = 0.5f;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;

            // Regen sprint stamina
            sprintStamina += sprintStaminaRegenSpeed * Time.deltaTime;
            if (sprintStamina > 1f) sprintStamina = 1f;

            if (HUDManager.instance != null)
            {
                HUDManager.instance.SetStamina(sprintStamina, 1);
            }

            DoStepping();

            viewBobbingTargetIntensity = 1f;
        }

        // Mode - Air
        else
        {
            state = MovementState.Air;

            // Regen sprint stamina
            sprintStamina += sprintStaminaRegenSpeed * Time.deltaTime;
            if (sprintStamina > 1f) sprintStamina = 1f;

            if (HUDManager.instance != null)
            {
                HUDManager.instance.SetStamina(sprintStamina, 1);
            }

            viewBobbingTargetIntensity = 0f;
        }

        // Check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 8f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // Do landing stuff
        isLanded = grounded || wallrunning || OnSlope();
        if (isLanded && !prevLanded)
        {
            if (landSoundTimer <= 0f)
                PlayRandFromList(audLandings, audLandingVolRange);
            landSoundTimer = landSoundCooldown;
        }
        prevLanded = isLanded;
        landSoundTimer -= Time.deltaTime;

        // Do speed-based fov shifting
        float camFovShift = fovShiftIntensity * (1f - (fovShiftFalloffIntensity / (rb.velocity.magnitude + fovShiftFalloffIntensity))) * (Vector3.Dot(Camera.main.transform.forward, rb.velocity.normalized));
        playerCamScript.SetFovShift(camFovShift);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // Smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, 3f * time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        //To stop player from moving while grappling
        if (activeGrappling)
        {
            return;
        }
        if (playerRailGrinding.onRail)
        {
            return;
        }

        // Calculate movement direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // On ground
        if (grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);

        // In air
        else
            rb.AddForce(moveDir.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        // Turn gravity off while on slope
        if (!wallrunning)
            rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (activeGrappling)
        {
            return;
        }

        // Limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // Limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if (playerRailGrinding.onRail)
        {
            playerRailGrinding.ExitRailGrind();
        }
        exitingSlope = true;

        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Play sound
        PlayRandFromList(audJumps, audJumpVolRange);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void PlayRandFromList(AudioClip[] auds, float2 volRange)
    {
        audioSource.PlayOneShot(auds[UnityEngine.Random.Range(0, auds.Length)], UnityEngine.Random.Range(volRange.x, volRange.y));
    }

    private void DoStepping()
    {
        Vector3 horizontalMovement = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        viewBobbingProgress += horizontalMovement.magnitude * viewBobbingSpeed * Time.deltaTime;
        curDistStepped += horizontalMovement.magnitude * Time.deltaTime;
        if (curDistStepped >= stepSize)
        {
            curDistStepped %= stepSize;
            PlayRandFromList(audSteps, audStepVolRange);
        }
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrappling = true;

        velocityToSet = DeterminePullVelocity(transform.position, targetPosition, trajectoryHeight);
        //Player will get pulled after 0.1 seconds
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(RestRestrictions), 3f);
    }


    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
        //activeGrappling = false;
    }
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private void DoViewBobbing()
    {
        if (viewBobbing)
        {
            viewBobbingIntensity = Mathf.MoveTowards(viewBobbingIntensity, viewBobbingTargetIntensity, 0.5f * Time.deltaTime); // gradually change intensity
            float horizontalBobPos = Mathf.Sin(Mathf.PI * viewBobbingProgress / 2f);
            float verticalBobPos = horizontalBobPos * horizontalBobPos * viewBobbingIntensity * viewBobbingIntensityMultiplierVert;
            horizontalBobPos *= viewBobbingIntensity * viewBobbingIntensityMultiplierHoriz;
            cam.transform.localPosition = new Vector3(horizontalBobPos, verticalBobPos, cam.transform.localPosition.z);
        }
        else
            cam.transform.localPosition = new Vector3(0f, 0f, cam.transform.localPosition.z);
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public Vector3 DeterminePullVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float yDisplacement = endPoint.y - startPoint.y;
        Vector3 xzDisplacement = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        //Vector3 yVelocity = new Vector3(0f, yDisplacement, 0f);
        Vector3 yVelocity = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        //Vector3 xzVelocity = new Vector3(xzDisplacement.x, 0f, xzDisplacement.z);
        Vector3 xzVelocity = xzDisplacement / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (yDisplacement - trajectoryHeight) / gravity));

        return xzVelocity + yVelocity;
    }

    //IEnumerator Shoot()
    //{
    //    isShooting = true;
    //    PlayRandFromList(audShoots, audShootVolRange);
    //    RaycastHit hit;
    //    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
    //    {
    //        IDamage dmg = hit.collider.GetComponent<IDamage>();
    //        if (dmg != null)
    //        {
    //            // Shot something!!!
    //            dmg.TakeDamage(shootDamage);
    //        }
    //    }
    //    yield return new WaitForSeconds(shootCooldown);
    //    isShooting = false;
    //}
}