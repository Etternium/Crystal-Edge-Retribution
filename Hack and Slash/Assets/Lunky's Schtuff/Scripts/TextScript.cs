using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{

    public GameObject a1SkillDesc;
    // Start is called before the first frame update
    void Start()
    {
        a1SkillDesc.SetActive(false);
    }

    public void OnMouseOver()
    {
        a1SkillDesc.SetActive(true);
    }

    public void OnMouseExit()
    {
        a1SkillDesc.SetActive(false);
    }
}
