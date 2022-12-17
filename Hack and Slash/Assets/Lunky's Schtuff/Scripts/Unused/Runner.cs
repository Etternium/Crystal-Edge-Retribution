using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public float speed = 5f;
    public int platformsToSpawn = 15;

    public GameObject platform;

    public bool allowHorizontalMovement = true;

    public List<GameObject> platforms = new List<GameObject>();

    float zPos = 0, rot = 0f;
    bool creatingSection = false;

    void Start()
    {
        //InvokeRepeating(nameof(Cases), 0f, 0.5f);

        for (int i = 0; i < platformsToSpawn; i++)
        {
            GameObject go = Instantiate(platform);

            if (i == 0)
            {
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            else if(i != 0)
            {
                /*int rnd = Random.Range(0, 3);

                if (rnd == 0)
                {
                    //straight
                    go.transform.position = new Vector3(platforms[i - 1].transform.position.x, platforms[i - 1].transform.position.y, zPos);
                    go.transform.rotation = platforms[i - 1].transform.rotation;
                }

                if (rnd == 1)
                {
                    //left
                    go.transform.position = new Vector3(- 15f, 15f, zPos);
                    go.transform.rotation = Quaternion.Euler(0f, 0f, platforms[i - 1].transform.rotation.z - 90f);
                }

                if (rnd == 2)
                {
                    //right
                    go.transform.position = new Vector3(15f, 15f, zPos);
                    go.transform.rotation = Quaternion.Euler(0f, 0f, platforms[i - 1].transform.rotation.z + 90f);
                }

                go.transform.name = "Plane " + rnd;*/

                go.transform.position = new Vector3(15f, 15f, zPos);
                go.transform.rotation = Quaternion.Euler(0f, 0f, rot);
            }

            zPos += 100;
            rot += 90;
            platforms.Add(go);
        }
    }

    void Update()
    {
        if (allowHorizontalMovement)
        {
            HorizontalMovement();
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if(!creatingSection)
        {
            /*creatingSection = true;
            StartCoroutine(nameof(GeneratePlatform));*/
        }
    }

    IEnumerator GeneratePlatform()
    {
        //zPos += 100;
        Cases();
        yield return new WaitForSeconds(2f);
        creatingSection = false;
    }

    void HorizontalMovement()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
    }

    void Cases()
    {

    }
}
