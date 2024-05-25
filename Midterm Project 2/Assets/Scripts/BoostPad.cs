using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] float boostSpeed;
    [SerializeField] AudioSource boostAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boostAudio.Play();
            other.attachedRigidbody.AddForce(transform.forward * boostSpeed, ForceMode.Impulse);
        }
    }
}
