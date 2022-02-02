using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private GameObject[] Players; // Array with all players
    private GameObject[] Enemies; // Array with all enemies

    private EnemySpawner enemySpawner;

    private Text waveText;

    public int enemyMultiplicator; // Multiplicator for the number of enemies in the next wave
    public int addedEnemies;

    public static int waveNumber = 1; // Number of the wave

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        waveText.text = "Wave " + waveNumber;
    }

    // Update is called once per frame
    void Update()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies and put them in the array
        Players = GameObject.FindGameObjectsWithTag("Player"); // Find all players and put them in the array


        // Checks if all enemies are dead and the number of enemies that should spawn did
        if (AllEnemiesDead() && enemySpawner.enemyCount == enemySpawner.enemyAmount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene

            //playerCanSpawn = true;
            //enemySpawner.enemyCount = 0; // Sets the counter for enemies back to 0
            //addedEnemies = addedEnemies * enemyMultiplicator; // Changes the amount of enemies that are getting add to the number of enemies -> Gets higher after every wave
            //enemySpawner.enemyAmount += addedEnemies; // multiplies the amount of enemies that should spawn with an int
            //StartCoroutine(enemySpawner.SpawnEnemies()); // Starts the coroutine to spawn new enemies

            //waveNumber++;
            //waveText.text = "Wave: " + waveNumber;
            //Debug.Log("Wave " + waveNumber + " starts now!");

        }

        // Loads the scene "Lose" if all players are dead
        if (AllPlayersDead())
        {
            SceneManager.LoadScene("Lose");
        }

        /*if (waveNumber == 7)
        {
            SceneManager.LoadScene("Boss Fight"); // Loads the scene for the boss fight
        }*/
    }

    // Checks if there are any players in the scene
    bool AllPlayersDead()
    {
        foreach (GameObject player in Players)
        {
            if (player != null)
            {
                return false;
            }
        }

        return true;
    }

    // Checks if there are any enemies in the scene
    bool AllEnemiesDead()
    {
        foreach (GameObject enemy in Enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }

        return true;
    }
}
