using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class Comba : MonoBehaviour
{
    public int maxHealth;
    public float damage;//changed from int to float
    public float attackRange;
    public KeyCode blockKey;
    public bool isBlocking = false;
    public TextMeshProUGUI blockText;

    public ParticleSystem slashVFX;
    public VisualEffect newSlash;
    public Transform attackIndicator, cam, player;
    public LayerMask enemy;
    public GameObject a2, a3, a4;

    Cinemachine.CinemachineImpulseSource source;
    Dave dave;
    Dash dash;
    Grappling grappling;

    float idleTimer = 1.5f, animSpeed = 1;
    //[HideInInspector]
    public int lightCounter = 0, heavyCounter = 0;
    float p_damage; //JS.changed from int to float

    //[HideInInspector]
    public bool canAttack, lastComboAttack, maxCombo;
    public float shakeStrength;

    public TextMeshProUGUI lightAttackTracker;

    [Header("Adrenaline")]
    public Slider adrenalineMeter;
    public Slider doubleAdrenalineMeter;
    public float adrenalineValue;
    //[HideInInspector]
    public float value;
    float adrenalineTimer = 2f;

    //sword stuff
    [Header("Blades")]
    public bool vampireBlade;
    public bool freezeBlade;
    public bool beserkBlade;
    public bool enhanceBlade;

    public bool vampireBladeActive;
    public bool freezeBladeActive;
    public bool beserkBladeActive;
    public bool enhanceBladeActive;

    public GameObject EBT;
    public GameObject VBT;
    public GameObject FBT;
    public GameObject BBT;

    public bool doubleAdrenaline;

    [Header("Health Bar")]
    public Slider healthBar;
    float health;

    public EnemyController_P enemyController;
    public SkillTreeUIScript skillTreeUIScript; //JS
    Animator anim;
    public AnimationCurve dodgeCurve;
    float dodgeTimer;

    void Start()
    {

        health = maxHealth;
        adrenalineValue = 0;
        value = 0;
        canAttack = true;

        Cursor.lockState = CursorLockMode.Locked;
        source = GetComponentInParent<Cinemachine.CinemachineImpulseSource>();
        dave = GetComponentInParent<Dave>();
        //dave = GetComponentInChildren<Dave>();
        dash = GetComponentInParent<Dash>();
        grappling = GetComponentInParent<Grappling>();
        anim = GetComponent<Animator>();

        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;

        doubleAdrenaline = false;
    }

    void Update()
    {
        blockText.gameObject.SetActive(isBlocking);
        anim.SetBool("Blocking", isBlocking);

        idleTimer -= Time.deltaTime;
        adrenalineTimer -= Time.deltaTime;

        lightAttackTracker.text = "Counter: " + lightCounter;

        HealthBarSetup();
        AdrenalineMeterSetup();
        Adrenaline();
        CombatInput();
        ComboAttack();
        SpecialMoves();
        //canAttack = !isBlocking;

        if (lastComboAttack || isBlocking || maxCombo)
            canAttack = false;
        else
            canAttack = true;


        if (Input.GetKey(KeyCode.LeftControl))
        {
            isBlocking = true;
            player.transform.rotation = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);
        }
        else
        {
            isBlocking = false;
        }

        if (isBlocking)
            Block();

        if (heavyCounter > 3)
            heavyCounter = 0;

        if (idleTimer <= 0)
        {
            RestartCounters();
        }

        if (adrenalineTimer <= 0f)
        {

            if (skillTreeUIScript.indomitablePerk == true)
            {
                value -= 0f;
            }
            else
            {
                value -= 0.001f;
            }

        }

        if (maxCombo)
            StartCoroutine(MaxComboDelay());

        if (health <= 0)
            health = 0;

        if (health >= maxHealth)
            health = maxHealth;

        if (skillTreeUIScript.healthyPerk == true)
        {
            StartCoroutine("healthIncrease");
        }

        if (skillTreeUIScript.regeneratorPerk == true)
        {
            health += 0.001f;
        }
    }

    public void DoDamage()
    {
        if (canAttack || (!canAttack && lastComboAttack))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackIndicator.position, attackRange, enemy);
            slashVFX.Play();
            newSlash.Play();

            //DashToEnemy();

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.GetComponent<TheBox>() == null)
                    enemy.GetComponent<EnemyController_P>().TakeDamage((int)p_damage); //JS.Added cast to change from float to int
                else if (enemy.GetComponent<EnemyController_P>() == null)
                    enemy.GetComponent<TheBox>().TakeDamage();
                else
                    return;
            }

            if (hitEnemies.Length >= 1)
            {
                adrenalineTimer = 2f;
                source.GenerateImpulse(Camera.main.transform.forward);

                if (skillTreeUIScript.pumpedUpPerk == true)
                {
                    value += 0.2f;
                    adrenalineValue += 0.2f;
                }
                else
                {
                    value += 0.1f;
                    adrenalineValue += 0.1f;
                }

                if (vampireBladeActive == true && skillTreeUIScript.overdrivePerk == true)
                {
                    health += (int)(value * 7);
                }
                else if (vampireBladeActive == true)
                {
                    health += (int)(value * 6);
                }

            }

            if (lightCounter == 5)
                maxCombo = true;
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Au that hurts");
        anim.SetTrigger("Hurt");

        if (beserkBladeActive == true)
        {
            health -= damage * 10;
        }
        if (skillTreeUIScript.toughskinPerk == true)
        {
            health -= (damage - (damage * 0.15f));
        }
        else
        {
            health -= damage;
        }

        if (skillTreeUIScript.steadyMindPerk == true)
        {
            value -= 0.10f;
        }
        else
        {
            value -= 0.15f;
        }

    }

    public void Block()
    {
        Debug.Log("Get Blocked idiot");
    }

    public void Parry()
    {
        Debug.Log("Parry!!");
    }

    void TargetLockOn()
    {

    }

    void ComboAttack()
    {
        if (lightCounter == 2 && heavyCounter == 1)
        {
            RestartCounters();
            a2.SetActive(true);
            if (skillTreeUIScript.jaggedBladePerk == true)
            {
                StartCoroutine("comboBleed");
            }
        }

        if (lightCounter == 3 && heavyCounter == 1)
        {
            RestartCounters();
            a3.SetActive(true);
            if (skillTreeUIScript.jaggedBladePerk == true)
            {
                StartCoroutine("comboBleed");
            }
        }

        if (lightCounter == 4 && heavyCounter == 1)
        {
            RestartCounters();
            a4.SetActive(true);
            if (skillTreeUIScript.jaggedBladePerk == true)
            {
                StartCoroutine("comboBleed");
            }
        }
    }

    void Adrenaline()
    {
        animSpeed = 1 + (value * 0.75f);
        anim.SetFloat("Speed", animSpeed);
        damage = (int)(10 + (value * 10));
    }

    void CombatInput()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            player.transform.rotation = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);
            idleTimer = 1.5f;
            shakeStrength = 2f;

            if (beserkBladeActive == true && skillTreeUIScript.overdrivePerk == true)
            {
                p_damage = damage * 6;
            }
            else if (beserkBladeActive == true)
            {
                p_damage = damage * 5;
            }

            else
            {
                p_damage = damage;
            }
        }

        if (Input.GetMouseButtonDown(1) && canAttack)
        {
            player.transform.rotation = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);
            idleTimer = 1.5f;
            shakeStrength = 3f;

            if (beserkBladeActive == true && skillTreeUIScript.overdrivePerk == true)
            {
                p_damage = damage * 12;
            }
            else if (beserkBladeActive == true)
            {
                p_damage = damage * 10;
            }
            else
            {
                p_damage = damage * 2;
            }
        }
    }

    public void IncrementLight()
    {
        lightCounter++;
        //dave.freeze = true;
    }

    public void IncrementHeavy()
    {
        heavyCounter++;
    }

    public void RestartCounters()
    {
        lightCounter = 0;
        heavyCounter = 0;
    }

    public void CantAttack()
    {
        lastComboAttack = true;
        //dave.freeze = true;
    }

    public void CanAttack()
    {
        lastComboAttack = false;
        //dave.freeze = false;
    }

    public void Unfreeze()
    {
        //dave.freeze = false;
    }

    public void Dodge()
    {
        dave.dodging = true;
        dave.readyToDodge = false;
    }

    IEnumerator ExecuteDodge()
    {
        Dodge();
        float timer = 0f;
        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (dave.transform.forward * dave.desiredMoveSpeed);
            GetComponentInParent<Rigidbody>().AddForce(dir * dave.desiredMoveSpeed * 10f, ForceMode.Force);
            timer += Time.deltaTime;
            yield return null;
        }
        ResetDodge();
    }

    public void ResetDodge()
    {
        dave.dodging = false;
        dave.readyToDodge = true;
    }

    void HealthBarSetup()
    {
        healthBar.value = health;
    }

    void DashToEnemy()
    {
        if (grappling.selectedEnemy != null)
        {
            if (Vector3.Distance(transform.position, grappling.selectedEnemy.transform.position) > 2f)
            {
                dash.DashToEnemy(grappling.selectedEnemy.transform);
            }
        }
    }

    IEnumerator MaxComboDelay()
    {
        canAttack = false;
        lightCounter = 0;
        yield return new WaitForSeconds(0.5f);
        maxCombo = false;
        canAttack = true;
    }

    void AdrenalineMeterSetup()
    {
        if (value >= 0f && value <= 1f)
        {
            adrenalineMeter.value = value;
        }
        else if (doubleAdrenaline == true) //&& value >= 1f)// && value <= 2f)
        {
            doubleAdrenalineMeter.value = value;
        }

        if (value <= 0f)
        {
            value = 0;
        }
        if (doubleAdrenaline == true && value >= 2f)
        {
            value = 2f;
        }
        else if (doubleAdrenaline == false && value >= 1f)
        {
            value = 1f;
        }

    }

    public void SpecialMoves()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enhanceBlade = true;
            vampireBlade = false;
            freezeBlade = false;
            beserkBlade = false;
            EBT.SetActive(true);
            VBT.SetActive(false);
            FBT.SetActive(false);
            BBT.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enhanceBlade = false;
            vampireBlade = true;
            freezeBlade = false;
            beserkBlade = false;
            EBT.SetActive(false);
            VBT.SetActive(true);
            FBT.SetActive(false);
            BBT.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enhanceBlade = false;
            vampireBlade = false;
            freezeBlade = true;
            beserkBlade = false;
            EBT.SetActive(false);
            VBT.SetActive(false);
            FBT.SetActive(true);
            BBT.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            enhanceBlade = false;
            vampireBlade = false;
            freezeBlade = false;
            beserkBlade = true;
            EBT.SetActive(false);
            VBT.SetActive(false);
            FBT.SetActive(false);
            BBT.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (enhanceBlade == true)
            {
                enhanceBladeActive = true;
                StartCoroutine("specialBladeTimer");
                Debug.Log("enhance blade active");
            }
            if (vampireBlade == true)
            {
                vampireBladeActive = true;
                StartCoroutine("specialBladeTimer");
                Debug.Log("vampire blade active");
            }
            if (beserkBlade == true)
            {
                beserkBladeActive = true;
                StartCoroutine("specialBladeTimer");
                Debug.Log("beserk blade active");
            }
            if (freezeBlade == true)
            {
                freezeBladeActive = true;
                StartCoroutine("specialBladeTimer");
                Debug.Log("freeze blade active");
            }

        }

        if (skillTreeUIScript.jaggedBladePerk == true && grappling.grappling == true)
        {
            StartCoroutine("comboBleed");
        }
    }

    IEnumerator specialBladeTimer()
    {
        float duration;
        if (skillTreeUIScript.efficencyPerk == true)
        {
            duration = (value * 12);
        }
        else
        {
            duration = (value * 10);
        }

        if (enhanceBladeActive == true)
        {
            while (duration >= 0)
            {
                doubleAdrenaline = true;
                Debug.Log(value * 10);
                yield return new WaitForSeconds(0.01f);
                duration -= 0.01f; ;
            }
            enhanceBladeActive = false;
            doubleAdrenaline = false;
        }
        if (vampireBladeActive == true)
        {
            while (duration >= 0)
            {
                Debug.Log(value * 10);
                yield return new WaitForSeconds(0.01f);
                duration -= 0.01f;
            }
            vampireBladeActive = false;
        }
        if (beserkBladeActive == true)
        {
            while (duration >= 0)
            {
                Debug.Log(value * 10);
                yield return new WaitForSeconds(0.01f);
                duration -= 0.01f;
            }
            beserkBladeActive = false;
        }
        if (freezeBladeActive == true)
        {
            while (duration >= 0)
            {
                Debug.Log(value * 10);
                yield return new WaitForSeconds(0.01f);
                duration -= 0.01f;
            }
            freezeBladeActive = false;
        }
    }

    IEnumerator healthIncrease()
    {
        maxHealth = maxHealth + 50;
        skillTreeUIScript.healthyPerk = false;
        yield return null;
        StopCoroutine("healthIncrease");
    }

    IEnumerator comboBleed()
    {
        Debug.Log("combo");
        int duration = 15;

        while (duration >= 0)
        {
            //damage = 1;
            //enemyController.TakeDamage(1);
            yield return new WaitForSeconds(5);
            duration--;
            Debug.Log("combo: " + duration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackIndicator.position, attackRange);
    }
}
