using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheShadow : MonoBehaviour
{
    public Zed player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Zed>();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        player.shadowSpawned = false;
        player.positionSwitched = false;
    }
}
