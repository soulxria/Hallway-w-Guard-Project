using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; //only if using text mesh pro

public class ItemPickup : MonoBehaviour
{
    public string itemName; //Setting up item names
    public string outsideKey = "OutsideKey"; //Name for garden key
    public string atticKey = "AtticKey"; //Name for final key
    public GameObject flashlightObject;
    public GameObject mcGuffinObject;
    public SceneManager sceneManager;
    public GameObject player; //Referencing player

    /*public GameObject cutsceneManagerObject;
    private CutsceneManager cutsceneManager;

    private void Start()
    {
        cutsceneManager = cutsceneManagerObject.GetComponent<cutsceneManager>();
    }*/
    private void OnTriggerStay(Collider other) //Allowing player to press E to collect item
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            switch (itemName)
            {
                case "OutsideKey":
                    player.GetComponent<PlayerMovement>().SetKey("outsideKey");
                    Debug.Log("Outside Key collected");
                    Destroy(this.gameObject);
                    break;

                case "AtticKey":
                    player.GetComponent<PlayerMovement>().SetKey("atticKey");
                    Debug.Log("Attic Key collected");
                    Debug.Log("Destroying item: " + itemName);
                    Destroy(this.gameObject);
                    break;

                case "Flashlight":
                    player.GetComponent<PlayerMovement>().hasFlashlight = true;
                    player.GetComponent<PlayerMovement>().flashlight = flashlightObject;
                    flashlightObject.SetActive(false);
                    Debug.Log("Flashlight collected");
                    break;

                case "McGuffin":
                    player.GetComponent<PlayerMovement>().hasMcGuffin = true;
                    Debug.Log("McGuffin collected");
                    sceneManager.LoadNextScene("youWin");
                    
                    break;

                default:
                    Debug.LogWarning("Unknown item type: " + itemName);
                    break;
            }
            
        }
    }


    
}


