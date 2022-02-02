using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AiNavDarkEnemy : MonoBehaviour
{
    public NavMeshAgent Agent;

    private Animator animator;

    private GameObject[] Players; // Array with all Players

    private Transform closestPlayer;

    public GameObject heart;

    public int healthPoints;

    public float timeTillDeath = 0.5f;

    private bool gotDamage;

    // Start is called before the first frame update
    void Start()
    {
        closestPlayer = null;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        closestPlayer = getClosestPlayer();
        Movement();
        Rotation();

        // Destroy the enemy when the HP is 0
        if (healthPoints <= 0)
        {
            StartCoroutine(Die());
            //Destroy(gameObject);
        }

        if (ButtonManager.improveEnemies == true)
        {
            gameObject.GetComponent<AiNavDarkEnemy>().Agent.speed += 0.6f;
            ButtonManager.improveEnemies = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Firenado" || other.tag == "Magma" || other.tag == "Ice" || other.tag == "Mud") && gotDamage == false)
        {
            healthPoints -= other.gameObject.GetComponent<ElementAreaBehavior>().Damage; // Substracts the damage the element is doing from the HP
            gotDamage = true; // true means the enemy got damage a short time ago
            StartCoroutine(CanGetDamage());
        }
        else if (other.tag != "Player" && other.tag != "Grandmaster" && gotDamage == false)
        {
            healthPoints -= 1; // Substracts 1 HP
            Debug.Log("Healtpoints = " + healthPoints + " - " + FireElement.damage);
            gotDamage = true; // true means the enemy got damage a short time ago
            StartCoroutine(CanGetDamage());
        }
    }

    private void Movement()
    {
        //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime); // Moving the enemy forwards#
        Agent.SetDestination(closestPlayer.position);
    }

    private void Rotation()
    {
        // Rotating the enemy to the position of target GameObject
        transform.LookAt(closestPlayer);
    }

    // Dropping a heart to gain life for the player
    private void Drop()
    {
        int dropChance = 5; // 5% chance of dropping a heart
        int randomChance = Random.Range(0, 101); // Getting a random number between 0 and 100 (101 is exclusive)

        if (dropChance >= randomChance)
        {
            Instantiate(heart, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity); // Spawning the heart
        }
    }

    public Transform getClosestPlayer()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity; // Setting a very high number to this variable
        Transform playerPosition = null;

        foreach (GameObject player in Players)
        {
            float currentDistance = Vector3.Distance(transform.position, player.transform.position); // Setting the distance of this gameobject and the player to the varibale currentDistance

            if (currentDistance < closestDistance) // Checking if the currentDistance is smaller than the closestDistance (which is a very high number)
            {
                closestDistance = currentDistance; // Setting the closestDistance to the currentDistance
                playerPosition = player.transform; // Seeting the Transform playerPosition to the position of the player
            }
        }
        return playerPosition;
    }

    IEnumerator CanGetDamage()
    {
        yield return new WaitForSeconds(0.5f);
        gotDamage = false; // Enemy can get damage again
    }

    IEnumerator Die()
    {
        animator.SetBool("Dying", true);
        GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
        Drop();
    }
}
