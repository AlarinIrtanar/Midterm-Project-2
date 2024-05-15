using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Splines;

public class RailGrinding : MonoBehaviour
{
    public bool normalDirection;
    public SplineContainer railSpline;
    public float totalSplineLength;

    void Start()
    {
        railSpline = GetComponent<SplineContainer>();
        totalSplineLength = railSpline.CalculateLength();
    }

    public Vector3 LocalToWorldConversion(float3 localPoint)
    {
        Vector3 worldPos = transform.TransformPoint(localPoint);
        return worldPos;
    }
    public Vector3 WorldToLocalConversion(float3 worldPoint)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPoint);
        return localPos;
    }

    public float CalculateTargetRailPoint(Vector3 playerPos, out Vector3 worldPosSpline)
    {
        float3 nearestPoint;
        float time;
        SplineUtility.GetNearestPoint(railSpline.Spline, WorldToLocalConversion(playerPos), out nearestPoint, out time);
        worldPosSpline = LocalToWorldConversion(nearestPoint);
        return time;
    }

    public void CalculateDirection(float3 railForward, Vector3 playerForward)
    {
        float angle = Vector3.Angle(railForward, playerForward.normalized);
        if (angle > 90f)
        {
            normalDirection = false;
        }
        else
        {
            normalDirection = true;
        }
    }
}