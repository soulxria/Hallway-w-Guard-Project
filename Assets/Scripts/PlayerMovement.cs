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
        horizontalInput = Input.GetAxis("Horizontal"); //A/D for left and right respectively
        verticalInput = Input.GetAxis("Vertical"); //W/S for up and down respectively

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

