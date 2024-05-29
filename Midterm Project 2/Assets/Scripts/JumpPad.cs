using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float launchSpeed;
    [SerializeField] AudioSource jumpSource;
    Rigidbody rb;
    float timer;

    private void Update()
    {
        if (timer > 0f && rb != null)
        {
            rb.velocity = new Vector3(rb.velocity.x, launchSpeed * 0.9f, rb.velocity.z);
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jumpSource.Play();
            rb = other.attachedRigidbody;
            rb.velocity = new Vector3(rb.velocity.x, launchSpeed * 0.9f, rb.velocity.z);
            timer = 0.05f;
            //other.attachedRigidbody.AddForce(transform.up * launchSpeed, ForceMode.Impulse);
        }
    }

}