using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    [Header("Drag the player's \"Orientation\" onto this")]
    public Transform orientation;
    public Transform camHolder;

    float rotX;
    float rotY;
    float zTilt;

    // Camera lerping
    Camera cam;
    float targetFov;
    float targetZTilt;



    void Start()
    {
        // Start the mouse off as invisible and locked on the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the camera's default state
        cam = GetComponent<Camera>();
        targetFov = cam.fieldOfView;
        targetZTilt = transform.localEulerAngles.z;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        rotY += mouseX;
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        // Rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(rotX, rotY, zTilt);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);

        // Lerp cam values
        LerpCamValues();
    }

    public void DoFov(float endValue)
    {
        targetFov = endValue;
    }

    public void DoTilt(float endZTilt)
    {
        targetZTilt = endZTilt;
    }

    private void LerpCamValues()
    {
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFov, 50f * Time.deltaTime);
        zTilt = Mathf.MoveTowards(zTilt, targetZTilt, 20f * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0, 0, zTilt);
        //transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(transform.rotation.z, targetZTilt, 20f * Time.deltaTime));
        //transform.rotation = Mathf.MoveTowardsAngle(transform.rotation, Quaternion.Euler(0, 0, targetZTilt), 20f * Time.deltaTime);
    }
}
