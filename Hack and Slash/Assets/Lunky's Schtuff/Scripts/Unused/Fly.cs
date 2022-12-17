using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float forceMagnitude = 10f;
    public Vector3 direction;

    float dir;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating(nameof(Jump), 0.5f, 0.75f);
    }

    void Update()
    {
        dir = Input.GetAxisRaw("Horizontal");

        transform.RotateAround(transform.position, transform.up, 20 * dir * Time.deltaTime);
    }

    void Jump()
    {
        rb.AddForce(direction * forceMagnitude);
    }

    private void FixedUpdate()
    {
        
    }
}
