using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public Combat combat;
    public Type attackType;

    float timer;

    public enum Type { basic, singleSlash};

    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Disable(0.5f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            switch (attackType)
            {
                case Type.basic:
                    if(combat.beserkBladeActive == true)
                    {
                        other.gameObject.GetComponent<EnemyController_P>().TakeDamage(combat.damage * 5f);
                    }
                    else
                    {
                        other.gameObject.GetComponent<EnemyController_P>().TakeDamage(combat.damage * 2f);
                    }

                    break;

                case Type.singleSlash:
                    if(combat.beserkBladeActive == true)
                    {
                        other.gameObject.GetComponent<EnemyController_P>().TakeDamage(combat.damage * 6f);
                    }
                    else
                    {
                        other.gameObject.GetComponent<EnemyController_P>().TakeDamage(combat.damage * 3f);
                    }
                    break;
            }
        }
    }

    IEnumerator Disable(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    void Slash()
    {
        timer = 0.1f;
        gameObject.SetActive(true);
        timer -= Time.deltaTime;

        if (timer <= 0f)
            gameObject.SetActive(false);

        Debug.Log("void done");
    }

    public IEnumerator Strash()
    {
        gameObject.SetActive(true);
        Debug.Log("ACtiove");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        Debug.Log("Gone");
        yield return new WaitForSeconds(1);
        Debug.Log("Done");
        StartCoroutine(Strash());
    }
}
