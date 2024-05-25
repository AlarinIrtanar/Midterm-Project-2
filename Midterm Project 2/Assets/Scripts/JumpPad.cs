using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float launchSpeed;
    [SerializeField] AudioSource jumpSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jumpSource.Play();
            other.attachedRigidbody.AddForce(transform.up * launchSpeed, ForceMode.Impulse);
            
        }
    }
}