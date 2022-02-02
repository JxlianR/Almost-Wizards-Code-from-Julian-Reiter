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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitCredits()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextWave()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        improveEnemies = true;

        LevelManager.waveNumber++;
    }

    public void BossFight()
    {
        SceneManager.LoadScene("Boss Fight");
    }
}
