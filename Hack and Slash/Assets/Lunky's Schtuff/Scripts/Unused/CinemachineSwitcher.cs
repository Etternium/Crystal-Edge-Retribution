using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    int i;

    void Start()
    {
        i = 0;
    }

    void Update()
    {
        if (i == cameras.Length)
            i = 0;

        if(Input.GetMouseButtonDown(0))
        {
            SwitchState();
            //SwitchCam();
        }
    }

    void SwitchState()
    {
        if (i == cameras.Length - 1)
        {
            cameras[cameras.Length - 1].Priority = 0;
            cameras[0].Priority = 1;
        }
        else if (i != cameras.Length - 1)
        {
            cameras[i].Priority = 0;
            cameras[i + 1].Priority = 1;
        }

        i++;
    }
}
