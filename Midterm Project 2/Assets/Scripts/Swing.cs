using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    float currentRotation;
    bool isSwingingForward;
    [SerializeField] float targetRotationHeight;
    [SerializeField] float roatationSpeed;

    void Start()
    {
        isSwingingForward = true;
    }

    void Update()
    {
        currentRotation = transform.rotation.z; 
        if (isSwingingForward)
        {
            //lerp rotation to targetRotationHeight;

            if (currentRotation >= targetRotationHeight)
            {
                isSwingingForward = false;
            }
        }
        else
        {
            //lerp rotation to -targetRotationHeight;

            if (currentRotation <= -targetRotationHeight)
            {
                isSwingingForward = true;
            }
        }
    }
}
