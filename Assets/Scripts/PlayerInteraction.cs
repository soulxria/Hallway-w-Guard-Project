using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public Transform playerCamera;

    private PlayerMovement playerMovement;

    private Light flashlight;
    private bool hasFlashlight = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); //So PlayerMovement and PlayerInteraction are on same GameObject
        flashlight = GetComponentInChildren<Light>();

        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    private void Update()
    {
        HandleInteraction();

        if (Input.GetKeyDown(KeyCode.F) && hasFlashlight)
        {
            ToggleFlashlight();
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionRange, interactableLayer))
            {
                InteractWithObject(hit.collider);
            }
        }
    }

    private void InteractWithObject(Collider collider)
    {
        if (collider.CompareTag("Key"))
        {
            string keyName = collider.gameObject.name;
            playerMovement.SetKey(keyName);
            Destroy(collider.gameObject);
        }

        else if (collider.CompareTag("Door"))
        {
            string doorName = collider.gameObject.name;
            UnlockDoor(collider, doorName);
        }
    }

    private void UnlockDoor(Collider doorCollider, string doorName)
    {
        string requiredKey = doorName + "Key";

        if (playerMovement.HasKey(requiredKey))
        {
            Debug.Log($"Unlocked the {doorName} with the {requiredKey}");
            doorCollider.gameObject.GetComponent<Collider>().enabled = false;
        }

        else
        {
            Debug.Log($"You need the {requiredKey} to unlock this door");
        }
    }

    private void PickupFlashLight(Collider collider)
    {
        hasFlashlight = true;
        Destroy(collider.gameObject);
        Debug.Log("You picked up the flashlight");
    }

    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
            Debug.Log("Flashlight toggled: " + flashlight.enabled);
        }
    }
}