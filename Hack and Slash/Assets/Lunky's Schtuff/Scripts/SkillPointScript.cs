using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointScript : MonoBehaviour
{
    public SkillTreeUIScript skillTreeUI;
    public GameObject skillPointText;
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        skillTreeUI = GameObject.Find("Pause Menu Canvas").GetComponent<SkillTreeUIScript>();
        skillPointText = GameObject.Find("Skill Point Text");
        skillPointText.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            skillTreeUI.skillTreePoint += 1;
            StartCoroutine("UIText");
        }
    }

    IEnumerator UIText()
    {
        float duration;
        duration = 6;
        while(duration > 0)
        {
            meshRenderer.enabled = false;
            skillPointText.SetActive(true);
            yield return new WaitForSeconds(1);
            duration--;
        }
            skillPointText.SetActive(false);
            Destroy(gameObject);
            yield return null;
    }
}
