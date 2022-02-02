using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCombinedElements : MonoBehaviour
{
    public GameObject[] CombinedElements; // Area with all combined elements

    private float maxZ = 27; // Maximal z-value objects can spawn at
    private float maxX = 26; // Maximal x-value objects can spawn at

    public int timeBetweenArea; // Time between spawn of two areas

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CombinedAreas()); // Starts coroutine to spawn combined elements
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        return hit.point; // returning the position of the ground
    }

    IEnumerator CombinedAreas()
    {
        float randomX = Random.Range(maxX, -maxX); // Random number between 26 & -26
        float randomZ = Random.Range(maxZ, -maxZ); // Random number between 27 & -27
        int randomCombinedElement = Random.Range(0, CombinedElements.Length); // Gets a random number between 0 and the length of the array with combined elements

        if (DetectGroundHeight(randomX, randomZ).y <= 1) // Checks if the y-value of the the position at randomX and randomZ is smaller than 1 -> Yes means there is no other object on the ground
        {
            Instantiate(CombinedElements[randomCombinedElement], DetectGroundHeight(randomX, randomZ) + new Vector3(0, 0.5f, 0), transform.rotation); // Spawns the combined elements
            yield return new WaitForSeconds(timeBetweenArea); // Waits a set time
        }

        StartCoroutine(CombinedAreas()); // Starts the coroutine again
    }
}
