using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_P : MonoBehaviour
{
    public Transform cam;
    public float speed = 6f, turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 moveDirection;
    Rigidbody rb;
    CharacterController controller;
    Dave dave;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        dave = GetComponent<Dave>();
    }

    private void FixedUpdate()
    {
        /*float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * vertical + transform.right * horizontal;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);*/

        if (!dave.dodging)
            MovementAndCamera();

        //if (Input.GetMouseButtonDown(0))
            //ClickCamera();
    }

    void MovementAndCamera()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    void ClickCamera()
    {
        Vector3 direction = new Vector3(cam.transform.position.x, 0, cam.transform.position.z).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //controller.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    void Pain()
    {
        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");
        Vector3 desiredMoveDirection;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
    }
}
