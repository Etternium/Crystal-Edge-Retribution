using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public int[] normalEnemiesPerWave;
    public int[] rangedEnemiesPerWave;
    public int[] beefyEnemiesPerWave;
    public GameObject normalEnemyPrefab, rangedEnemyPrefab, beefyEnemyPrefab;
    public LayerMask enemyMask;
    public float radius = 20f;
    public TextMeshProUGUI waveText;

    int z = 0;
    bool active = false;
    Collider[] enemies;
    List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        waveText.gameObject.SetActive(false);
        
        //enemies = Physics.OverlapSphere(transform.position, 20f, enemyMask);
    }

    void Update()
    {
        enemies = Physics.OverlapSphere(transform.position, radius, enemyMask);

        if (enemies.Length <= 0 && z < normalEnemiesPerWave.Length && active)
        {
            StartCoroutine(nameof(SpawnDelay));
        }

        else if(z == normalEnemiesPerWave.Length)
        {
            
            /*waveText.gameObject.SetActive(true);
            StartCoroutine(nameof(DisableText));*/
        }

        else if (enemies.Length > 0 || z == normalEnemiesPerWave.Length)
            return;
    }

    void SpawnAll()
    {
        Spawn(normalEnemyPrefab, normalEnemiesPerWave);
        Spawn(rangedEnemyPrefab, rangedEnemiesPerWave);
        Spawn(beefyEnemyPrefab, beefyEnemiesPerWave);
    }

    IEnumerator SpawnDelay()
    {
        enemyList.Clear();
        if (z < normalEnemiesPerWave.Length -1)
        {
            waveText.text = "Wave " + (z + 2) + " incoming";
        }

        else if (z == normalEnemiesPerWave.Length - 1)
        {
            waveText.text = "Combat arena cleared";
            //add skill points either here or below
        }

        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);

        if (z < normalEnemiesPerWave.Length)
        {
            z++;
            waveText.text = "Wave " + (z + 1) + " incoming";
        }

        else if (z == normalEnemiesPerWave.Length)
        {
            z += 0;
            waveText.text = "Combat arena cleared";
            //add skill points either here or above
        }
        waveText.gameObject.SetActive(false);
        
        SpawnAll();
        StopAllCoroutines();
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
        waveText.gameObject.SetActive(false);
    }

    void Spawn(GameObject enemy, int[] wave)
    {
        for (int i = 0; i < wave[z]; i++)
        {
            GameObject go = Instantiate(enemy, transform.position, Quaternion.identity);
            enemyList.Add(go);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !active)
        {
            waveText.text = "You entered the combat arena. Defeat all enemies";
            waveText.gameObject.SetActive(true);
            active = true;
            StartCoroutine(nameof(DisableText));
            SpawnAll();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
