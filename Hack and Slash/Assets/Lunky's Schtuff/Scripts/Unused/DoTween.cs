using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTween : MonoBehaviour
{
    public float duration, strength = 1f, randomness = 90f;
    public int vibration = 10;
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            cam.DOShakePosition(duration, strength, vibration, randomness, false, true);
    }
}
