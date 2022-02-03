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

    public static int waveNumber = 1; // Number of the wave

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        waveText.text = "Wave " + waveNumber; // Sets the wavetext to the correct text
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
        }

        // Loads the scene "Lose" if all players are dead
        if (AllPlayersDead())
        {
            SceneManager.LoadScene("Lose"); // Loads the "Lose" scene
        }
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
