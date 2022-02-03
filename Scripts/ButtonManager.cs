using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public static bool improveEnemies; // If the bool is true the enmies can improve (in terms of speed)


    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits"); // Loads the credits
    }

    public void Exit()
    {
        Application.Quit(); // Ends the program
    }

    public void ExitCredits()
    {
        SceneManager.LoadScene("Menu"); // Loads the menu
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu"); // Loads the menu
    }

    public void NextWave()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene

        improveEnemies = true; // Enemies will get faster

        LevelManager.waveNumber++;
    }

    public void BossFight()
    {
        SceneManager.LoadScene("Boss Fight"); // Loads the boss fight
    }
}
