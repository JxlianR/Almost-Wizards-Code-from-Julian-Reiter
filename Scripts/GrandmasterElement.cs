using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterElement : MonoBehaviour
{
    private GrandmasterBehavior grandmaster;

    public GameObject combinedElement1; // First combined element of this element
    public GameObject combinedElement2; // second combined element of this element

    private Transform redPlayer; // Transform of Player 1
    private Transform bluePlayer; // Transform of Player 2
    private Transform lastPlayer; // Transform of the last player in the scene if only one is still alive

    private Vector3 playerPosition;

    public int speed; // Movement speed

    private bool followRed;

    public string elementTag; // Tag of this element
    public string elementAreaTag; // Tag of the area of this element
    public string combiningArea1; // Tag of the first area this element can combine with
    public string combiningArea2; // Tag of the second area this element can combine with

    // Start is called before the first frame update
    void Start()
    {
        grandmaster = gameObject.GetComponent<GrandmasterBehavior>();
        playerPosition = GrandmasterBehavior.playerPosition;

        if (GameObject.FindGameObjectsWithTag("Player").Length == 2) // Checks if both players are alive
        {
            if (GrandmasterBehavior.randomPlayer == 0) // Checks if the player the grandmaster is looking at, is the red player (Player 1)
            {
                followRed = true;
            }
            else if (GrandmasterBehavior.randomPlayer == 1) // Checks if the player the grandmaster is looking at, is the blue player (Player 2)
            {
                followRed = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == 2) // Checks if both players are alive
        {
            redPlayer = GameObject.Find("Red Character").transform; // gets transform of red player and assigns it to variable
            bluePlayer = GameObject.Find("Blue Character").transform; // gets transform of blue player and assigns it to variable

            if (followRed == true) // followRed == true means that the grandmaster is looking at the red player
            {
                transform.position = Vector3.MoveTowards(transform.position, redPlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime); // Moves the element towards the position of the red player and follows it
            }
            else if (followRed == false) // followRed == false means that the grandmaster is looking at the blue player
            {
                transform.position = Vector3.MoveTowards(transform.position, bluePlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime); // Moves the element towards the position of the blue player and follows it
            }
        }
        else // If only one player is alive
        {
            lastPlayer = GameObject.FindGameObjectWithTag("Player").transform; // lastPlayer is the transform of the player that is left
            transform.position = Vector3.MoveTowards(transform.position, lastPlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime); // Moves the element towards the position of the last player and follows it
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == combiningArea1) // Checks if the object is the first area this element can combine with
        {
            Destroy(gameObject); // Destroys this element
            Instantiate(combinedElement1, other.transform.position, combinedElement1.transform.rotation); // Spawns the first combined element
        }
        else if (other.tag == combiningArea2) // Checks if the object is the first area this element can combine with
        {
            Destroy(gameObject); // Destroys this element
            Instantiate(combinedElement2, other.transform.position, combinedElement2.transform.rotation); // Spawns the second combined element
        }
        else if (other.tag != elementTag && other.tag != elementAreaTag) // Checks if the object is not an element own its own type or area of its own type
        {
            Destroy(gameObject); // Destroys this element
        }
    }
}
