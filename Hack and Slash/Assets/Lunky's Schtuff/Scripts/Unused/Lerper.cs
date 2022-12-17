using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    public AnimationCurve curve;
    public Vector3 positionGoal;
    public Vector3 rotationGoal;
    public float goalScale = 2;
    public float speed = 0.5f;

    float current, target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            target = target == 0 ? 1 : 0;

        current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);

        transform.position = Vector3.Lerp(Vector3.zero, positionGoal, curve.Evaluate(current));
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(rotationGoal), curve.Evaluate(current));
        //transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * goalScale, curve.Evaluate(current));
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * goalScale, curve.Evaluate(Mathf.PingPong(current, 0.5f) * 2));
    }
}
