using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour
{
    public GameObject player;
    public SceneManager sceneManager;
    public string requiredKey;
    public string outsideDoorScene = "finalInterior";
    public string atticDoorScene = "finalAttic";

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Check if the player has the required key in their inventory
            if (player.GetComponent<PlayerMovement>().HasKey(requiredKey))
            {
                Debug.Log($"{requiredKey} door unlocked");

                if (requiredKey == "outsideKey")
                {
                    sceneManager.LoadNextScene("finalInterior");
                }

                else if (requiredKey == "atticKey")
                {
                    sceneManager.LoadNextScene("finalAttic");
                }

                else
                {
                    Debug.Log("This door is locked. You need a key.");
                }
            }
        }
    }
}

