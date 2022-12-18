using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointScript : MonoBehaviour
{
    public SkillTreeUIScript skillTreeUI;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            skillTreeUI.skillTreePoint += 1;
            Destroy(gameObject);
        }
    }
}
