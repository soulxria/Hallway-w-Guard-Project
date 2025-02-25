using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //For UI stamina bar

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float WalkSpeed = 3.0f; //Walk speed to be slower than monster
    public float RunSpeed = 6.0f; //Run speed to be faster than monster
    public float Stamina = 100f; //Max stamina. Will be changed later to make it easier or harder
    public float StaminaDrain = 10f; //How much the stamina will drain per second while running
    public float StaminaRegen = 5f; //How much stamina regen will be per second
    public float StaminaThreshold = 10f; //How much stamina the player needs in order to run again
    public Slider StaminaBar;

    private float CurrentStamina;
    private bool IsRunning = false;

    private Rigidbody rigidbody;
    private Vector3 MoveDirection;

    private float HorizontalInput;
    private float VerticalInput;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        CurrentStamina = Stamina;
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
        HorizontalInput = Input.GetAxis("Horizontal"); //A/D for left and right respectively
        VerticalInput = Input.GetAxis("Vertical"); //W/S for up and down respectively

        IsRunning = Input.GetKey(KeyCode.LeftShift) && CurrentStamina > StaminaThreshold;

        if (IsRunning)
        {
            MoveDirection = new Vector3(HorizontalInput, 0, VerticalInput).normalized * RunSpeed;
        }

        else
        {
            MoveDirection = new Vector3(HorizontalInput, 0, VerticalInput).normalized * WalkSpeed;
        }
    }

        private void MovePlayer()
    {
        Vector3 movement = MoveDirection * Time.fixedDeltaTime;
        rigidbody.MovePosition(transform.position + movement);
    }

    private void HandleStamina()
    {
        if (IsRunning)
        {
            CurrentStamina -= StaminaDrain * Time.deltaTime;
        }

        else
        {
            CurrentStamina += StaminaRegen * Time.deltaTime;
        }

        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, Stamina);
    }

    private void UpdateStaminaUI()
    {
        if (StaminaBar != null)
        {
            StaminaBar.value = CurrentStamina / Stamina; //Update the slider UI with percentage of stamina
        }
    }
}

