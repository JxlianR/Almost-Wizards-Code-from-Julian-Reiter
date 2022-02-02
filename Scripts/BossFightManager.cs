using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    private GameObject[] Grandmaster; // Array with the grandmaster
    private GameObject[] Players; // Array with all players

    private Text waveText;

    // Start is called before the first frame update
    void Start()
    {
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
        waveText.text = "Boss Fight";
    }

    // Update is called once per frame
    void Update()
    {
        Grandmaster = GameObject.FindGameObjectsWithTag("Grandmaster"); // Find the grandmaster and put it in the array
        Players = GameObject.FindGameObjectsWithTag("Player"); // Find all players and put them in the array

        // Loads the next scene when the grandmaster is dead
        if (GrandmasterDead())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene
        }

        // Loads the scene "Lose" if all players are dead
        if (AllPlayersDead())
        {
            SceneManager.LoadScene("Lose");
        }
    }


    // Checks if the grandmaster is still in the scene
    bool GrandmasterDead()
    {
        foreach (GameObject grandmaster in Grandmaster)
        {
            if (grandmaster != null)
            {
                return false;
            }
        }

        return true;
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
}
