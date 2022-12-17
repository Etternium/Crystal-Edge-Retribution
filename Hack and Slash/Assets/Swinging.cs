using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Swinging : MonoBehaviour
{
    [Header("Input")]
    public KeyCode swingKey;

    [Header("References")]
    public LineRenderer lr;
    public Transform cam;
    public Transform player;
    public LayerMask whatIsGrappleable;
    Dave pm;

    [Header("Swinging")]
    public float spring = 4.5f, damper = 7f, massScale = 4.5f;
    float maxSwingDistance = 25f;
    bool movedToMinGrappleDist;
    Vector3 swingPoint, endPos, currentPos;
    SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predicitonPoint;

    public Transform[] routes;
    int routeToGo;
    float tParam, speedModifier;
    bool coroutineAllowed;
    Vector3 playerPos;

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;

        pm = GetComponent<Dave>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckForSwingPoints();

        if (Input.GetKeyDown(swingKey) /*&& !pm.dodging*/)
        {
            StartSwing();
            
            //Akshan();
            //rb.AddForce(orientation.forward * horizontalThrustForce, ForceMode.Impulse);

            /*if (pm.swinging && !coroutineAllowed)
                coroutineAllowed = true;*/

            //if (coroutineAllowed)
            //StartCoroutine(GoByTheRoute(routeToGo));
        }

        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
            StopAllCoroutines();
            movedToMinGrappleDist = false;
        }

        /*if (pm.swinging)
            Akshan();
        else
            return;*/

        if (Vector3.Distance(transform.position, swingPoint) < 5.5f)
        {
            movedToMinGrappleDist = false;
        }

        if (joint != null)
        {
            OdmGearMovement();
            /*if (Vector3.Distance(transform.position, swingPoint) > 5.5f && !movedToMinGrappleDist)
            {
                transform.position = Vector3.Lerp(transform.position, swingPoint, Time.deltaTime);
                movedToMinGrappleDist = true;
            }*/
        }

        if (pm.swinging)
        {
            /*if (transform.position.y <= 2f)
                transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
            else
                return;*/
            //Akshan();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartSwing()
    {
        if (predictionHit.point == Vector3.zero)
            return;

        if (GetComponent<Grappling>() != null)
            GetComponent<Grappling>().StopGrapple();
        pm.ResetRestrictions();

        pm.swinging = true;

        swingPoint = predictionHit.transform.position;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        /*joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;*/

        joint.maxDistance = 5f;
        joint.minDistance = 3f;

        //customise these values as we like
        joint.spring = spring;
        joint.damper = damper;
        joint.massScale = massScale;

        lr.positionCount = 2;
        currentGrapplePosition = player.position;
    }

    public void StopSwing()
    {
        /*if (pm.swinging)
            rb.AddForce(orientation.forward * forwardThrustForce*Time.deltaTime/*, ForceMode.Force);*/
        pm.swinging = false;

        lr.positionCount = 0;
        Destroy(joint);
    }

    Vector3 currentGrapplePosition;

    void DrawRope()
    {
        if (!joint)
            return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, player.position);
        lr.SetPosition(1, swingPoint);
    }

    void OdmGearMovement()
    {
        /*if (Input.GetKey(KeyCode.D))
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);*/

        if (Input.GetKey(KeyCode.W))
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);

        /*if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }

        if(Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }*/
    }

    void Akshan()
    {
        currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 swingPointPos = new Vector3(swingPoint.x, 0f, swingPoint.z);
        Vector3 difference = new Vector3(swingPointPos.x - currentPos.x, currentPos.y + 1f, swingPointPos.z - currentPos.z);
        endPos = swingPointPos + difference;

        //transform.position = Vector3.Lerp(currentPos, endPos, Time.deltaTime);
        //transform.position = Vector3.MoveTowards(currentPos, endPos, 1f);
        //rb.AddForce((swingPoint - currentPos) * forwardThrustForce * Time.deltaTime, ForceMode.VelocityChange);
        //rb.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            playerPos = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = playerPos;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo++;
        if (routeToGo > routes.Length - 1)
            routeToGo = 0;
        coroutineAllowed = true;
    }

    void CheckForSwingPoints()
    {
        if (joint != null)
            return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero)
        {
            predicitonPoint.gameObject.SetActive(true);
            predicitonPoint.position = realHitPoint;
        }

        else
        {
            predicitonPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(endPos, 1f);
    }


    /*if (Physics.CheckSphere(player.position, 25f, whatIsGrappleable))
    {
        gPoints = Physics.OverlapSphere(player.position, 25f, whatIsGrappleable);

        float dstToClosestGPoint = Mathf.Infinity;
        GameObject closestGPoint = null;

        foreach (Collider gPoint in gPoints)
        {
            float dstToGPoint = (gPoint.transform.position - player.transform.position).sqrMagnitude;
            if (dstToGPoint < dstToClosestGPoint)
            {
                dstToClosestGPoint = dstToGPoint;
                closestGPoint = gPoint.gameObject;
                swingPoint = closestGPoint.transform.position;
            }
        }
    }*/
}
