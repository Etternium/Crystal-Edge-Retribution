using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Yone : MonoBehaviour
{
    public float duration =  5f;
    public bool walking = false;

    public GameObject anchor;
    public Transform pos;
    public TextMeshProUGUI text;

    GameObject go;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            go = Instantiate(anchor, pos.position, Quaternion.identity);
            walking = true;
        }

        if(walking)
        {
            ShadowWalk();
        }

        text.text = duration.ToString();
        if (go == null)
            return;
    }

    void ShadowWalk()
    {
        duration -= Time.deltaTime;

        if (duration > 0)
            transform.Translate(Vector3.forward * Time.deltaTime * 3f);

        if(duration <= 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, go.transform.position, 20f * Time.deltaTime);

            if (Vector3.Distance(transform.position,go.transform.position) <= 0.1f)
            {
                Destroy(go);
                walking = false;
                duration = 2f;
            }

        }
    }
}
