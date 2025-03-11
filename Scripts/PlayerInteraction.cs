using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public Transform playerCamera;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); //So PlayerMovement and PlayerInteraction are on same GameObject
    }

    private void Update()
    {
        HandleInteraction();
        InteractWithObject();
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
}
