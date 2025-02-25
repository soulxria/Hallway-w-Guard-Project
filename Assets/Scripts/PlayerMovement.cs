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

    bool lockedCursor = true;

    public Transform player;
    public Slider staminaBar;

    private float currentStamina;
    private bool isRunning = false;

    private Rigidbody rigidbody;
    private Vector3 MoveDirection;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentStamina = stamina;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleStamina();
        UpdateStaminaUI();
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
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

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
