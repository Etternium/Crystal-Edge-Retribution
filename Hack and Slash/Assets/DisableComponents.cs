using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableComponents : MonoBehaviour
{
    EnemyController_P controller;
    Animator animator;
    Transform player;

    void Start()
    {
        controller = GetComponent<EnemyController_P>();
        animator = GetComponentInChildren<Animator>();
        player = PlayerManager.instance.player.transform;
    }

    void Update()
    {
        bool close = Vector3.Distance(transform.position, player.position) < 50f;

        if(close)
        {
            controller.enabled = true;
            animator.enabled = true;
        }

        else
        {
            controller.enabled = false;
            animator.enabled = false;
        }
    }
}
