using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmasterBehavior : MonoBehaviour
{
    private Animator animator;

    public GameObject[] Elements; // Array with all elements the grandmaster can shoot
    public GameObject[] CombinedElements; // Array with all combined elements that can spawn
    public GameObject[] Enemies; // Array with all enemies the grandmaster can spawn

    private GameObject[] Players; // Array with all players

    public Transform[] EnemySpawnPoints; // Array with spawnpoints for the enemies
    public Transform[] SpawnPoints; // Array with spawnpoints for the grandmaster
    public Transform spawnPoint; // Spawnpoint for the elements

    public ParticleSystem teleport;

    public static Vector3 playerPosition;

    private float maxZ = 27; // Maximal z-value objects can spawn at
    private float maxX = 26; // Maximal x-value objects can spawn at

    public static int randomPlayer;
    public int HP; // Healthpoints of grandmaster
    public int timeBetweenSpell; // Time between spawn of two spells
    public int timeBetweenArea; // Time between spawn of two areas
    public int timeBetweenEnemy; // Time between spawn of enemies

    private bool gotDamage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(Grandmaster());
        StartCoroutine(CombinedAreas());
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        playerPosition = Players[randomPlayer].transform.position;

        transform.LookAt(playerPosition);

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        return hit.point; // returning the position of the ground
    }

    IEnumerator Grandmaster()
    {
        int randomSpawnPoint = Random.Range(0, SpawnPoints.Length); // Gets a random number between 0 and the length of the array and assign it to the variable
        Instantiate(teleport, transform.position, transform.rotation); // Spawns the particle effect for teleporting
        transform.position = SpawnPoints[randomSpawnPoint].position; // Teleports the grandmaster to a random spawnposition

        yield return new WaitForSeconds(2); // Waits for 2 seconds

        for (int i = 0; i < 4; i++) // Does the following loop 4 times
        {
            randomPlayer = Random.Range(0, Players.Length); // Gets a random number between 0 and the length of the array and assign it to the variable

            int randomElement = Random.Range(0, Elements.Length); // Gets a random number between 0 and the length of the array and assign it to the variable
            animator.SetBool("CastingSpell", true); // Starts the animation for casting a spell
            Instantiate(Elements[randomElement], spawnPoint.position, spawnPoint.rotation); // Spawns a random element at a random spawnpoint
            StartCoroutine(EndAnimation()); // Starts Coroutine to end the animation

            yield return new WaitForSeconds(timeBetweenSpell); // Waits a set time before repeating the loop
        }

        StartCoroutine(Grandmaster()); // Starts this Coroutine again
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

        StartCoroutine(CombinedAreas()); // Starts this coroutine again
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(4); 

        for (int i = 0; i < EnemySpawnPoints.Length; i++) // Spawns 4 Enemys at every spawnposition around the grandmaster
        {
            int randomEnemy = Random.Range(0, Enemies.Length); //// Gets a random number between 0 and the length of the array and assign it to the variable
            Instantiate(Enemies[randomEnemy], EnemySpawnPoints[i].position, transform.rotation); // Instantiates a random enemy at one of the spawnpoint
        }

        yield return new WaitForSeconds(timeBetweenEnemy);

        StartCoroutine(SpawnEnemies()); // Starts this coroutine again
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(1.5f); // Waits 1.5 seconds
        animator.SetBool("CastingSpell", false); // Ends the animation for casting a spell
    }

    IEnumerator CanGetDamage()
    {
        yield return new WaitForSeconds(4.3f); // Waits 4.3 seconds
        gotDamage = false; // Enemy can get damage again
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if the grandmaster got damage before and if the other collision object is a player or enemy
        if (gotDamage == false && (other.tag != "Player" || other.tag != "Enemy"))
        {
            HP -= 1; // Subtracts 1 HP
            gotDamage = true;
            StartCoroutine(CanGetDamage()); // Starts Coroutine to set gotDamage to false
        }
    }
}
