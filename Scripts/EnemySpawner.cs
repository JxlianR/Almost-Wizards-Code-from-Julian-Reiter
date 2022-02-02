using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] SpawnPoints; // Array with all spawn points

    public GameObject[] Enemies; // Array with all enemies

    public int enemyAmount; // Amount of enemies that should spawn
    public int minTimeBetweenEnemies; // minimum time between 2 enemies
    public int maxTimeBetweenEnemies; // maximum time between 2 enemies

    [HideInInspector]
    public int enemyCount = 0; // Amount of enemies that spawned

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnEnemies()
    {
        while (enemyCount < enemyAmount) // As long as the amount of enemies that should spawn is not reached it will repeat the coroutine and spawn a new enemy
        {
            int randomSpawnWait = Random.Range(minTimeBetweenEnemies, maxTimeBetweenEnemies); // randomSpawnwait will be a random number between minTimeBetweenEnemies and maxTimeBetweenEnemies
            int randomSpawnPoint = Random.Range(0, SpawnPoints.Length); // randomSpawnPoint will be a random number from the SpawnPoints array
            int randomEnemy = Random.Range(0, Enemies.Length); // randomEnemy will be a random number from the Enemies array

            yield return new WaitForSeconds(randomSpawnWait); // Wait a random number (randomSpawnWait)

            Instantiate(Enemies[randomEnemy], SpawnPoints[randomSpawnPoint].position, SpawnPoints[randomSpawnPoint].rotation); // Instantiating the enemy at a random SpawnPoint
            enemyCount++;
        }
    }
}
