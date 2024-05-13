using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] float offset;
    Quaternion startRot;
    Vector3 axis;

    private void Start()
    {
        startRot = transform.rotation;
        axis = transform.forward;
    }

    private void Update()
    {
        transform.rotation = Quaternion.AngleAxis(amplitude * Mathf.Cos(Time.time * frequency + offset), axis) * startRot;
    }


    //float lerpedValue;
    //[SerializeField] float currentRotation;
    //[SerializeField] bool isSwingingForward;
    //[SerializeField] float elapsedTime;
    //[SerializeField] float from;
    //[SerializeField] float to;
    //Quaternion start;
    //Quaternion target;
    //[SerializeField] float targetRotationHeight;
    //[SerializeField] float roatationSpeed;
    //[SerializeField] float rotationDuration;
    //
    //void Start()
    //{
    //    isSwingingForward = true;
    //    elapsedTime = 0;
    //    //start = Quaternion.Euler(new Vector3(0, 0, transform.rotation.z));
    //    //target = Quaternion.Euler(new Vector3(0, 0, targetRotationHeight));
    //    from = transform.rotation.z;
    //    to = targetRotationHeight;
    //}
    //
    //void Update()
    //{
    //    //transform.rotation = Quaternion.Lerp(from, to, elapsedTime * roatationSpeed);
    //
    //    currentRotation = transform.rotation.z;
    //    if (true == isSwingingForward)
    //    {
    //        //lerp rotation to targetRotationHeight;
    //        float t = elapsedTime / rotationDuration;
    //        lerpedValue = Mathf.Lerp(from, to, t);
    //        transform.rotation = Quaternion.Euler(0, 0, lerpedValue);
    //        elapsedTime += Time.deltaTime;
    //        if (elapsedTime >= rotationDuration)
    //        {
    //            //from = transform.rotation.z;
    //            from = targetRotationHeight;
    //            to = -targetRotationHeight;
    //            elapsedTime = 0;
    //            isSwingingForward = false;
    //        }
    //    }
    //    else if (false == isSwingingForward)
    //    {
    //        //lerp rotation to -targetRotationHeight;
    //        float t = elapsedTime / rotationDuration;
    //        lerpedValue = Mathf.Lerp(from, to, t);
    //        transform.rotation = Quaternion.Euler(0, 0, lerpedValue);
    //        elapsedTime += Time.deltaTime;
    //        if (elapsedTime >= rotationDuration)
    //        {
    //            //from = transform.rotation.z;
    //            from = -targetRotationHeight;
    //            to = targetRotationHeight;
    //            elapsedTime = 0;
    //            isSwingingForward = true;
    //        }
    //    }
    //    //elapsedTime += Time.deltaTime;
    //
    //}
}
