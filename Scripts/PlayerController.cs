using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    private Lives health;

    public CharacterController characterController;

    private Camera cam;

    public GameObject firstElement; // First projectile character can shoot
    public GameObject secondElement; // Second projectile character can shoot
    public GameObject oppositePlayer; // GameObject of the other player -> will spawn this gameObject if the other player is dead ans the next wave starts

    private Vector2 movementInput;
    private Vector2 mouseRotationInput;
    private Vector2 controllerRotationInput;

    private Vector3 moveVector;

    public float movementSpeed; // Speed of movement
    public float rotationSpeed; // Speed of rotation
    public float waitTime;

    private float controllerDeadzone = 0.1f;

    public int shootCooldown;

    public string firstElementTag; // Tag of the first element the player can shoot
    public string secondElementTag; // Tag of the second element the player can shoot

    public static Vector3 mousePositionLeft;
    public static Vector3 mousePositionRight;
    public static Vector3 aimPositionOne;
    public static Vector3 aimPositionTwo;

    public static bool live2;
    public static bool live1;
    public static bool live0;

    private bool isMovingPressed;
    private bool isWPressed;
    private bool isAPressed;
    private bool isSPressed;
    private bool isDPressed;
    private bool rotationMinus45To45;
    private bool rotation45To135;
    private bool rotation135ToMinus135;
    private bool rotationMinus135ToMinus45;
    private bool mouseCanShoot;
    private bool controllerCanShoot;

    public Transform spawnPoint; // Position of ElementSpawner (Gameobject) where the element will be spawned

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        health = gameObject.GetComponent<Lives>();

        mouseCanShoot = true;
        controllerCanShoot = true;
    }

    void FixedUpdate()
    {
        isMovingPressed = movementInput.x != 0 || movementInput.y != 0; //isMovingPressed is true when the player is pressing WASD
        isWPressed = movementInput.y > 0.7 && movementInput.y <= 1;
        isAPressed = movementInput.x >= -1 && movementInput.x < -0.7;
        isSPressed = movementInput.y >= -1 && movementInput.y < -0.7;
        isDPressed = movementInput.x > 0.7 && movementInput.x <= 1;

        rotationMinus45To45 = transform.rotation.y > -0.35 && transform.rotation.y < 0.35; // rotationMinus45To45 is true when the players y rotation is between -0.35 and 0.35
        rotation45To135 = transform.rotation.y > 0.35 && transform.rotation.y < 0.92; // rotation45To135 is true when the players y rotation is between 0.35 and 0.92
        rotation135ToMinus135 = transform.rotation.y > 0.92 || transform.rotation.y < -0.92; // rotation135ToMinus135 is true when the players y rotation is over 0.92 and under -0.92
        rotationMinus135ToMinus45 = transform.rotation.y > -0.92 && transform.rotation.y < -0.35; // rotationMinus135ToMinus45 is true when the players y rotation is between -0.92 and -0.35

        HandleAnimations();

        // Movement
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement.Normalize();
        characterController.Move(movement * Time.deltaTime * movementSpeed);

        // Adding gravity to our player when it is not grounded
        moveVector = Vector3.zero;

        if (characterController.isGrounded == false) //Checks if character is grounded
        {
            moveVector += Physics.gravity; //Add gravity Vector
        }

        characterController.Move(moveVector * Time.deltaTime); //Apply move Vector

        // Rotation towards mouse position
        Ray ray = cam.ScreenPointToRay(mouseRotationInput);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 rotationPoint = ray.GetPoint(rayLength);
            Quaternion rotation = Quaternion.LookRotation(rotationPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime); // Character rotates from own rotation towards the position of the mouse with a set speed
            transform.LookAt(new Vector3(rotationPoint.x, transform.position.y, rotationPoint.z));
        }

        // Checking if the player lost all lives and if so destroying it
        if (health.lives == 0)
        {
            StartCoroutine(Dying());
        }



        // Rotation with controller
        if (Mathf.Abs(controllerRotationInput.x) > controllerDeadzone || Mathf.Abs(controllerRotationInput.y) > controllerDeadzone) // Checks if the rotation input is bigger than the Deadzone (0.1f)
        {
            Vector3 playerDirection = Vector3.right * controllerRotationInput.x + Vector3.forward * controllerRotationInput.y; // Gets the rotation input and setd it to a Vector3

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up); // newRotation is the position the character should rotate to
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime); // Rotates the character towards newRotation
            }
        }



        /*if (ButtonManager.improveEnemies == true) // If this bool is true it means that the wave is over and the dead character can spawn again
        {
            StartCoroutine(Respawn());
        }*/
    }

    private void HandleAnimations()
    {
        bool isMoving = animator.GetBool("IsMoving"); // Getting access to the bool

        if (isMovingPressed && !isMoving) // Checking if player presses WASD and the character is not moving
        {
            animator.SetBool("IsMoving", true); // Setting bool to true and trigger moving animation
        }
        else if (!isMovingPressed && isMoving) // Checking if player is not pressing WASD and the character is moving
        {
            animator.SetBool("IsMoving", false); // Setting bool to false and stop animation
        }


        if(isWPressed && rotationMinus45To45) // Checks if W is pressed and the player is rotated between -45 and 45 (y-value)
        {
            // Starts animation for walking forward and sets everything else to false
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkForward", true);
        }
        else if (isWPressed && rotation45To135)
        {
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", true);
        }
        else if (isWPressed && rotation135ToMinus135)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", true);
        }
        else if (isWPressed && rotationMinus135ToMinus45)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkRight", true);
        }
        else if (isAPressed && rotationMinus45To45)
        {
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkLeft", true);
        }
        else if (isAPressed && rotation45To135)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkBackward", true);
        }
        else if (isAPressed && rotation135ToMinus135)
        {
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkRight", true);
        }
        else if (isAPressed && rotationMinus135ToMinus45)
        {
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkForward", true);
        }
       else if (isSPressed && rotationMinus45To45)
        {
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", true);
        }
        else if (isSPressed && rotation45To135)
        {
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkRight", true);
        }
        else if (isSPressed && rotation135ToMinus135)
        {
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkForward", true);
        }
        else if (isSPressed && rotationMinus135ToMinus45)
        {
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkLeft", true);
        }
         else if (isDPressed && rotationMinus45To45)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkRight", true);
        }
        else if (isDPressed && rotation45To135)
        {
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkForward", true);
        }
        else if (isDPressed && rotation135ToMinus135)
        {
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", true);
        }
        else if (isDPressed && rotationMinus135ToMinus45)
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkBackward", true);
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void MouseRotation(InputAction.CallbackContext context)
    {
        mouseRotationInput = context.ReadValue<Vector2>();
    }

    public void ControllerRotation(InputAction.CallbackContext context)
    {
        controllerRotationInput = context.ReadValue<Vector2>();
    }

    public void MouseShootFirstElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (mouseCanShoot == true) // Checks if the player can shoot
            {
                GameObject.Instantiate(firstElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element
                StartCoroutine(MouseCooldown());
            }
        }
    }

    public void MouseShootSecondElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (mouseCanShoot == true) // Checks if the player can shoot
            {
                GameObject.Instantiate(secondElement, spawnPoint.position, spawnPoint.rotation ); // Spawning the element
                StartCoroutine(MouseCooldown());
            }
        }
    }

    public void ControllerShootFirstElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (controllerCanShoot == true) // Checks if the player can shoot
            {
                GameObject.Instantiate(firstElement, spawnPoint.position, transform.rotation); // Spawning the element
                StartCoroutine(ControllerCooldown());
            }
        }
    }

    public void ControllerShootSecondElement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (controllerCanShoot == true) // Checks if the player can shoot
            {
                GameObject.Instantiate(secondElement, spawnPoint.position, spawnPoint.rotation); // Spawning the element
                StartCoroutine(ControllerCooldown());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") // Checks if the gameObject the player is colliding with is an enemy
        {
            health.lives -= 1; // Subtracting 1 life
            animator.SetBool("Damage", true);
            StartCoroutine(EndAnimation());

            Destroy(other.gameObject); // Destroys the enemy
        }
        else if (other.tag == "Grandmaster") // Checks if the gameObject the player is colliding with is an enemy
        {
            health.lives -= 1; // Subtracting 1 life
            animator.SetBool("Damage", true);
            StartCoroutine(EndAnimation());
        }
        else if (other.tag == "Element")
        {
            health.lives -= 1;
            animator.SetBool("Damage", true);
            StartCoroutine(EndAnimation());
        }
        else if (other.tag == "Heart")
        {
            if (health.lives < 3)
            {
                health.lives++;
            }

            Destroy(other.gameObject);
        }
        else if (other.tag == "Fire" || other.tag == "Air" || other.tag == "Water" || other.tag == "Earth" || other.tag == "FireArea" || other.tag == "AirArea" || other.tag == "WaterArea" || other.tag == "EarthArea" || other.tag == "Firenado" || other.tag == "Magma" || other.tag == "Ice" || other.tag == "Mud")
        {
            health.lives -= 1;
            animator.SetBool("Damage", true);
            StartCoroutine(EndAnimation());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);

        if (GameObject.FindGameObjectsWithTag("Player").Length == 1) // Checks if there is only one character left, which means one died
        {
            Instantiate(oppositePlayer, new Vector3(-3, 5, -3), Quaternion.identity); // Spawns the opposite character of the character that is still alive
            //oppositePlayer.Lives.lives = 2; // Sets the number of lives of the new spawned character to 1
            ButtonManager.improveEnemies = false; // Sets the boolean to false -> No other character can spawn till it is true again
        }
        else
        {
            ButtonManager.improveEnemies = false; // Sets the boolean to false -> No other character can spawn till it is true again
        }
    }

    IEnumerator Dying()
    {
        animator.SetBool("PlayerDies", true); // Starts the animation
        gameObject.GetComponent<PlayerInput>().enabled = false; // Enables the PlayerInput to make the player unable to move or rotate the character
        yield return new WaitForSeconds(1.2f); // Waits for 1.2 seconds
        Destroy(gameObject); // Destroys the dead player
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Damage", false);
    }

    IEnumerator MouseCooldown()
    {
        mouseCanShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        mouseCanShoot = true;
    }

    IEnumerator ControllerCooldown()
    {
        controllerCanShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        controllerCanShoot = true;
    }
}
