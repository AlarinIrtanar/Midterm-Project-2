using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] float boostSpeed;
    [SerializeField] Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //addforce according to /transform.rotation/
            //GameManager.Instance.playerScript.controller.enabled = false;
            //            GameManager.Instance.player.transform.position = teleportTarget.transform.position;

            //            GameManager.Instance.player.transform.position = teleportTarget.transform.position;

            //GameManager.Instance.playerScript.controller.enabled = false;
            other.attachedRigidbody.AddForce(transform.forward * boostSpeed);
            //GameManager.Instance.playerScript.playerVel.y = launchSpeed;
            //GameManager.Instance.playerScript.controller.enabled = true;

            //GameManager.Instance.playerScript.enabled = true;
        }
        if (other.CompareTag("Test"))
        {
            //addforce according to /transform.rotation/
            //            GameManager.Instance.playerScript.enabled = false;
            //GameManager.Instance.playerScript.controller.enabled = false;
            //            GameManager.Instance.player.transform.position = teleportTarget.transform.position;
            //other.attachedRigidbody.AddForce(transform.up * launchSpeed);

            other.attachedRigidbody.AddForce(transform.forward * boostSpeed);
            
            //GameManager.Instance.playerScript.enabled = true;
            //            GameManager.Instance.playerScript.enabled = true;
        }
    }
}
