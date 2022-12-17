using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    Dave pm;
    public Transform cam;
    public Transform player;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.LeftShift;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predicitonPoint;

    public bool grappling;
    public EnemyController_P selectedEnemy;

    void Start()
    {
        pm = GetComponent<Dave>();
    }

    void Update()
    {
        CheckForGrapplePoints();

        if (Input.GetKeyDown(grappleKey) && !pm.dodging)
            StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;

        if (pm.grounded)
            overshootYAxis = -1;
        else
            overshootYAxis = 0;
    }

    private void LateUpdate()
    {
        if (grappling)
            lr.SetPosition(0, player.position);
    }

    void StartGrapple()
    {
        if (grapplingCdTimer > 0)
            return;

        if (predictionHit.point == Vector3.zero)
            return;

        if (GetComponent<Swinging>() != null)
            GetComponent<Swinging>().StopSwing();

        grappling = true;
        pm.freeze = true;

        grapplePoint = predictionHit.transform.position;
        selectedEnemy.agent.speed = 0f;
        Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0)
            highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        predictionHit.transform.GetComponent<EnemyController_P>().TakeDamage(10);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;
        if (selectedEnemy != null)
            selectedEnemy.agent.speed = 3.5f;

        lr.enabled = false;
    }

    void CheckForGrapplePoints()
    {
        RaycastHit sphereCastHit;
        bool inCircle = Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxGrappleDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        bool inRay = Physics.Raycast(cam.position, cam.forward, out raycastHit, maxGrappleDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
        }

        else if (sphereCastHit.point != Vector3.zero)
        {
            realHitPoint = sphereCastHit.point;

            if (inCircle)
            {
                selectedEnemy = sphereCastHit.transform.gameObject.GetComponent<EnemyController_P>();
            }
        }

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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxGrappleDistance);
    }

    /*if (Physics.CheckSphere(player.position, maxGrappleDistance, whatIsGrappleable))
        {
            gPoints = Physics.OverlapSphere(player.position, maxGrappleDistance, whatIsGrappleable);

            float dstToClosestGPoint = Mathf.Infinity;
            GameObject closestGPoint = null;

            foreach (Collider gPoint in gPoints)
            {
                float dstToGPoint = (gPoint.transform.position - player.transform.position).sqrMagnitude;
                if(dstToGPoint < dstToClosestGPoint)
                {
                    dstToClosestGPoint = dstToGPoint;
                    closestGPoint = gPoint.gameObject;
                    grapplePoint = closestGPoint.transform.position;
                }
            }
        }*/


    /*else
    {
        grapplePoint = player.position + player.forward * maxGrappleDistance;

        Invoke(nameof(StopGrapple), grappleDelayTime);
    }*/
}
