 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamRotation : MonoBehaviour
{
    public float sensX, sensY;

    public Transform orientation;

    float xRotation, yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.fixedDeltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }
}
