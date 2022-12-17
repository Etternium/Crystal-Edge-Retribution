using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(tag == "healingPickup")
        {
            if(playerHealth >= 10)
            {
                playerHealth = 10;
            }
            else
            {
                playerHealth += (playerHealth * 0.25f);
            }
            

        }

        if(tag == "playerDamage")
        {
            if(playerHealth <=0)
            {
                playerHealth = 0;
            }
            else
            {
                playerHealth -= 1;
            }
        }
    }
}
