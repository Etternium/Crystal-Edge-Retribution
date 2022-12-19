using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController_P : MonoBehaviour
{
    public GameObject mainCamera;

    public Combat combatScript;
    public Enemy enemy;
    public GameObject bloodSplatter;
    public Transform pos;
    public ParticleSystem bloodVFX;

    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;

    public float TimeStopSpeed;
    public GameObject playerCombat;

    public float easyDifficulty = 0.75f;
    public float normalDifficulty = 1;
    public float hardDifficulty = 1.25f;

    public bool pauseCheck;
    public float startEnemyDMG;
    public float currDMG;

    [Space]
    [Header("Health Bar")]
    // public int currHealth, poise;
    public float currHealth;
    public float healthLost;
    public float startEnemyHealth;
    public float easyHealth;
    public float normalHealth;
    public float hardHealth;
    public int poise;
    float calculateHealth;
    public GameObject healthBarUI;
    public Slider healthBar;
    public Vector3 offset;

    [Space]
    [Header("AI")]
    public float lookRadius = 5f, avoidanceRadius = 4f;
    public GameObject transparentSphere, fillSphere;

    public bool patrolingEnemy;
    public float startWaitTime = 2f;
    public Transform[] patrolPoints;
    int current;
    bool once;
    Vector3[] points;

    float attackTimer, waitTime;
    bool checkForEnemies;

    public Transform player;
    public LayerMask enemyMask;

    Rigidbody rb;
    GameObject[] enemiesNearby;
    Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;

    public State state;
    public enum State
    {
        idle,
        patrol,
        disperse,
        chase,
        attack
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");

        player = PlayerManager.instance.player.transform;
        //combat = player.GetComponent<Combat>();
        difficultyEasy = GlobalControl.Instance.difficultyEasy;
        difficultyNormal = GlobalControl.Instance.difficultyNormal;
        difficultyHard = GlobalControl.Instance.difficultyHard;

        startEnemyHealth = enemy.maxHealth;
        startEnemyDMG = enemy.damage;

        if (difficultyEasy == true)
        {
            currHealth = enemy.maxHealth * easyDifficulty;
            currDMG = enemy.damage * easyDifficulty;
        }
        else if (difficultyHard == true)
        {
            currHealth = enemy.maxHealth * hardDifficulty;
            currDMG = enemy.damage * hardDifficulty;
        }
        else
        {
            currHealth = enemy.maxHealth * normalDifficulty;
            currDMG = enemy.damage * normalDifficulty;
        }

        poise = enemy.startingPoise;

        healthBar.value = CalculateHealth();

        attackTimer = 0;
        waitTime = startWaitTime;

        points = new Vector3[patrolPoints.Length];

        current = 0;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            points[i] = patrolPoints[i].position;
        }
        //agent.SetDestination(points[current]);

        enemiesNearby = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        HealthBarSetUp();
        AI();
        //StateHandler();
        Disperse();

        healthBar.value = CalculateHealth();
        transparentSphere.SetActive(state == State.attack);

        if(poise <= 0)
        {
            anim.ResetTrigger("Hurt");
            anim.SetTrigger("Hurt");
            attackTimer = 0;
            poise = enemy.startingPoise;
        }

        enemiesNearby = GameObject.FindGameObjectsWithTag("Enemy");

        //enemy difficulty changer
        pauseCheck = GlobalControl.Instance.pauseCheck;
        if (pauseCheck == true)
        {
            StartCoroutine("DifficultyChange");
            Debug.Log("Difficulty changed");
        }

        healthBarUI.transform.LookAt(mainCamera.transform);

        /*if (difficultyEasy == true)
        {
            if (currHealth < easyHealth)
            {
                healthLost = easyHealth - currHealth;
            }
        }
        else if (difficultyNormal == true)
        {
            if (currHealth < normalHealth)
            {
                healthLost = normalHealth - currHealth;
            }
        }
        else if (difficultyHard == true)
        {
            if (currHealth < hardHealth)
            {
                healthLost = hardHealth - currHealth;
            }
        }*/
    }

    private void FixedUpdate()
    {
        StateHandler();
    }

    void StateHandler()
    {
        switch(state)
        {
            case State.idle:
                agent.stoppingDistance = 0f;
                Idle();
                break;

            case State.patrol:
                agent.stoppingDistance = 0f;
                Patrol();
                break;

            case State.chase:
                agent.stoppingDistance = enemy.attackRange;
                Chase();
                break;

            case State.attack:
                agent.stoppingDistance = enemy.attackRange;
                agent.isStopped = true;
                StartCoroutine(nameof(Attack));
                break;
        }
    }

    void AI()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        checkForEnemies = Physics.CheckSphere(transform.position, lookRadius + 1, enemyMask);

        if (distance <= lookRadius && distance > agent.stoppingDistance)
        {
            state = State.chase;
        }

        else if (distance <= agent.stoppingDistance)
        {
            state = State.attack;
        }

        else if(patrolingEnemy)
        {
            state = State.patrol;
        }

        else
        {
            state = State.idle;
        }

        if(distance >= (agent.stoppingDistance + 0.25f))
        {
            attackTimer = 0;
        }
    }

    IEnumerator DifficultyChange()
    {

        /*difficultyEasy = GlobalControl.Instance.difficultyEasy;
        difficultyNormal = GlobalControl.Instance.difficultyNormal;
        difficultyHard = GlobalControl.Instance.difficultyHard;

        if (difficultyEasy == true)
        {
            easyHealth = (startEnemyHealth * easyDifficulty);
            currHealth = easyHealth - healthLost;

            currDMG = startEnemyDMG * easyDifficulty;
        }
        else if (difficultyHard == true)
        {
            hardHealth = (startEnemyHealth * hardDifficulty);
            currHealth = hardHealth - healthLost;

            currDMG = startEnemyDMG * hardDifficulty;
        }
        else
        {
            normalHealth = (startEnemyHealth * normalDifficulty);
            currHealth = normalHealth - healthLost;

            currDMG = startEnemyDMG * normalDifficulty;
        }

        pauseCheck = false;
        GlobalControl.Instance.pauseCheck = pauseCheck;
        Debug.Log("difficulty coroutine");*/
        yield return null;
    }

    IEnumerator Attack()
    {
        anim.SetBool("Moving", false);
        attackTimer += Time.deltaTime;
        float angle = Vector3.Angle(player.forward, transform.position - player.position);
        fillSphere.transform.localScale = new Vector3(attackTimer / enemy.attackSpeed, attackTimer / enemy.attackSpeed, attackTimer / enemy.attackSpeed);

        if (attackTimer >= (enemy.attackSpeed - 0.3f))
        {
            if(Input.GetKeyDown(KeyCode.LeftControl) && angle < 90f)
            {
                attackTimer = 0;
                player.GetComponentInChildren<Combat>().Parry();
                currHealth -= 100;
            }
        }

        if (attackTimer >= enemy.attackSpeed)
        {
            anim.SetTrigger("MeleeAttack");
            if(player.GetComponentInChildren<Combat>().isBlocking && angle < 90f)
            {
                Debug.Log("haha xd");
            }

            else if(!player.GetComponent<Dave>().dodging)
            {
                //target.GetComponent<Player>().currentHealth -= damage;
                //player.GetComponentInChildren<Combat>().TakeDamage(currDMG);
                player.GetComponentInChildren<Combat>().TakeDamage(enemy.damage);
            }

            attackTimer = 0;
        }

        yield return new WaitForSeconds(enemy.attackSpeed);
    }

    void Disperse()
    {
        if (checkForEnemies)
        {
            foreach (GameObject enemy in enemiesNearby)
            {
                if (enemy != null)
                {
                    agent.isStopped = false;
                    float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (currentDistance < avoidanceRadius)
                    {
                        Vector3 dist = transform.position - enemy.transform.position;
                        transform.position += dist * Time.deltaTime;
                    }
                }
            }
        }

        else
            return;
    }

    void Patrol()
    {
        agent.SetDestination(points[current]);
        FaceTarget(points[current]);

        if (Vector3.Distance(transform.position, points[current]) <= 1.5f)
        {
            anim.SetBool("Moving", false);
            agent.isStopped = true;
            waitTime -= Time.deltaTime;
        }

        if(waitTime <= 0f)
        {
            anim.SetBool("Moving", true);
            agent.isStopped = false;
            IncreaseCurrent();
            agent.SetDestination(points[current]);
            waitTime = startWaitTime;
        }
    }

    void Chase()
    {
        anim.SetBool("Moving", true);
        agent.isStopped = false;
        FaceTarget(player.position);
        agent.SetDestination(player.position);
    }

    void Idle()
    {
        agent.isStopped = true;
    }

    public void TakeDamage(float damage)
    {
        //Player player = target.GetComponent<Player>();
        //rb = gameObject.AddComponent<Rigidbody>();

        state = State.idle;
        Vector3 pos = (transform.position - player.transform.position).normalized;
        Vector3 newPos = transform.position + pos;
        //transform.position = newPos * 1.05f;
        //Vector3.MoveTowards(transform.position, pos * 40f, 50f);
        //agent.SetDestination(newPos);

        currHealth -= (int)damage;
        poise--;
        /*player.BloodThirst();

        if (!player.wellDepleting)
            player.bloodMeterValue += 2;*/

        //player.CamShake();
        //transform.DOShakeScale(0.1f, 2f);
        bloodVFX.Play();
        Debug.Log("Enemy Au");
        //Destroy(rb);

        if (currHealth <= 0)
            Die();
    }

    void HealthBarSetUp()
    {
        /*healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);

        healthBar.gameObject.SetActive(currHealth < enemy.maxHealth);
        healthBar.value = currHealth;
        healthBar.maxValue = enemy.maxHealth;*/
    }

    void Die()
    {
        //target.GetComponent<Player>().bloodOrbsCount += enemy.bloodOrbs;
        Instantiate(bloodSplatter, pos.position, Quaternion.Euler(90f, Random.Range(0f, 360f), 0f));
        Destroy(gameObject);
    }

    float CalculateHealth()
    {
        if (difficultyEasy == true)
        {
            calculateHealth = easyHealth;
        }
        if (difficultyNormal == true)
        {
            calculateHealth = normalHealth;
        }
        if (difficultyHard == true)
        {
            calculateHealth = hardHealth;
        }
        return currHealth / enemy.maxHealth;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void IncreaseCurrent()
    {
        current++;
        if(current >= patrolPoints.Length)
        {
            current = 0;
        }
    }

    float Angle()
    {
        float angle = Vector3.Angle(player.forward, transform.position - player.position);
        return angle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, enemy.attackRange);
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
