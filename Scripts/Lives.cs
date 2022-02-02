using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    private GameObject heart1; // First heart to show lives of the player
    private GameObject heart2; // Second heart to show lives of the player
    private GameObject heart3; // Third heart to show lives of the player

    public int lives = 3;

    public string heart1Name;
    public string heart2Name;
    public string heart3Name;


    // Start is called before the first frame update
    void Start()
    {
        //lives = 3;

        if (lives == 0)
        {
            lives = 3;
        }

        heart1 = GameObject.Find(heart1Name);
        heart2 = GameObject.Find(heart2Name);
        heart3 = GameObject.Find(heart3Name);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayLives();
    }

    void DisplayLives()
    {
        if (lives == 3)
        {
            heart1.GetComponent<RawImage>().color = Color.white;
            heart2.GetComponent<RawImage>().color = Color.white;
            heart3.GetComponent<RawImage>().color = Color.white;
        }
        if (lives == 2)
        {
            heart1.GetComponent<RawImage>().color = Color.black;
            heart2.GetComponent<RawImage>().color = Color.white;
            heart3.GetComponent<RawImage>().color = Color.white;
        }
        else if (lives == 1)
        {
            heart1.GetComponent<RawImage>().color = Color.black;
            heart2.GetComponent<RawImage>().color = Color.black;
            heart3.GetComponent<RawImage>().color = Color.white;
        }
        else if (lives == 0)
        {
            heart1.GetComponent<RawImage>().color = Color.black;
            heart2.GetComponent<RawImage>().color = Color.black;
            heart3.GetComponent<RawImage>().color = Color.black;
        }
    }

}
