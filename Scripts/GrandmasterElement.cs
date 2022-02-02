using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterElement : MonoBehaviour
{
    private GrandmasterBehavior grandmaster;

    public GameObject combinedElement1;
    public GameObject combinedElement2;

    private Transform redPlayer;
    private Transform bluePlayer;
    private Transform lastPlayer;

    private Vector3 playerPosition;

    public int speed;

    private bool followRed;

    public string elementTag;
    public string elementAreaTag;
    public string combiningArea1;
    public string combiningArea2;

    // Start is called before the first frame update
    void Start()
    {
        grandmaster = gameObject.GetComponent<GrandmasterBehavior>();
        //playerPosition = grandmaster.Players[grandmaster.randomPlayer];
        playerPosition = GrandmasterBehavior.playerPosition;

        if (GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
            if (GrandmasterBehavior.randomPlayer == 0)
            {
                followRed = true;
            }
            else if (GrandmasterBehavior.randomPlayer == 1)
            {
                followRed = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
            redPlayer = GameObject.Find("Red Character").transform;
            bluePlayer = GameObject.Find("Blue Character").transform;

            if (followRed == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, redPlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime);
            }
            else if (followRed == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, bluePlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime);
            }
        }
        else
        {
            lastPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = Vector3.MoveTowards(transform.position, lastPlayer.position + new Vector3(0, 1.5f, 0), speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == combiningArea1)
        {
            Destroy(gameObject);
            Instantiate(combinedElement1, other.transform.position, combinedElement1.transform.rotation);
        }
        else if (other.tag == combiningArea2)
        {
            Destroy(gameObject);
            Instantiate(combinedElement2, other.transform.position, combinedElement2.transform.rotation);
        }
        else if (other.tag != elementTag && other.tag != elementAreaTag)
        {
            Destroy(gameObject);
        }
    }
}
