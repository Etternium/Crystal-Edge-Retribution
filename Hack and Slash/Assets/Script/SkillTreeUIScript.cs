using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUIScript : MonoBehaviour
{
    public Button Adrenaline1, Adrenaline2, Adrenaline3, Health1, Health2, Health3, Sword1, Sword2, Sword3;

    public bool pumpedUpPerk, steadyMindPerk, indomitablePerk, toughskinPerk, healthyPerk, regeneratorPerk, efficencyPerk, overdrivePerk, jaggedBladePerk;

    public GameObject a1SkillDesc, a2SkillDesc, a3SkillDesc, h1SkillDesc, h2SkillDesc, h3SkillDesc, s1SkillDesc, s2SkillDesc, s3SkillDesc;

    public int skillTreePoint;

    private void Start()
    {
        Adrenaline2.interactable = false; Adrenaline3.interactable = false; Health2.interactable = false; Health3.interactable = false; Sword2.interactable = false; Sword3.interactable = false;
        skillTreePoint = 0;
        a1SkillDesc.SetActive(false); a2SkillDesc.SetActive(false); a3SkillDesc.SetActive(false); h1SkillDesc.SetActive(false); h2SkillDesc.SetActive(false); h3SkillDesc.SetActive(false); s1SkillDesc.SetActive(false); s2SkillDesc.SetActive(false); s3SkillDesc.SetActive(false);
    }


    public void SteadyMindEnter()
    {
        a1SkillDesc.SetActive(true);
    }

    public void SteadyMindExit()
    {
        a1SkillDesc.SetActive(false);
    }

    public void SteadyMind()
    {
        if (skillTreePoint > 0)
        {
            Adrenaline2.interactable = true;
            steadyMindPerk = true;
            skillTreePoint--;
        }
    }

    public void PumpedUpEnter()
    {
        a2SkillDesc.SetActive(true);
    }

    public void PumpedUpExit()
    {
        a2SkillDesc.SetActive(false);
    }

    public void PumpedUp()
    {
        if(skillTreePoint > 0)
        {
            Adrenaline3.interactable = true;
            pumpedUpPerk = true;
            skillTreePoint--;
        }
    }

    public void IndomitableEnter()
    {
        a3SkillDesc.SetActive(true);
    }

    public void IndomitableExit()
    {
        a3SkillDesc.SetActive(false);
    }

    public void Indomitable()
    {
        if (skillTreePoint > 0)
        {
            indomitablePerk = true;
            skillTreePoint--;
        }
    }

    public void HealthyEnter()
    {
        h1SkillDesc.SetActive(true);
    }

    public void HealthyExit()
    {
        h1SkillDesc.SetActive(false);
    }

    public void Healthy()
    {
        if (skillTreePoint > 0)
        {
            Health2.interactable = true;
            healthyPerk = true;
            skillTreePoint--;
        }
    }

    public void ToughSkinEnter()
    {
        h2SkillDesc.SetActive(true);
    }

    public void ToughSkinExit()
    {
        h2SkillDesc.SetActive(false);
    }


    public void ToughSkin()
    {
        if (skillTreePoint > 0)
        {
            Health3.interactable = true;
            toughskinPerk = true;
            skillTreePoint--;
        }
    }

    public void RegeneratorEnter()
    {
        h3SkillDesc.SetActive(true);
    }

    public void RegeneratorExit()
    {
        h3SkillDesc.SetActive(false);
    }

    public void Regenerator()
    {
        if (skillTreePoint > 0)
        {
            regeneratorPerk = true;
            skillTreePoint--;
        }
    }

    public void EfficencyEnter()
    {
        s1SkillDesc.SetActive(true);
    }

    public void EfficencyExit()
    {
        s1SkillDesc.SetActive(false);
    }

    public void Efficency()
    {
        if (skillTreePoint > 0)
        {
            Sword2.interactable = true;
            efficencyPerk = true;
            skillTreePoint--;
        }
    }

    public void OverdriveEnter()
    {
        s2SkillDesc.SetActive(true);
    }

    public void OverdriveExit()
    {
        s2SkillDesc.SetActive(false);
    }

    public void Overdrive()
    {
        if (skillTreePoint > 0)
        {
            Sword3.interactable = true;
            overdrivePerk = true;
            skillTreePoint--;
        }
    }

    public void JaggedBladeEnter()
    {
        s3SkillDesc.SetActive(true);
    }

    public void JaggedBladeExit()
    {
        s3SkillDesc.SetActive(false);
    }

    public void JaggedBlade()
    {
        if (skillTreePoint > 0)
        {
            jaggedBladePerk = true;
            skillTreePoint--;
        }
    }
}
