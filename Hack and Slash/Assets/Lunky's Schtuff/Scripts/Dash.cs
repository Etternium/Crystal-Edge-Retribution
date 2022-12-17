using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    Rigidbody rb;
    Dave dave;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("CameraEffects")]
    public CamRotation cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dave = GetComponent<Dave>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dashing();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }
    
    public void Dashing()
    {
        if (dashCdTimer > 0)
            return;
        else
            dashCdTimer = dashCd;

        dave.dashing = true;
        dave.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    public void DashToEnemy(Transform enemyPos)
    {
        if (dashCdTimer > 0)
            return;
        else
            dashCdTimer = dashCd;

        dave.dashing = true;
        dave.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Vector3 direction = enemyPos.position - transform.position;

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    Vector3 delayedForceToApply;
    void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    void ResetDash()
    {
        dave.dashing = false;
        dave.maxYSpeed = 0;

        cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
