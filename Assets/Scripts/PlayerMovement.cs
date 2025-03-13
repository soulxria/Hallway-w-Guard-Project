using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; //For UI stamina bar

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public GameManager gameManager; //GameManager reference

    public float lookSpeedX = 2.0f; //Camera movement for left and right
    public float lookSpeedY = 2.0f; //Camera movement for up and down

    public float walkSpeed = 3.0f; //Walking speed
    public float runSpeed = 6.0f; //Running speed
    private bool isRunning = false;
    private Vector3 moveDirection;
    private float cameraVerticalRotation = 0f;

    private float currentStamina;
    public float stamina = 100f; //Max stamina
    public float staminaDrain = 10f; //How much stamina will drain per second while running
    public float staminaRegen = 5f; //Stamina regen per second
    public float staminaThreshold = 10f; //How much stamina is needed in order for player to run again
    public Slider staminaBar; //Setting up stamina bar UI

    public float interactionRange = 3f; //Distance for player to interact with an object (ex: door)
    public LayerMask interactableLayer; //Detecting object that can be picked up

    public Transform playerCamera; //Reference to current player camera (Cinemachine Virtual Camera)

    private Rigidbody rigidbody;

    private HashSet<string> playerKeys = new HashSet<string>(); //Stores the keys that the player is going to pick up

    //variables i'm using for stare of death
    public GameObject enemy;
    public LayerMask targetMask; //will look for Enemy layer :3
    public LayerMask obstructionMask;
    private float sightRadius = 10.0f;
    private float sightAngle = 20.0f;
    private bool enemyDetected;
    public Enemy enemyS;
    public bool hasKey;

    //sound variables
    AudioSource audioSource;
    public AudioClip footstepsWalk;
    public AudioClip footstepsRun;
    public AudioClip keyPickup;

    public bool hasFlashlight = false;
    public GameObject flashlight;
    private bool flashlightOn = false;

    public bool hasMcGuffin = false;

    public bool hasOutsideKey = false;
    public bool hasAtticKey = false;

    public string outsideDoorScene = "FinalExterior";
    public string atticDoorScene = "finalAttic";

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentStamina = stamina;
        enemyS = enemy.GetComponent<Enemy>();
        audioSource = GetComponent<AudioSource>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleStamina();
        UpdateStaminaUI();
        CheckForStaring();
        CheckForKeyPickup();
        CheckForDoorInteraction();
        Flashlight();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMovementInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        transform.Rotate(Vector3.up * mouseX);

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = Camera.main.transform.right;

        moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > staminaThreshold;

        if (isRunning)
        {
            moveDirection *= runSpeed;
            PlaySoundOnce(footstepsRun);
        }

        else
        {
            moveDirection *= walkSpeed;
        }

        transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    void CheckForStaring()
    {
        if (enemy == null)
        {
            return;
        }

        Vector3 myPosition = transform.position;

        Physics.CheckSphere(transform.position, sightRadius, targetMask);

        Vector3 directionToTarget = (enemy.transform.position - transform.position).normalized;

        if (Vector3.Dot(directionToTarget, transform.forward) > Mathf.Cos(sightAngle * 0.5f * Mathf.Deg2Rad))
        {
            //Vector3 toPlayer = PlayerMovement.Instance.transform.position - enemyPosition;
            //toPlayer.y = 0;
            float toPlayer = Vector3.Distance(enemy.transform.position, myPosition);

            if (!Physics.Raycast(transform.position, directionToTarget, toPlayer, obstructionMask))
            {
                enemyDetected = true;
                if (enemyDetected == true)
                {
                    enemyS.preDeathSprint = enemyS.preDeathSprint - Time.deltaTime;
                    if (enemyS.preDeathSprint <= 0)
                    {
                        enemyS.ChaseMode(3);
                    }
                    Debug.Log("" + enemyS.preDeathSprint);
                }
                Debug.Log("enemy found");

            }
            else
                enemyDetected = false;
        }
        else if (!enemyDetected)
            enemyDetected = false;

        if (!enemyDetected)
            enemyS.preDeathSprint = 2.0f;
    }

    private void MovePlayer()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 movement = moveDirection * Time.fixedDeltaTime;
            rigidbody.MovePosition(transform.position + movement);
            PlaySoundOnce(footstepsWalk);
        }
    }

    private void HandleStamina()
    {
        if (isRunning)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
        }

        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina / stamina; //Update the slider UI with percentage of stamina
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            Destroy(this.transform.gameObject);
            GameManager.isGameOver = true;
        }
    }

    public bool HasKey(string keyName)
    {
        return playerKeys.Contains(keyName); //Checking if player has a certain key
    }

    public void SetKey(string keyName)
    {
        playerKeys.Add(keyName); //Adds key to collection (there are no duplicates)
        Debug.Log($"You have picked up the {keyName}");
        audioSource.loop = false;
        PlaySoundOnce(keyPickup);
        audioSource.loop = true;
    }

    private void CheckForKeyPickup()
    {
        if (Input.GetKeyDown(KeyCode.E)) //E to pick up key
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionRange, interactableLayer))
            {
                if (hit.collider.CompareTag("Key")) //If object has the Key tag
                {
                    string keyName = hit.collider.gameObject.name; //Gets the keys name
                    SetKey(keyName); //Sets the player to have this key
                    Destroy(hit.collider.gameObject); //Destroys the key once it is picked up
                }
            }
        }
    }

    private void CheckForDoorInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionRange, interactableLayer))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    if (hit.collider.name == "outsideDoor" && HasKey("outsideKey"))
                    {
                        Debug.Log("Outside door unlocked");
                        StartCoroutine(LoadSceneAfterDelay(outsideDoorScene));
                    }

                    else if (hit.collider.name == "atticDoor" && HasKey("atticKey"))
                    {
                        Debug.Log("Attic door unlocked");
                        StartCoroutine(LoadSceneAfterDelay(atticDoorScene));
                    }

                    else
                    {
                        Debug.Log("Door is locked. Looks like you need a key");
                    }
                }
            }
        }
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f); //Delay to allow unlocking audio
        //SceneManager.LoadScene("finalInterior");
    }

    private void Flashlight()
    {
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        flashlightOn = !flashlightOn;
        flashlight.SetActive(flashlightOn);
        Debug.Log("Flashlight " + (flashlightOn ? "On" : "Off"));
    }

    public void PlaySoundOnce(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}