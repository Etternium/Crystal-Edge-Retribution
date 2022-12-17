using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dave : MonoBehaviour
{
    SkillTreeUIScript skillTreeUIScript;//JS.Added script call

    [Header("Movement")]
    public Combat combat;
    float moveSpeed;
    public TextMeshProUGUI debugingSpeedText;
    public float walkSpeed;
    public float sprintSpeed;
    public float swingSpeed;
    public float blockSpeed;
    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    [Header("Dodging")]
    public bool dodging, readyToDodge;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode dodgeKey = KeyCode.Space;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    public Transform orientation, playerModel;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    Animator animator;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        grappling,
        swinging,
        walking,
        sprinting,
        crouching,
        blocking,
        dashing,
        dodging,
        air
    }

    public bool freeze;
    public bool activeGrapple;
    public bool swinging;
    public bool dashing;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        rb = GetComponentInParent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        //animator = GetComponentInParent<Animator>();
        rb.freezeRotation = true;

        readyToJump = true;
        readyToDodge = true;

        startYScale = transform.localScale.y;
    }

    void Update()
    {
        grounded = Physics.Raycast(playerModel.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        //grounded = Physics.CheckSphere(groundCheck.position, 0.4f, whatIsGround);
        //if (desiredMoveSpeed > 0.1f)
            //Pain();

        MyInput();
        Run();
        SpeedControl();
        StateHandler();
        if (!activeGrapple || swinging)
            GravityControl();

        /*if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            rb.drag = groundDrag;*/
        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!dodging)
            MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");   //was without raw
        verticalInput = Input.GetAxisRaw("Vertical");       //was without raw

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetKeyDown(dodgeKey) && readyToDodge && grounded)
        {
            animator.SetTrigger("Dodge");
            readyToDodge = false;
        }

        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    public float desiredMoveSpeed;
    float lastDesiredMoveSpeed;
    MovementState lastState;
    bool keepMomentum;
    void StateHandler()
    {
        if(freeze)
        {
            state = MovementState.freeze;
            desiredMoveSpeed = 0;
            rb.velocity = Vector3.zero;
        }

        else if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed /*+ (dashSpeed * skillTreeUIScript.skillTreeMOVE)*/;//JS.Added skill tree variable
            speedChangeFactor = dashSpeedChangeFactor;
        }

        else if(swinging)
        {
            state = MovementState.swinging;
            desiredMoveSpeed = swingSpeed;
        }

        else if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed /*+ (crouchSpeed * skillTreeUIScript.skillTreeMOVE)*/;//JS.Added skill tree variable
        }

        else if (Input.GetKey(combat.blockKey))
        {
            state = MovementState.blocking;
            desiredMoveSpeed = blockSpeed /*+ (blockSpeed * skillTreeUIScript.skillTreeMOVE)*/;//JS.Added skill tree variable
        }

        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed /*+ (sprintSpeed * skillTreeUIScript.skillTreeMOVE)*/;//JS.Added skill tree variable
        }

        else if(dodging)
        {
            state = MovementState.dodging;
            Dodge();
        }

        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed /*+ (walkSpeed * skillTreeUIScript.skillTreeMOVE)*/ + (GetComponentInChildren<Combat>().value * 5f);//JS.Added skill tree variable
            //desiredMoveSpeed = walkSpeed /*+ (walkSpeed * skillTreeUIScript.skillTreeMOVE)*/ + (GetComponentInParent<Combat>().value * 5f);//JS.Added skill tree variable
        }

        else
        {
            state = MovementState.air;
            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChangded = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing)
            keepMomentum = true;

        if(desiredMoveSpeedHasChangded)
        {
            if(keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    float speedChangeFactor;
    IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    void MovePlayer()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        if (state == MovementState.dashing || activeGrapple || swinging)
            return;

        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection = verticalInput * forward + horizontalInput * right;

        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (activeGrapple)
            return;

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        
        else
        {
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);

        debugingSpeedText.text = "Speed: " + flatVel.magnitude;
    }

    void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void Dodge()
    {
        if (dodging)
        {
            rb.AddForce(transform.forward * moveSpeed * 5f, ForceMode.Force);
        }
    }

    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public void ResetDodge()
    {
        readyToDodge = true;
        //make player vulnerable
        //able to attack
    }

    bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(playerModel.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    Vector3 velocityToSet;
    void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(playerModel.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    void GravityControl()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    void Run()
    {
        if(horizontalInput != 0 || verticalInput != 0)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(playerModel.position, Vector3.down);
    }
}
