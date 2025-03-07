using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public Camera playerCamera;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            Interaction interaction = hit.collider.GetComponent<Interaction>();
            if (interaction != null)
            {
                if (Input.GetKeyDown(KeyCode.E)) //Interact with E key
                {
                    interaction.Interact();
                }
            }
        }
    }
}
