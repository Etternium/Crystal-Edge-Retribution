using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zed : MonoBehaviour
{
    public GameObject shadow;

    public bool shadowSpawned = false, positionSwitched = false;

    public GameObject go;

    Vector3 playerPos, shadowPos;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!shadowSpawned)
            {
                shadowSpawned = true;
                SpawnShadow();
            }

            else if(shadowSpawned && !positionSwitched)
            {
                positionSwitched = true;
                SwitchPositions();
            }
        }

        if (go == null)
            return;
    }

    void SpawnShadow()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
            go = Instantiate(shadow, raycastHit.point, Quaternion.identity);
    }

    void SwitchPositions()
    {
        playerPos = transform.position;
        shadowPos = go.transform.position;

        transform.position = shadowPos;
        go.transform.position = playerPos;
    }
}
