using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

public class PlayerRailGrinding : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [Header("Inputs")]
    [SerializeField] bool jump;
    [SerializeField] Vector3 input;

    [Header("Variables")]
    public bool onRail;
    public float grindingSpeed;
    [SerializeField] float heightOffset;
    float timeForFullSpline;
    float elapsedTime;
    [SerializeField] float lerpSpeed = 10f;
    bool isFacingForward;
    [SerializeField] float railExitVelocityMagnifier = 1.5f;

    [Header("Sphere Cast")]
    [SerializeField] GameObject playerOrientation;
    [SerializeField] float sphereCastRadius = 1f;
    public LayerMask railMask;
    RaycastHit hit;
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float railCooldown = 0;
    [SerializeField] float railCooldownTimer = .5f;

    [Header("Scripts")]
    public RailGrinding currentRailScript;
    Rigidbody rb;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        DetectRail();
        railCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (onRail)
        {
            isFacingForward = currentRailScript.normalDirection;
            MovePlayerAlongRail();
        }
    }

    void MovePlayerAlongRail()
    {
        if (currentRailScript != null && onRail)
        {
            float progress = elapsedTime / timeForFullSpline;

            if (progress < 0 || progress > 1f)//elapsedTime check
            {
                if (elapsedTime > .2f)
                {
                    ThrowOffRail();
                    return;
                }
            }
            float nextTimeNormalized;
            nextTimeNormalized = (elapsedTime + Time.deltaTime) / timeForFullSpline;

            float3 pos, tangent, up;
            float3 nextPosFloat, nextTan, nextUp;
            SplineUtility.Evaluate(currentRailScript.railSpline.Spline, progress, out pos, out tangent, out up);
            SplineUtility.Evaluate(currentRailScript.railSpline.Spline, nextTimeNormalized, out nextPosFloat, out nextTan, out nextUp);

            Vector3 worldPos = currentRailScript.LocalToWorldConversion(pos);
            Vector3 nextPos = currentRailScript.LocalToWorldConversion(nextPosFloat);

            transform.position = worldPos + (transform.up * heightOffset);
            //forward
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - worldPos), lerpSpeed * Time.deltaTime);
            //
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, up) * transform.rotation, lerpSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
        }
    }

    public void DetectRail()
    {
        if (!onRail)
        {
            if (railCooldown <= 0)
            {
                if (Physics.SphereCast(transform.position, sphereCastRadius, transform.up * (-1), out hit, maxDistance, railMask))
                {
                    railCooldown = railCooldownTimer;
                    onRail = true;
                    currentRailScript = hit.collider.gameObject.GetComponent<RailGrinding>();
                    CalculateAndSetRailPosition();
                }
            }
        }
    }

    void CalculateAndSetRailPosition()
    {
        timeForFullSpline = currentRailScript.totalSplineLength / grindingSpeed;
        Vector3 SplinePoint;
        float normalizedTime = currentRailScript.CalculateTargetRailPoint(transform.position, out SplinePoint);
        elapsedTime = timeForFullSpline * normalizedTime;
        float3 pos;
        float3 forward;
        float3 up;
        SplineUtility.Evaluate(currentRailScript.railSpline.Spline, normalizedTime, out pos, out forward, out up);
        currentRailScript.CalculateDirection(forward, playerOrientation.transform.forward);
        transform.position = SplinePoint + (transform.up * heightOffset);
    }
    public void ExitRailGrind()
    {
        float progress = elapsedTime / timeForFullSpline;
        float3 pos, tangent, up;
        SplineUtility.Evaluate(currentRailScript.railSpline.Spline, progress, out pos, out tangent, out up);
        playerMovement.rb.velocity = new Vector3(grindingSpeed * railExitVelocityMagnifier * tangent.x, grindingSpeed * railExitVelocityMagnifier * tangent.y, grindingSpeed * railExitVelocityMagnifier * tangent.z);

        onRail = false;
        currentRailScript = null;
        railCooldown = railCooldownTimer;
    }

    void ThrowOffRail()
    {
        ExitRailGrind();
        transform.position += transform.forward * 1; //+1

    }

}
