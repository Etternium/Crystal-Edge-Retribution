using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAndAnimation : MonoBehaviour
{
    public float cooldownTime = 2f, current;
    public string[] attackStrings;
    public AnimationClip[] clips;

    public int noOfClicks = 0;
    float nextFireTime = 0f, lastClickedTime = 0, maxComboDelay = 1;

    AnimatorStateInfo m_CurrentStateInfo;
    AnimatorStateInfo m_NextStateInfo;
    bool m_IsAnimatorTransitioning;
    AnimatorStateInfo m_PreviousCurrentStateInfo;
    AnimatorStateInfo m_PreviousNextStateInfo;
    bool m_PreviousIsAnimatorTransitioning;

    readonly int a1 = Animator.StringToHash("Slash 1");
    readonly int a2 = Animator.StringToHash("Slash 2");
    readonly int a3 = Animator.StringToHash("Slash 3");
    readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
    readonly int m_HashHeavyAttack = Animator.StringToHash("HeavyAttack");
    readonly int m_HashStateTime = Animator.StringToHash("StateTime");

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        //clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            //Debug.Log(clip.length);
        }
    }

    void Update()
    {
        CacheAnimatorState();
        animator.SetFloat(m_HashStateTime, Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));

        if(GetComponent<Combat>().canAttack)
        {
            if (Input.GetMouseButtonDown(0) && GetComponent<Combat>().lightCounter < 5)
            {
                animator.ResetTrigger(m_HashHeavyAttack);
                animator.SetTrigger(m_HashMeleeAttack);
            }

            if(Input.GetMouseButtonDown(1))
            {
                animator.ResetTrigger(m_HashMeleeAttack);
                animator.SetTrigger(m_HashHeavyAttack);
            }
        }
    }

    void CacheAnimatorState() //Not Needed
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = animator.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = animator.IsInTransition(0);
    }

    bool IsWeaponEquipped()
    {
        bool equipped = m_NextStateInfo.shortNameHash == a1 || m_CurrentStateInfo.shortNameHash == a1;
        equipped |= m_NextStateInfo.shortNameHash == a2 || m_CurrentStateInfo.shortNameHash == a2;
        equipped |= m_NextStateInfo.shortNameHash == a3 || m_CurrentStateInfo.shortNameHash == a3;

        return equipped;
    }

    void EquipMeleeWeapon(bool equip)
    {
        /*m_InAttack = false;
        m_InCombo = equip;*/

        if (!equip)
            animator.ResetTrigger(m_HashMeleeAttack);
    }

    void Attack()
    {
        noOfClicks++;
        animator.SetTrigger(attackStrings[noOfClicks - 1]);
        current = 0f;
        if (noOfClicks >= 3)
            noOfClicks = 0;

    }
}
