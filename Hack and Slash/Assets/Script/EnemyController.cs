using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //DifficultyUIScript difficultyUIScript;
    GlobalControl globalControl;

    public bool tmpPlayerBlocked;

    public float finalPlayerDamage;
    public float playerDamageResistance;

    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;

    //small enemy stats
    public float smallEnemyHealth;
    public float smallEnemyHealthMax;
    public float smallEnemySpeed;
    public float smallEnemyDamage;
    public float smallEnemyAttackSpeed;
    public int smallI = 0;

    //medium enemy stats
    public float midEnemyHealth;
    public float midEnemySpeed;
    public float midEnemyDamage;
    public float midEnemyAttackSpeed;
    public int midI = 0;

    //large enemy stats
    public float largeEnemyHealth;
    public float largeEnemySpeed;
    public float largeEnemyDamage;
    public float largeEnemyAttackSpeed;
    public int largeI = 0;

    //difficulty

    public float easyDifficulty = 0.75f;
    public float normalDifficulty = 1;
    public float hardDifficulty = 1.25f;

    public Slider slider;
    public GameObject healthBarUI; 
    

    // Start is called before the first frame update
    void Start()
    {
        tmpPlayerBlocked = false;

        difficultyEasy = GlobalControl.Instance.difficultyEasy;
        difficultyNormal = GlobalControl.Instance.difficultyNormal;
        difficultyHard = GlobalControl.Instance.difficultyHard;

        //small enemy
        if (difficultyEasy == true)
        {
            smallEnemyHealth = 5 * easyDifficulty;
            smallEnemyHealthMax = smallEnemyHealth;
            smallEnemyDamage = 1 * easyDifficulty;
            Debug.Log("difficultyEasy");
        }
        else if(difficultyNormal == true)
        {
            smallEnemyHealth = 5 * normalDifficulty;
            smallEnemyDamage = 1 * normalDifficulty;
            Debug.Log("difficultyNormal");
        }
        else if(difficultyHard == true)
        {
            smallEnemyHealth = 5 * hardDifficulty;
            smallEnemyDamage = 1 * hardDifficulty;
            Debug.Log("difficultyHard");
        }
        
        smallEnemySpeed = 10;       
        smallEnemyAttackSpeed = 10;

        //medium enemy
        if (difficultyEasy == true)
        {
            midEnemyHealth = 10 * easyDifficulty;
            midEnemyDamage = 2 * easyDifficulty;
        }
        else if (difficultyNormal == true)
        {
            midEnemyHealth = 10 * normalDifficulty;
            midEnemyDamage = 2 * normalDifficulty;
        }
        else if (difficultyHard == true)
        {
            midEnemyHealth = 10 * hardDifficulty;
            midEnemyDamage = 2 * hardDifficulty;
        }

        midEnemySpeed = 5;
        midEnemyAttackSpeed = 5;

        //large enemy
        if (difficultyEasy == true)
        {
            largeEnemyHealth = 20 * easyDifficulty;
            largeEnemyDamage = 5 * easyDifficulty;
        }
        else if (difficultyNormal == true)
        {
            largeEnemyHealth = 20 * normalDifficulty;
            largeEnemyDamage = 5 * normalDifficulty;
        }
        else if (difficultyHard == true)
        {
            largeEnemyHealth = 20 * hardDifficulty;
            largeEnemyDamage = 5 * hardDifficulty;
        }
        
        largeEnemySpeed = 2;
        largeEnemyAttackSpeed = 2;

        slider.value = CalculateHealth();
}

    // Update is called once per frame
    void Update()
    {
      //  slider.value = CalculateHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tag == "SmallEnemy" && other.tag == "Player")
        {
            StartCoroutine("smallEnemyCoroutine");
            Debug.Log("smallCoStart");
        }

        if (tag == "MediumEnemy" && other.tag == "Player")
        {
            StartCoroutine("midEnemyCoroutine");
            Debug.Log("midCoStart");
        }

        if (tag == "LargeEnemy" && other.tag == "Player")
        {
            StartCoroutine("largeEnemyCoroutine");
            Debug.Log("largeCoStart");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tag == "SmallEnemy" && other.tag == "Player")
        {
            StopCoroutine("smallEnemyCoroutine");
            midI = 0;
            Debug.Log("smallCoEnd");
        }

        if (tag == "MediumEnemy" && other.tag == "Player")
        {
            StopCoroutine("midEnemyCoroutine");
            midI = 0;
            Debug.Log("midCoEnd");
        }

        if (tag == "LargeEnemy" && other.tag == "Player")
        {
             StopCoroutine("largeEnemyCoroutine");
             midI = 0;
             Debug.Log("largeCoEnd");
        }
    }

    float CalculateHealth()
    {
        return smallEnemyHealth / smallEnemyHealthMax;
    }
    IEnumerator smallEnemyCoroutine()
    {

        if (smallI < smallEnemyAttackSpeed)
        {
            smallI++;
            Debug.Log(smallI);
            yield return new WaitForSeconds(1);
        }
        if (smallI == smallEnemyAttackSpeed)
        {
            if (tmpPlayerBlocked == true)
            {
                finalPlayerDamage = 0;
                smallI = 0;
            }

            if (tmpPlayerBlocked == false)
            {
                finalPlayerDamage += smallEnemyDamage - (smallEnemyDamage * playerDamageResistance);
                smallI = 0;
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine("smallEnemyCoroutine");
    }

    IEnumerator midEnemyCoroutine()
    {
        
        if(midI < midEnemyAttackSpeed)
        {
            midI++;
            Debug.Log(midI);
            yield return new WaitForSeconds(1);
        }
        if (midI == midEnemyAttackSpeed)
        {
            if (tmpPlayerBlocked == true)
            {
                finalPlayerDamage = 0;
                midI = 0;
            }

            if (tmpPlayerBlocked == false)
            {
                finalPlayerDamage += midEnemyDamage - (midEnemyDamage * playerDamageResistance);
                midI = 0;
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine("midEnemyCoroutine");
    }

    IEnumerator largeEnemyCoroutine()
    {

        if (largeI < largeEnemyAttackSpeed)
        {
            largeI++;
            Debug.Log(largeI);
            yield return new WaitForSeconds(1);
        }
        if (largeI == largeEnemyAttackSpeed)
        {
            if (tmpPlayerBlocked == true)
            {
                finalPlayerDamage = 0;
                largeI = 0;
            }

            if (tmpPlayerBlocked == false)
            {
                finalPlayerDamage += largeEnemyDamage - (largeEnemyDamage * playerDamageResistance);
                largeI = 0;
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine("largeEnemyCoroutine");
    }
}
