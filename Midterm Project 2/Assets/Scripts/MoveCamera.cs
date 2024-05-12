using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Drag the player's \"Camera Pos\" onto this")]
    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
