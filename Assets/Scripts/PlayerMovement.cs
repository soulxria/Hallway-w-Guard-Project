using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //For UI stamina bar

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float walkSpeed = 3.0f; //Walk speed to be slower than monster
    public float runSpeed = 6.0f; //Run speed to be faster than monster
    public float stamina = 100f; //Max stamina. Will be changed later to make it easier or harder
    public float staminaDrain = 10f; //How much the stamina will drain per second while running
    public float staminaRegen = 5f; //How much stamina regen will be per second
    public float staminaThreshold = 10f; //How much stamina the player needs in order to run again
    public float mouseSensitivity = 2f; //How senstive the camera mouse movement will be
    float cameraVerticalRotation = 0f; //Setting up how the variable will handle movement
    float cameraHorizontalRotation = 0f; //Setting up for the horizontal movement

    bool lockedCursor = true;

    public Transform player;
    public Slider staminaBar;

    private float currentStamina;
    private bool isRunning = false;

    private Rigidbody rigidbody;
    private Vector3 MoveDirection;

    private float horizontalInput;
    private float verticalInput;

    //variables i'm using for stare of death
    public GameObject enemy;
    public LayerMask targetMask; //will look for Enemy layer :3
    public LayerMask obstructionMask;
    private float sightRadius = 10.0f;
    private float sightAngle = 20.0f;
    private bool enemyDetected;
    Enemy enemyS;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentStamina = stamina;
        enemyS = enemy.GetComponent<Enemy>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleStamina();
        UpdateStaminaUI();
        CheckForStaring();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMovementInput()
    {
        //Mouse and key inputs
        horizontalInput = Input.GetAxis("Horizontal"); //A/D for left and right respectively
        verticalInput = Input.GetAxis("Vertical"); //W/S for forward and backwards respectively
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity; //Looking left and right
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity; //Looking up and down

        //Rotating the camera around the X axis
        cameraVerticalRotation -= inputY;
        cameraHorizontalRotation -= inputX;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        cameraHorizontalRotation = Mathf.Clamp(cameraHorizontalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;
        transform.localEulerAngles = Vector3.up * cameraHorizontalRotation;

        //Rotating the player object and the camera around the Y axis
        player.Rotate(Vector3.up * inputX);

        isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > staminaThreshold;

        if (isRunning)
        {
            MoveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized * runSpeed;
        }

        else
        {
            MoveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized * walkSpeed;
        }
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
        Vector3 movement = MoveDirection * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + movement);
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
            GameManager.GameOver();
        }
    }
}
